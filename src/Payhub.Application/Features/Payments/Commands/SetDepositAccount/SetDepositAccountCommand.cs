using Payhub.Domain.Entities.AccountManagement;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Payments.Commands.SetDepositAccount;

public sealed record SetDepositAccountCommand : IQuery<int>
{
    public int DepositId { get; set; }
    public int AccountId { get; set; }
}