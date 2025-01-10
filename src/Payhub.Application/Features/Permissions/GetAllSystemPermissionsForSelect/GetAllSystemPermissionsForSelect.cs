using Payhub.Domain.Entities.UserManagement;
using Shared.Abstractions.Messaging;
using Shared.Utils.Responses;
namespace Payhub.Application.Features.Permissions.GetAllSystemPermissionsForSelect;

public sealed record GetAllSystemPermissionsForSelect : IQuery<IEnumerable<SystemPermission>>
{
}