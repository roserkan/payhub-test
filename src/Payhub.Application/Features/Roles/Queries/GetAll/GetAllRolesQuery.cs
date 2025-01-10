using Payhub.Application.Common.DTOs.Roles;

using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Roles.Queries.GetAll;

public sealed record GetAllRolesQuery : IQuery<IEnumerable<RoleDto>>
{
}