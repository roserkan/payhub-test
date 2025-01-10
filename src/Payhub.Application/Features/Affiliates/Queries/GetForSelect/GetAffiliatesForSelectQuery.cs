using Shared.Abstractions.Messaging;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Affiliates.Queries.GetForSelect;

public sealed record GetAffiliatesForSelectQuery : IQuery<IEnumerable<SelectDto>>
{
}