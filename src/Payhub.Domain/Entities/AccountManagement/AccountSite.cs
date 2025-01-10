using Payhub.Domain.Entities.SiteManagement;
using Shared.Domain;

namespace Payhub.Domain.Entities.AccountManagement;

public class AccountSite : BaseEntity
{
    public int AccountId { get; set; }
    public int SiteId { get; set; }
    
    // Navigation properties
    public Account Account { get; set; } = null!;
    public Site Site { get; set; } = null!;
}