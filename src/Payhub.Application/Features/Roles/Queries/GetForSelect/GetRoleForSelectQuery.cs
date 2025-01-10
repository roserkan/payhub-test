using Shared.Abstractions.Messaging;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Roles.Queries.GetForSelect;

public sealed record GetRoleForSelectQuery : IQuery<IEnumerable<SelectDto>>
{
}