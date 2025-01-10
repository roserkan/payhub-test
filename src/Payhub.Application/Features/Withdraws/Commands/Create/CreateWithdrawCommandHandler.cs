using System.Diagnostics;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.Constants;
using Payhub.Application.Common.DTOs.Consumers;
using Payhub.Application.Common.DTOs.Withdraws;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.RequestModels;
using Payhub.Application.Features.Withdraws.EventConsumers;
using Payhub.Application.Features.Withdraws.Events;
using Payhub.Domain.Entities.AccountManagement;
using Payhub.Domain.Entities.AffiliateManagement;
using Payhub.Domain.Entities.CustomerManagement;
using Payhub.Domain.Entities.SiteManagement;
using Payhub.Domain.Entities.TransactionManagement;
using Payhub.Domain.Enums;
using Shared.Abstractions.Hubs;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Shared.Utils.Helpers;
using Shared.Utils.Security.Hashing;

namespace Payhub.Application.Features.Withdraws.Commands.Create;

public sealed class CreateWithdrawCommandHandler : ICommandHandler<CreateWithdrawCommand, CreatedWithdrawDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IPayoxService _payoxService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<CreatedWithdrawEventConsumer> _logger;
    
    public CreateWithdrawCommandHandler(IUnitOfWork unitOfWork, 
        IHttpContextAccessor httpContextAccessor,
        ISendEndpointProvider sendEndpointProvider,
        IPayoxService payoxService,
        IHubContext<NotificationHub> hubContext,
        ILogger<CreatedWithdrawEventConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _sendEndpointProvider = sendEndpointProvider;
        _payoxService = payoxService;
        _hubContext = hubContext;
        _logger = logger;
    }

    
    public async Task<CreatedWithdrawDto> Handle(CreateWithdrawCommand request, CancellationToken cancellationToken)
    {
        var createdWithdrawEvent = await CheckWithdraw(request, cancellationToken);
     
        // var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:withdraw-event-queue"));
        // await endpoint.Send(createdWithdrawEvent); 
        
        await AddWithdrawAndCustomer(createdWithdrawEvent);
        
        var result = new CreatedWithdrawDto
        {
            Message = "Çekim Talebiniz Alındı",
            ProcessId = request.ProcessId,
            Success = true
        };
        
        return result;
    }

    private async Task<CreatedWithdrawEvent> CheckWithdraw(CreateWithdrawCommand request, CancellationToken cancellationToken)
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
                        IsActive = spw.IsActive
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
                                                  (accSite.AccountType == AccountType.Cekim) &&
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
                                      accSite.Account.AccountType == AccountType.Cekim &&
                                      accSite.Account.PaymentWayId == request.PaymentWayType)
                    .Select(accSite => new AccountSite
                    {
                        // AccountSite özelliklerini atayabilirsiniz.
                        Account = new Account
                        {
                            Id = accSite.Account.Id,
                            AffiliateId = accSite.Account.AffiliateId,
                            IsActive = accSite.Account.IsActive,
                            AccountType = accSite.Account.AccountType,
                            PaymentWayId = accSite.Account.PaymentWayId,
                            Affiliate = accSite.Account.Affiliate
                        }
                    })
                    .ToList()
            }).FirstOrDefaultAsync(i => i.SitePaymentWays.Any(i => i.ApiKey == apiKey), cancellationToken);

        if (site == null)
            throw new NotFoundException(ErrorMessages.Withdraws_SiteNotFound);
        
        // Sitenin bu yatırım yöntemini desteklemesi kontrolü
        var spw = site.SitePaymentWays.FirstOrDefault(i => i.IsActive);
        if (spw == null)
            throw new BusinessException(ErrorMessages.Deposits_PaymentWayInactive);
        
        // Güvenlik Anahtarı Kontrolü
        var hashed = HashingHelper.Sha256Hash(apiKey + spw.SecretKey + request.ProcessId);
        if (hashed != securityKey)
            throw new BusinessException(ErrorMessages.Deposits_InvalidSecurityKey);
        
        
        // Bayi ayarları
        var affiliates = site.AffiliateSites.Select(i => i.Affiliate).ToList();

        var random = new Random();
        var shuffledAffiliates = affiliates.OrderBy(a => Random.Shared.Next()).ToList();
        Account? affiliateAccount = null;
        foreach (var affiliate in shuffledAffiliates)
        {
            var affiliateAccounts = affiliate.Accounts.Where(acc => acc.IsActive &&
                                                                    (acc.AccountType == AccountType.Cekim) &&
                                                                    acc.PaymentWayId == request.PaymentWayType).ToList();
            if (affiliateAccounts.Any())
            {
                affiliateAccount = affiliateAccounts.ElementAt(random.Next(affiliateAccounts.Count()));
                break; // Döngüyü durdur
            }
        }
        
        var affiliateAcc = site.AccountSites
            .Select(i => i.Account)
            .FirstOrDefault(acc => acc.IsActive
                                   && acc.AccountType == AccountType.Cekim
                                   && acc.AffiliateId != null);
        
        

        if (affiliateAccount != null && affiliateAccount.AffiliateId != null && affiliateAccount.Affiliate!.IsDynamic)
        {
            // Burada eğer çekim bayiye yönlenmiş ve bayi hesapları dinamik ise çekimi bayiye gönderiyoruz
            await PayoxAffiliateWithdraw(request.ProcessId, request.PaymentWayType, request.Amount, request.CustomerId, request.CustomerFullName, request.CustomerUserName, affiliateAcc.Name, affiliateAcc.AccountNumber);
        }
        
        return new CreatedWithdrawEvent
        {
            WithdrawRequest = request,
            SiteId = site.Id, 
            SiteName = site.Name,
            AccountId = null,
            PaymentId = Guid.NewGuid().ToString(),
            CreatedDate = DateTime.Now,
            AffiliateId = affiliateAccount?.AffiliateId
        };
    }
    
    private async Task PayoxAffiliateWithdraw(string processId, int paymentWayId, decimal amount, string? customerId, string? customerFullName, string customerUserName, string bankAccountName, string accountNumber)
    {
        var availableBanks = await _payoxService.GetAvailableBanksAsync(paymentWayId);
        var bank = availableBanks.Data.Count < 2
            ? availableBanks.Data.FirstOrDefault()
            : availableBanks.Data.FirstOrDefault(b => b.Id.Length > 4);

        if (bank == null)
            throw new BusinessException("Uygun banka bulunamadı.");

        var request = new PayoxCreateWithdrawRequest
        {
            BankId = bank.Id,
            ProcessId = processId,
            Amount = amount,
            Name = customerFullName ?? string.Empty,
            UserName = customerUserName ?? string.Empty,
            UserId = customerId ?? string.Empty,
            AccountName = bankAccountName,
            Iban = accountNumber,
        };

        await _payoxService.CreateWithdrawAsync(request, paymentWayId);
    }
    private async Task AddWithdrawAndCustomer(CreatedWithdrawEvent context)
    {
        var  stopwatch = new Stopwatch();
        stopwatch.Start();
        
        var log = new ConsumerLog
        {
            ConsumeType = "CreatedWithdrawEventConsumer",
            Message = "CreatedWithdrawEventConsumer started",
            ElapsedTime = 0,
            ProcessId = context.WithdrawRequest.ProcessId,
            ErrorMessage = null
        };
        
        try
        {
            var data = context;
            var request = data.WithdrawRequest;
            var panelCustomerId = request.CustomerId + data.SiteName;
            var customer =
                await _unitOfWork.CustomerRepository.GetAsync(i =>
                    i.PanelCustomerId == request.CustomerId + data.SiteName);
            if (customer == null)
            {
                customer = new Customer
                {
                    SiteId = data.SiteId,
                    SiteCustomerId = request.CustomerId,
                    FullName = request.CustomerFullName,
                    Username = request.CustomerUserName,
                    SignupDate = request.CustomerSignupDate,
                    IdentityNumber = request.CustomerIdentityNumber,
                    CustomerIpAddress = request.CustomerIpAddress,
                    PanelCustomerId = panelCustomerId
                };

                await _unitOfWork.CustomerRepository.AddAsync(customer);
            }
            else
            {
                customer.SiteCustomerId = request.CustomerId;
                customer.FullName = request.CustomerFullName;
                customer.Username = request.CustomerUserName;
                customer.SignupDate = request.CustomerSignupDate;
                customer.IdentityNumber = request.CustomerIdentityNumber;
                customer.CustomerIpAddress = request.CustomerIdentityNumber;
                customer.PanelCustomerId = panelCustomerId;

                await _unitOfWork.CustomerRepository.UpdateAsync(customer);
            }

            log.Message += " -- Customer created or updated";

            var isIbanBlacklisted = await _unitOfWork.BlacklistIbanRepository
                .AnyAsync(i => i.Iban == request.AccountNumber.Trim().Replace(" ", ""));
            var withdraw = new Withdraw
            {
                PanelCustomerId = customer.PanelCustomerId,
                SiteCustomerId = customer.SiteCustomerId,
                CustomerFullName = request.CustomerFullName,
                SiteId = data.SiteId,
                ProcessId = request.ProcessId,
                Amount = request.Amount,
                PayedAmount = request.Amount,
                PaymentWayId = request.PaymentWayType,
                Status = WithdrawStatus.PendingWithdraw,
                InfraConfirmed = false,
                AccountId = data.AccountId,
                CustomerAccountNumber = request.AccountNumber,
                IsIbanBlacklisted = isIbanBlacklisted,
                AffiliateId = data.AffiliateId
            };

            log.Message += " -- Deposit created";

            await _unitOfWork.WithdrawRepository.AddAsync(withdraw);
            await _unitOfWork.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "withdraw");
        }
        catch (Exception e)
        {
            log.ErrorMessage = e.Message + "-- BİR SORIUN OLUŞTU";
            throw new Exception( e.Message + "-- BİR SORIUN OLUŞTU");
        }
        finally
        {
            stopwatch.Stop();
            log.ElapsedTime = stopwatch.ElapsedMilliseconds;
            if (log.ErrorMessage == null)
                _logger.LogInformation("{@ConsumerLog}", log);
            else
                _logger.LogError("{@ConsumerLog}", log);
        }
    }
}