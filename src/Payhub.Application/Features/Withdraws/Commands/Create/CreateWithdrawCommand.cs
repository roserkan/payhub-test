using Payhub.Application.Common.DTOs.Withdraws;
using Payhub.Application.Common.Pipelines.Logging;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Withdraws.Commands.Create;

public sealed record CreateWithdrawCommand : ICommand<CreatedWithdrawDto>, ILogRequest
{
    public string CustomerId { get; set; } = null!;
    public string CustomerFullName { get; set; } = null!;
    public string CustomerUserName { get; set; } = null!;
    public string? CustomerSignupDate { get; set; }
    public string? CustomerIpAddress { get; set; }  
    public string? CustomerIdentityNumber { get; set; }
    public string AccountNumber { get; set; } = null!;
    public decimal Amount { get; set; } 
    public string ProcessId { get; set; } = null!;
    public int PaymentWayType { get; set; } // Burası paymentWayId
}