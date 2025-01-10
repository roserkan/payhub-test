using Payhub.Application.Features.HavaleBots.BotResults;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.WithdrawOrders.Commands.SetOrderStatus;

public sealed class SetOrderStatusQuery : ICommand<BotResult<int>>
{
    public WithdrawOrderStatus? Status { get; set; }
    public int BankRobotTransferOrderId { get; set; }
} 
