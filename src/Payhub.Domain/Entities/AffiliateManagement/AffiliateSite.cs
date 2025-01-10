using Payhub.Domain.Entities.SiteManagement;
using Shared.Domain;

namespace Payhub.Domain.Entities.AffiliateManagement;

public class AffiliateSite : BaseEntity
{
    public int AffiliateId { get; set; }
    public int SiteId { get; set; }
    
    // Navigation properties
    public Affiliate Affiliate { get; set; } = null!;
    public Site Site { get; set; } = null!;
}