using Payhub.Domain.Entities.BotManagement;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.WithdrawOrders.Commands.Update;

public sealed record UpdateWithdrawOrderCommand(int Id, 
    int AccountId,
    WithdrawOrderStatus? Status,
    string ReceiverAccountNumber, string ReceiverFullName, decimal Amount,
    string? Description) : ICommand<int>;