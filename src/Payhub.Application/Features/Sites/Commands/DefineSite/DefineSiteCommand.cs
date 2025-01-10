using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Sites.Commands.DefineSite;

public sealed record DefineSiteCommand : ICommand<int>
{
    public int AccountId { get; set; }
    public List<int> SiteIds { get; set; } = new();
}