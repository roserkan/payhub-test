using Payhub.Application.Common.DTOs.Affiliates;

using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Affiliates.Queries.GetAll;

public sealed record GetAllAffiliatesQuery : IQuery<IEnumerable<AffiliateDto>>
{
}