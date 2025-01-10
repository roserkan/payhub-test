using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.WithdrawOrders.Commands.Delete;

public sealed record DeleteWithdrawOrderCommand(int Id) : ICommand<int>;