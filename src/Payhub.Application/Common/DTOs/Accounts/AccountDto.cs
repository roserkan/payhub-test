using Payhub.Application.Common.DTOs.Affiliates;
using Payhub.Application.Common.DTOs.PaymentWays;
using Payhub.Domain.Enums;
using Shared.Utils.Responses;

namespace Payhub.Application.Common.DTOs.Accounts;

public sealed record AccountDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public decimal FirstBalance { get; set; }

    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? EmailPassword { get; set; }
    public string? EmailImapPassword { get; set; }
    
    public decimal MinDepositAmount { get; set; }
    public decimal MaxDepositAmount { get; set; }
    public decimal DailyDepositAmountLimit { get; set; }
    public decimal DailyWithdrawAmountLimit { get; set; }

    public int PaymentWayId { get; set; }
    public int? AffiliateId { get; set; }
    public int BankId { get; set; }
    public AccountType AccountType { get; set; } 
    public AccountClassification AccountClassification { get; set; }

    public int HealthyPercent { get; set; } = 0;
    public bool IsDeleted { get; set; }
    
    public BankDto Bank { get; set; } = new();
    public PaymentWayDto PaymentWay { get; set; } = new();
    public AffiliateDto? Affiliate { get; set; } = null;
    public IEnumerable<SelectDto> Sites { get; set; } = new List<SelectDto>();
}