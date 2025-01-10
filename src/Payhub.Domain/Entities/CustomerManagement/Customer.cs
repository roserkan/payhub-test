using Payhub.Domain.Entities.SiteManagement;
using Shared.Domain;

namespace Payhub.Domain.Entities.CustomerManagement;

public class Customer : BaseEntity
{
    public int SiteId { get; set; }
    public string? SiteCustomerId { get; set; } // siteden gelen customer'ın ID si
    public string? FullName { get; set; }
    public string? Username { get; set; }
    public string? SignupDate { get; set; }
    public string? IdentityNumber { get; set; }
    public string? CustomerIpAddress { get; set; }
    public string PanelCustomerId { get; set; } = string.Empty; // siteCustomerId + siteName
    
    // Navigation Properties
    public Site Site { get; set; } = null!;
}