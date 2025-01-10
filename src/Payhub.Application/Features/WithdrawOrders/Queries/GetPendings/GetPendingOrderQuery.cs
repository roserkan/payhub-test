using Payhub.Application.Common.DTOs.WithdrawOrders;
using Payhub.Application.Features.HavaleBots.BotResults;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.WithdrawOrders.Queries.GetPendings;

public sealed record GetPendingOrderQuery(int AccId) : IQuery<BotResult<PendingWithdrawOrderDto>>;