using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Accounts.Commands.Delete;

public sealed record DeleteAccountCommand : ICommand<int>
{
    public int Id { get; set; }
}