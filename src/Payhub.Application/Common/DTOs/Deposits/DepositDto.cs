using Payhub.Domain.Enums;

namespace Payhub.Application.Common.DTOs.Deposits;

public sealed record DepositDto
{
    public int Id { get; set; }
    public string ProcessId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PayedAmount { get; set; }
    public DepositStatus Status { get; set; }
    public bool InfraConfirmed { get; set; }
    public string? DynamicAccountName { get; set; }
    public string? DynamicAccountNumber { get; set; }
    public string? CustomerRequestName { get; set; }
    
    // include other properties
    public string SiteName { get; set; } = string.Empty;
    public int PaymentWayId { get; set; }
    public int SiteId { get; set; }
    public string? CustomerFullName { get; set; } = string.Empty;
    public string? CustomerUserName { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string? SiteCustomerId { get; set; }
    public string PanelCustomerId { get; set; } = string.Empty; 
    public string? BankAccountName { get; set; }
    public string? AccountNumber { get; set; }
    public int? AccountId { get; set; }
    public string? BankIconUrl { get; set; }
    public string? BankName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? ProcessOwnerName { get; set; } 
    public string? LastProcessOwnerName { get; set; }
    public string? AutoUpdatedName { get; set; }
    public InfraCallbackType? InfraCallbackType { get; set; }
    public string? AffiliateName { get; set; }
    
}