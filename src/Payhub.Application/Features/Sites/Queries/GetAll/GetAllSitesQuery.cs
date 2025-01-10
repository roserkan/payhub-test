using Payhub.Application.Common.DTOs.Sites;

using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Sites.Queries.GetAll;

public sealed record GetAllSitesQuery : IQuery<IEnumerable<SiteDto>>
{
    public bool DisableSiteFilter { get; set; }
}