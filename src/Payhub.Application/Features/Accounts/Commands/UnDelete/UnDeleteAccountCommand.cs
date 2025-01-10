using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Accounts.Commands.UnDelete;

public sealed record UnDeleteAccountCommand : ICommand<int>
{
    public int Id { get; set; }
}