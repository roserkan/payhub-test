using Payhub.Application.Features.HavaleBots.BotResults;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.HavaleBotMoves.Commands;

public sealed class CreateHavaleBotMoveCommand : ICommand<BotResult<int>>
{
    public string SenderName { get; set; } = null!;
    public decimal Amount { get; set; }
    public int ReceiverAccId { get; set; }
    public DateTime TransferReceivedDate { get; set; }
    public string SecurityKey { get; set; } = null!;
}