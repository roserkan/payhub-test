using Payhub.Domain.Enums;

namespace Payhub.Application.Common.DTOs.Deposits;

public sealed record DepositDetailForAccountDto
{
    public int Id { get; set; }
    public string ProcessId { get; set; } = null!;
    public decimal Amount { get; set; }
    public decimal PayedAmount { get; set; }
    public DepositStatus Status { get; set; }
    public bool InfraConfirmed { get; set; }
    public string? AccountName { get; set; }
    public string? Iban { get; set; }
}