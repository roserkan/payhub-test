using Payhub.Domain.Entities.AccountManagement;
using Payhub.Domain.Entities.AffiliateManagement;
using Payhub.Domain.Entities.SafeManagement;
using Payhub.Domain.Entities.TransactionManagement;
using Shared.Domain;

namespace Payhub.Domain.Entities.SiteManagement;

public class Site : BaseEntity
{
    public int InfrastructureId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    
    // Navigation properties
    public Infrastructure Infrastructure { get; set; } = null!;
    public ICollection<SitePaymentWay> SitePaymentWays { get; set; } = new List<SitePaymentWay>();
    public ICollection<AccountSite> AccountSites { get; set; } = new List<AccountSite>();
    public ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();
    public ICollection<Withdraw> Withdraws { get; set; } = new List<Withdraw>();
    public ICollection<SiteSafeMove> SiteSafeMoves { get; set; } = new List<SiteSafeMove>();
    public ICollection<AffiliateSite> AffiliateSites { get; set; } = new List<AffiliateSite>();
}