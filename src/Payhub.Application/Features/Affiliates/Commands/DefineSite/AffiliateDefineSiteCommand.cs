using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Affiliates.Commands.DefineSite;

public sealed record AffiliateDefineSiteCommand : ICommand<int>
{
    public int AffiliateId { get; set; }
    public List<int> SiteIds { get; set; } = new();
}