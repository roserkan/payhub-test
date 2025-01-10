using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Blacklists.Commands.Create;

public sealed record CreateBlacklistCommand : ICommand<int>
{
    public BlacklistType BlacklistType { get; set; }
    public string Value { get; set; } = null!;
}