using Payhub.Application.Common.DTOs.Deposits;
using Payhub.Application.Common.Pipelines.Logging;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Deposits.Commands.Create;

public sealed record CreateDepositCommand : ICommand<CreatedDepositDto>, ILogRequest

{
    public string CustomerId { get; set; } = null!;
    public string CustomerFullName { get; set; } = null!;
    public string CustomerUserName { get; set; } = null!;
    public string? CustomerSignupDate { get; set; }
    public string? CustomerIpAddress { get; set; }
    public string? CustomerIdentityNumber { get; set; }
    public decimal Amount { get; set; }
    public string ProcessId { get; set; } = null!;
    public string? RedirectUrl { get; set; }
    public int PaymentWayType { get; set; } // Burası paymentWayId
}