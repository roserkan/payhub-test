using Payhub.Domain.Entities.AffiliateManagement;
using Payhub.Domain.Entities.UserManagement;
using Payhub.Domain.Enums;
using Shared.Domain;

namespace Payhub.Domain.Entities.SafeManagement;

public class AffiliateSafeMove : BaseEntity
{
    public int AffiliateId { get; set; }
    public AffiliateSafeMoveTransactionType TransactionType { get; set; }
    public AffiliateSafeMoveType MoveType { get; set; }
    
    public decimal Amount { get; set; }
    public decimal CommissionRate { get; set; }
    public decimal CommissionAmount { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; }

  
    public int? CreatedUserId { get; set; }
    public int? UpdatedUserId { get; set; }
    
    // Navigation Properties
    public Affiliate Affiliate { get; set; } = null!;
    public User CreatedUser { get; set; } = null!;
    public User UpdatedUser { get; set; } = null!;
}