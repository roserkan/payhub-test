using Payhub.Domain.Entities.SiteManagement;
using Payhub.Domain.Entities.UserManagement;
using Payhub.Domain.Enums;
using Shared.Domain;

namespace Payhub.Domain.Entities.SafeManagement;

public class SiteSafeMove : BaseEntity
{
    public int SiteId { get; set; }
    public SiteSafeMoveTransactionType TransactionType { get; set; }
    public SiteSafeMoveType MoveType { get; set; }
    
    public decimal Amount { get; set; }
    public decimal CommissionRate { get; set; }
    public decimal CommissionAmount { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; }

  
    public int? CreatedUserId { get; set; }
    public int? UpdatedUserId { get; set; }
    
    // Navigation Properties
    public Site Site { get; set; } = null!;
    public User CreatedUser { get; set; } = null!;
    public User UpdatedUser { get; set; } = null!;
}