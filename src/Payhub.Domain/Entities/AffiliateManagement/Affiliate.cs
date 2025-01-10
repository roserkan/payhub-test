using Payhub.Domain.Entities.AccountManagement;
using Payhub.Domain.Entities.SafeManagement;
using Payhub.Domain.Entities.TransactionManagement;
using Shared.Domain;

namespace Payhub.Domain.Entities.AffiliateManagement;

public class Affiliate : BaseEntity
{
    public string Name { get; set; } = null!;
    public bool IsDynamic { get; set; }
    public decimal DailyDepositLimit { get; set; }
    public decimal DailyWithdrawLimit { get; set; }
    public decimal MinDepositAmount { get; set; }
    public decimal MaxDepositAmount { get; set; }
    public decimal MinWithdrawAmount { get; set; }
    public decimal MaxWithdrawAmount { get; set; }
    public bool IsDepositActive { get; set; }
    public bool IsWithdrawActive { get; set; }
    public bool DepositLimitExceeded { get; set; }
    public bool WithdrawLimitExceeded { get; set; }
    public decimal CommissionRate { get; set; }
    
    // Navigation properties
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public ICollection<AffiliateSite> AffiliateSites { get; set; } = new List<AffiliateSite>();
    public ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();
    public ICollection<Withdraw> Withdraws { get; set; } = new List<Withdraw>();
    public ICollection<AffiliateSafeMove> AffiliateSafeMoves{ get; set; } = new List<AffiliateSafeMove>();

}