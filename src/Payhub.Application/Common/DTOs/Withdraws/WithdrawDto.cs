using Payhub.Domain.Enums;

namespace Payhub.Application.Common.DTOs.Withdraws;

public sealed record WithdrawDto
{
    public int Id { get; set; }
    public string ProcessId { get; set; } = string.Empty;
    public string SiteName { get; set; } = string.Empty;
    public string? CustomerFullName { get; set; } = string.Empty;
    public string? CustomerSiteId { get; set; } = string.Empty;
    public string? CustomerPanelId { get; set; } = string.Empty;
    public string CustomerAccountNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PayedAmount { get; set; }
    public WithdrawStatus Status { get; set; }
    public bool InfraConfirmed { get; set; }
    public int? AccountId { get; set; }
    public string? BankIconUrl { get; set; }
    public string? BankName { get; set; }
    public string? AccountName { get; set; }
    public string? AccountNumber { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? ProcessOwnerName { get; set; } 
    public string? LastProcessOwnerName { get; set; }
    public string? AutoUpdatedName { get; set; }
    public int PaymentWayId { get; set; }
    public InfraCallbackType? InfraCallbackType { get; set; }
    public bool IsIbanBlacklisted { get; set; }
    public string? AffiliateName { get; set; }
}