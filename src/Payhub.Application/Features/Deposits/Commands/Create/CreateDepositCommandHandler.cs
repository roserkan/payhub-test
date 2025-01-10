using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.Deposits;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.RequestModels;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.ResponseModels;
using Payhub.Application.Features.Deposits.Events;
using Payhub.Domain.Entities.AccountManagement;
using Payhub.Domain.Entities.AffiliateManagement;
using Payhub.Domain.Entities.SiteManagement;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Shared.Utils.Helpers;
using Shared.Utils.Security.Hashing;

namespace Payhub.Application.Features.Deposits.Commands.Create;

public sealed class CreateDepositCommandHandler : ICommandHandler<CreateDepositCommand, CreatedDepositDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IPayoxService _payoxService;

    public CreateDepositCommandHandler(IUnitOfWork unitOfWork, 
        IHttpContextAccessor httpContextAccessor,
        ISendEndpointProvider sendEndpointProvider,
        IPayoxService payoxService)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _sendEndpointProvider = sendEndpointProvider;
        _payoxService = payoxService;
    }

    public async Task<CreatedDepositDto> Handle(CreateDepositCommand request, CancellationToken cancellationToken)
    {

        var createdDepositEvent = await CheckDeposit(request, cancellationToken); // 300ms
        
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:deposit-event-queue-test"));
        await endpoint.Send(createdDepositEvent);
        
        return new CreatedDepositDto
        {
            PaymentLink = "https://payment.sipsakpay.com/payment/" + createdDepositEvent.PaymentId,
            ProcessId = request.ProcessId,
            Success = true
        };
    }

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
                AffiliateSites = s.AffiliateSites
                    .Where(affSite => affSite.Affiliate.IsDepositActive &&
                                      affSite.Affiliate.DepositLimitExceeded == false &&
                                      affSite.Affiliate.MinDepositAmount <= request.Amount &&
                                      affSite.Affiliate.MaxDepositAmount >= request.Amount)
                    .Select(affSite => new AffiliateSite()
                    {
                        // AccountSite properties
                        Affiliate = new Affiliate()
                        {
                            Id = affSite.Affiliate.Id,
                            Name = affSite.Affiliate.Name,
                            IsDynamic = affSite.Affiliate.IsDynamic,
                            DailyDepositLimit = affSite.Affiliate.DailyDepositLimit,
                            DailyWithdrawLimit = affSite.Affiliate.DailyWithdrawLimit,
                            MinDepositAmount = affSite.Affiliate.MinDepositAmount,
                            MaxDepositAmount = affSite.Affiliate.MaxDepositAmount,
                            MinWithdrawAmount = affSite.Affiliate.MinWithdrawAmount,
                            MaxWithdrawAmount = affSite.Affiliate.MaxWithdrawAmount,
                            IsDepositActive = affSite.Affiliate.IsDepositActive,
                            IsWithdrawActive = affSite.Affiliate.IsWithdrawActive,
                            DepositLimitExceeded = affSite.Affiliate.DepositLimitExceeded,
                            WithdrawLimitExceeded = affSite.Affiliate.WithdrawLimitExceeded,
                            CommissionRate = affSite.Affiliate.CommissionRate,
                            Accounts = affSite.Affiliate.Accounts
                                .Where(accSite => accSite.IsActive &&
                                                  accSite.MinDepositAmount <= request.Amount &&
                                                  accSite.MaxDepositAmount >= request.Amount &&
                                                  (accSite.AccountType == AccountType.UstYatirim ||
                                                   accSite.AccountType == AccountType.Yatirim) &&
                                                  accSite.PaymentWayId == request.PaymentWayType)
                                .Select(accSite => new Account
                                {
                                    // Account properties
                                    Id = accSite.Id,
                                    AffiliateId = accSite.AffiliateId,
                                    IsActive = accSite.IsActive,
                                    MinDepositAmount = accSite.MinDepositAmount,
                                    MaxDepositAmount = accSite.MaxDepositAmount,
                                    AccountType = accSite.AccountType,
                                    PaymentWayId = accSite.PaymentWayId,
                                    Affiliate = accSite.Affiliate != null ? new Affiliate
                                    {
                                        Id = accSite.Affiliate.Id,
                                        Name = accSite.Affiliate.Name,
                                        IsDynamic = accSite.Affiliate.IsDynamic
                                    } : null
                                }).ToList()
                        }
                    }).ToList(),
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

        
        // Bayi kontrolü
        var affiliates = site.AffiliateSites.Select(i => i.Affiliate).ToList();
        
        var random = new Random();
        var shuffledAffiliates = affiliates.OrderBy(a => Random.Shared.Next()).ToList();
        Account? affiliateAccount = null;
        
        foreach (var affiliate in shuffledAffiliates)
        {
            var affiliateAccounts = affiliate.Accounts.Where(acc => acc.IsActive &&
                                                                    acc.MinDepositAmount <= request.Amount &&
                                                                    acc.MaxDepositAmount >= request.Amount &&
                                                                    (acc.AccountType == AccountType.UstYatirim ||
                                                                     acc.AccountType == AccountType.Yatirim) &&
                                                                    acc.PaymentWayId == request.PaymentWayType).ToList();
            if (affiliateAccounts.Any())
            {
                affiliateAccount = affiliateAccounts.ElementAt(random.Next(affiliateAccounts.Count()));
                break; // Döngüyü durdur
            }
                
        }
        
        
        // Uygun hesapların alınması
        var availableAccounts = site.AccountSites.Select(i => i.Account);
        Account? selectedAccount = null;
        // Eğer yatırım 100k altındaysa hesap otomatik seçilir
        if (request.Amount <= 100000)
        {
            selectedAccount =
                affiliateAccount ?? // Bayi hesabı varsa onu alır
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
            AffiliateId = selectedAccount?.AffiliateId
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