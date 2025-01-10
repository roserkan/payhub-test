using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.BlacklistIbans.Commands.Delete;

public sealed record DeleteBlacklistIbanCommand : ICommand<int>
{
    public int Id { get; set; }
}