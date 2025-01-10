using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.WithdrawOrders.Commands.Create;

public sealed record CreateWithdrawOrderCommand(int AccountId, string ReceiverAccountNumber, string ReceiverFullName, decimal Amount, string? Description) : ICommand<int>;