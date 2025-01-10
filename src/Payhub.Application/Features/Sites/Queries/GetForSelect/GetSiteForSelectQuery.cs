using Shared.Abstractions.Messaging;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Sites.Queries.GetForSelect;

public sealed record GetSiteForSelectQuery : IQuery<IEnumerable<SelectDto>>
{
    public bool DisableSiteFilter { get; set; }
}