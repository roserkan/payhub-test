using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.Deposits;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.RequestModels;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.ResponseModels;
using Payhub.Application.Features.Deposits.Commands.Create;
using Payhub.Application.Features.Deposits.Events;
using Payhub.Domain.Entities.AccountManagement;
using Payhub.Domain.Entities.AffiliateManagement;
using Payhub.Domain.Entities.SiteManagement;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Shared.Utils.Helpers;
using Shared.Utils.Security.Hashing;

namespace Payhub.Application.Features.Deposits.Commands.CreateForAccount;


// Burada deposit oluşturulunca normal akıştaki gibi paymentLink dönmez. Direkt hesap döndürürüz. Biz bir yerin bayisiysek bunu veriyoruz.
// Buradaki tüm kontroller vs. Create ile aynıdır. Bir değişiklik olacağonda ikisinde birden yapılmalıdır.
// Burası Create ile birebir aynıdır.

public sealed class CreateDepositForAccountCommandHandler : ICommandHandler<CreateDepositForAccountCommand, CreatedDepositForAccountDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IPayoxService _payoxService;
    
    public CreateDepositForAccountCommandHandler(IUnitOfWork unitOfWork, 
        IHttpContextAccessor httpContextAccessor,
        ISendEndpointProvider sendEndpointProvider,
        IPayoxService payoxService)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _sendEndpointProvider = sendEndpointProvider;
        _payoxService = payoxService;
    }
    
    public async Task<CreatedDepositForAccountDto> Handle(CreateDepositForAccountCommand request, CancellationToken cancellationToken)
    {
        var createdDepositEvent = await CheckDeposit(new()
        {
            CustomerId = request.CustomerId,
            CustomerFullName = request.CustomerFullName,
            CustomerUserName = request.CustomerUserName,
            CustomerSignupDate = request.CustomerSignupDate,
            CustomerIpAddress = request.CustomerIpAddress,
            CustomerIdentityNumber = request.CustomerIdentityNumber,
            ProcessId = request.ProcessId,
            Amount = request.Amount,
            RedirectUrl = request.RedirectUrl,
            PaymentWayType = request.PaymentWayType
        }, cancellationToken);
        
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:deposit-event-queue"));
        await endpoint.Send(createdDepositEvent);
        
        if (createdDepositEvent.DynamicAccountName == null || createdDepositEvent.DynamicAccountNumber == null)
        {
            throw new BusinessException(ErrorMessages.Deposits_AccountNotFound);
        }
        
        return new CreatedDepositForAccountDto
        {
            AccountName = createdDepositEvent.DynamicAccountName ?? createdDepositEvent.AccountName,
            Iban = createdDepositEvent.DynamicAccountNumber ?? createdDepositEvent.AccountNumber,
        };
    }
    
    // Burası 1-1 Create ile aynıdır
     private async Task<CreatedDepositEvent> CheckDeposit(CreateDepositCommand request, CancellationToken cancellationToken)
    {
        // ApiKey ve SecurityKey Alınması
        var apiKey = HeaderHelper.GetApiKey(_httpContextAccessor.HttpContext!);
        var securityKey = HeaderHelper.GetSecurityKey(_httpContextAccessor.HttpContext!);
        
        // Yatırımın geldiği site, sitenin ödeme yöntemleri ve siteye bağlı hesapların alınması
        var site = await _unitOfWork.SiteRepository.Query()
            .Select(s => new Site
            {
                Id = s.Id,
                Name = s.Name,
                SitePaymentWays = s.SitePaymentWays
                    .Where(spw => spw.ApiKey == apiKey && spw.PaymentWayId == request.PaymentWayType)
                    .Select(spw => new SitePaymentWay
                    {
                        Id = spw.Id,
                        ApiKey = spw.ApiKey,
                        SecretKey = spw.SecretKey,
                        PaymentWayId = spw.PaymentWayId,
                        IsActive = spw.IsActive,
                        Commission = spw.Commission
                    })
                    .ToList(),
                AccountSites = s.AccountSites
                    .Where(accSite => accSite.Account.IsActive &&
                                      accSite.Account.MinDepositAmount <= request.Amount &&
                                      accSite.Account.MaxDepositAmount >= request.Amount &&
                                      (accSite.Account.AccountType == AccountType.UstYatirim ||
                                       accSite.Account.AccountType == AccountType.Yatirim) &&
                                      accSite.Account.PaymentWayId == request.PaymentWayType)
                    .Select(accSite => new AccountSite
                    {
                        // AccountSite properties
                        Account = new Account
                        {
                            Id = accSite.Account.Id,
                            AffiliateId = accSite.Account.AffiliateId,
                            IsActive = accSite.Account.IsActive,
                            MinDepositAmount = accSite.Account.MinDepositAmount,
                            MaxDepositAmount = accSite.Account.MaxDepositAmount,
                            AccountType = accSite.Account.AccountType,
                            PaymentWayId = accSite.Account.PaymentWayId,
                            Affiliate = accSite.Account.Affiliate != null ? new Affiliate
                            {
                                Id = accSite.Account.Affiliate.Id,
                                Name = accSite.Account.Affiliate.Name,
                                IsDynamic = accSite.Account.Affiliate.IsDynamic
                            } : null
                        }
                    })
                    .ToList()
            }).FirstOrDefaultAsync(i => i.SitePaymentWays.Any(i => i.ApiKey == apiKey), cancellationToken: cancellationToken);
        if (site == null)
            throw new NotFoundException(ErrorMessages.Deposits_SiteNotFound);

        // Sitenin bu yatırım yöntemini desteklemesi kontrolü
        var spw = site.SitePaymentWays.FirstOrDefault(i => i.IsActive);
        if (spw == null)
            throw new BusinessException(ErrorMessages.Deposits_PaymentWayInactive);

        // Güvenlik Anahtarı Kontrolü
        var hashed = HashingHelper.Sha256Hash(apiKey + spw.SecretKey + request.ProcessId);
        if (hashed != securityKey)
            throw new BusinessException(ErrorMessages.Deposits_InvalidSecurityKey);

        // Aktif talep kontrolü
        var activeDepositRequest = await _unitOfWork.DepositRepository.AnyAsync(d
                => d.PanelCustomerId == request.CustomerId + site.Name && 
            (d.Status == DepositStatus.PendingDeposit || d.Status == DepositStatus.PendingConfirmation),
            cancellationToken: cancellationToken);
        if (activeDepositRequest)
            throw new BusinessException(ErrorMessages.Deposits_HasAlreadyActiveDeposit);

        // Karalisteye alınmış bir müşteri olup olmadığının kontrolü
        var isBanned = await _unitOfWork.BlacklistRepository.AnyAsync(b =>
            b.BlacklistType == BlacklistType.PanelCustomerId &&
            b.Value == request.CustomerId + site.Name, cancellationToken: cancellationToken);
        if (isBanned)
            throw new BusinessException(ErrorMessages.Deposits_Blacklisted);

        // Uygun hesapların alınması
        var availableAccounts = site.AccountSites.Select(i => i.Account);
        Account? selectedAccount = null;
        // Eğer yatırım 100k altındaysa hesap otomatik seçilir
        if (request.Amount <= 100000)
        {
            selectedAccount =
                availableAccounts.FirstOrDefault(i =>
                    i.AffiliateId != null) ?? // Hesabın bayisi varsa o bayinin hesabı alınır.
                availableAccounts.FirstOrDefault() ?? // Bayisi yoksa herhangi bir hesap alınır.
                throw new NotFoundException(ErrorMessages.Deposits_AccountNotFound);  // Hesap bulunamazsa hata fırlatılır.
        }
        
        var result = new CreatedDepositEvent
        {
            DepositRequest = request,
            SiteId = site.Id, 
            SiteName = site.Name,
            AccountId = selectedAccount?.Id,
            PaymentId = Guid.NewGuid().ToString(),
            Commission = spw.Commission,
            CreatedDate = DateTime.Now,
            DynamicAccountName = null,
            DynamicAccountNumber = null,
        };

        if (selectedAccount != null && selectedAccount.AffiliateId != null && selectedAccount.Affiliate!.IsDynamic)
        {
            // Burada eğer yatırım bayiye yönlenmiş ve bayi hesapları dinamik ise bayinin hesabını almak için işlem yapmalıyız.
            
            // Dinamik Bayi Payox ise
            if (selectedAccount.Affiliate.Name == "Payox")
            {
                var payoxResponse = await PayoxAffiliateDeposit(request.ProcessId, request.PaymentWayType, request.Amount,
                    request.CustomerFullName, request.CustomerUserName, request.CustomerId);
                result.DynamicAccountName = payoxResponse.BankAccountName;
                result.DynamicAccountNumber = payoxResponse.BankAccountIban;                
            }
        }

        return result;
    }

    private async Task<PayoxCreateDepositResponse> PayoxAffiliateDeposit(string processId, 
        int paymentWayId, decimal amount, string? customerFullName, string? customerUserName, string? customerId)
    {
        var availableBanks = await _payoxService.GetAvailableBanksAsync(paymentWayId);
        var bank = availableBanks.Data.FirstOrDefault(b => b.Id.Length > 4) ??
                   availableBanks.Data.FirstOrDefault();
        
        var request = new PayoxCreateDepositRequest
        {
            BankId = bank!.Id,
            ProcessId = processId,
            Amount = amount,
            Name = customerFullName ?? string.Empty,
            UserName = customerUserName ?? string.Empty,
            UserId = customerId ?? string.Empty
        };
        
        var result = await _payoxService.CreateDepositAsync(request, paymentWayId);
        return result.Data;
    }
}