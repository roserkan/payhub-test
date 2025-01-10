using System.Diagnostics;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Consumers;
using Payhub.Application.Features.Withdraws.Events;
using Payhub.Domain.Entities.CustomerManagement;
using Payhub.Domain.Entities.TransactionManagement;
using Payhub.Domain.Enums;
using Shared.Abstractions.Hubs;

namespace Payhub.Application.Features.Withdraws.EventConsumers;

public class CreatedWithdrawEventConsumer : IConsumer<CreatedWithdrawEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<CreatedWithdrawEventConsumer> _logger;
    
    public CreatedWithdrawEventConsumer(IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext, ILogger<CreatedWithdrawEventConsumer> logger)
    {
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreatedWithdrawEvent> context)
    {
        var  stopwatch = new Stopwatch();
        stopwatch.Start();
        
        var log = new ConsumerLog
        {
            ConsumeType = "CreatedWithdrawEventConsumer",
            Message = "CreatedWithdrawEventConsumer started",
            ElapsedTime = 0,
            ProcessId = context.Message.WithdrawRequest.ProcessId,
            ErrorMessage = null
        };
        
        try
        {
            var data = context.Message;
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