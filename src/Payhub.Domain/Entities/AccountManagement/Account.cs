using Payhub.Domain.Entities.AffiliateManagement;
using Payhub.Domain.Entities.PaymentWayManagement;
using Payhub.Domain.Entities.TransactionManagement;
using Payhub.Domain.Enums;
using Shared.Domain;

namespace Payhub.Domain.Entities.AccountManagement;

public class Account : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
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
    
    // Navigation properties
    public ICollection<AccountSite> AccountSites { get; set; } = new List<AccountSite>();
    public Bank Bank { get; set; } = null!;
    public PaymentWay PaymentWay { get; set; } = null!;
    public Affiliate? Affiliate { get; set; }
    public ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();
    public ICollection<Withdraw> Withdraws { get; set; } = new List<Withdraw>();
}