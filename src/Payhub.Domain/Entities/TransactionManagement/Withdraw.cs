using Payhub.Domain.Entities.AccountManagement;
using Payhub.Domain.Entities.AffiliateManagement;
using Payhub.Domain.Entities.PaymentWayManagement;
using Payhub.Domain.Entities.SiteManagement;
using Payhub.Domain.Entities.UserManagement;
using Payhub.Domain.Enums;
using Shared.Domain;

namespace Payhub.Domain.Entities.TransactionManagement;

public class Withdraw : BaseEntity
{
    public int SiteId { get; set; }
    public string ProcessId { get; set; } = null!;
    public int? AccountId { get; set; }
    public string PanelCustomerId { get; set; } = null!; // siteCustomerId + siteName
    public string SiteCustomerId { get; set; } = null!;
    public string? CustomerFullName { get; set; }
    public decimal Amount { get; set; } 
    public decimal PayedAmount { get; set; }
    public int PaymentWayId { get; set; }
    public WithdrawStatus Status { get; set; }
    public bool InfraConfirmed { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string CustomerAccountNumber { get; set; } = null!;
    public int? CreatedUserId { get; set; }
    public int? UpdatedUserId { get; set; }
    public string? AutoUpdatedName { get; set; }
    public int? AffiliateId { get; set; }
    public InfraCallbackType? InfraCallbackType { get; set; }
    public bool IsIbanBlacklisted { get; set; }
    public decimal AffiliateCommission { get; set; }

     
    // Navigation properties
    public Site Site { get; set; } = null!;
    public Account? Account { get; set; }
    public PaymentWay PaymentWay { get; set; } = null!;
    public User? CreatedUser { get; set; }
    public User? UpdatedUser { get; set; }
    public Affiliate? Affiliate { get; set; }
}