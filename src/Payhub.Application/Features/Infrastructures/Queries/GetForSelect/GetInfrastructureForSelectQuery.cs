using Shared.Abstractions.Messaging;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Infrastructures.Queries.GetForSelect;

public sealed record GetInfrastructureForSelectQuery : IQuery<IEnumerable<SelectDto>>
{
}