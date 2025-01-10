using System.Diagnostics;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Consumers;
using Payhub.Application.Features.Deposits.Events;
using Payhub.Domain.Entities.CustomerManagement;
using Payhub.Domain.Entities.TransactionManagement;
using Payhub.Domain.Enums;
using Shared.Abstractions.Hubs;

namespace Payhub.Application.Features.Deposits.EventConsumers;

public class CreatedDepositEventConsumer : IConsumer<CreatedDepositEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<CreatedDepositEventConsumer> _logger;
    
    public CreatedDepositEventConsumer(IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext, ILogger<CreatedDepositEventConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<CreatedDepositEvent> context)
    {
        var  stopwatch = new Stopwatch();
        stopwatch.Start();
        
        var log = new ConsumerLog
        {
            ConsumeType = "CreatedDepositEventConsumer",
            Message = "CreatedDepositEventConsumer started",
            ElapsedTime = 0,
            ProcessId = context.Message.DepositRequest.ProcessId,
            ErrorMessage = null
        };
        
        try
        {
            var data = context.Message;
            var request = data.DepositRequest;
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


            var deposit = new Deposit
            {
                PanelCustomerId = customer.PanelCustomerId,
                SiteCustomerId = customer.SiteCustomerId,
                SiteId = data.SiteId,
                ProcessId = request.ProcessId,
                CustomerFullName = request.CustomerFullName,
                Amount = request.Amount,
                PayedAmount = request.Amount,
                RedirectUrl = request.RedirectUrl,
                PaymentWayId = request.PaymentWayType,
                Status = DepositStatus.PendingDeposit,
                InfraConfirmed = false,
                AccountId = data.AccountId,
                PaymentId = data.PaymentId,
                Commission = data.Commission,
                DynamicAccountName = data.DynamicAccountName,
                DynamicAccountNumber = data.DynamicAccountNumber,
                AffiliateId = data.AffiliateId
            };

            log.Message += " -- Deposit created";
        
            await _unitOfWork.DepositRepository.AddAsync(deposit);
            await _unitOfWork.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "deposit");
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