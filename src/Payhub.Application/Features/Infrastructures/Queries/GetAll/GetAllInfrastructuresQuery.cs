using Payhub.Application.Common.DTOs.Infrastructures;

using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Infrastructures.Queries.GetAll;

public sealed record GetAllInfrastructuresQuery : IQuery<IEnumerable<InfrastructureDto>>
{
}