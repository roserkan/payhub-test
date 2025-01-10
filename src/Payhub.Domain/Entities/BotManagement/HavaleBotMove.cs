using Payhub.Domain.Enums;
using Shared.Domain;

namespace Payhub.Domain.Entities.BotManagement;

public class HavaleBotMove : BaseEntity
{
    public string SenderName { get; set; } = null!;
    public decimal Amount { get; set; }
    public int ReceiverAccId { get; set; }
    public DateTime TransferReceivedDate { get; set; }
    public string SecurityKey { get; set; } = null!;
    public HavaleBotMoveStatus Status { get; set; }
    public int TryCount { get; set; }
}