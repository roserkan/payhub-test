namespace Payhub.Application.Common.DTOs.Deposits;

public sealed record CreatedDepositDto
{
    public string PaymentLink { get; set; } = default!;
    public string ProcessId { get; set; } = default!;
    public bool Success { get; set; }
}