using Payhub.Domain.Entities.AccountManagement;
using Payhub.Domain.Enums;
using Shared.Domain;

namespace Payhub.Domain.Entities.BotManagement;

public class WithdrawOrder : BaseEntity
{
    public int AccountId { get; set; }
    public string ReceiverAccountNumber { get; set; } = null!;
    public string ReceiverFullName { get; set; } = null!;
    public decimal Amount { get; set; }
    public WithdrawOrderStatus Status { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; }
    
    // Navigation properties
    public Account Account { get; set; } = null!;
}