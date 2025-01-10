using Payhub.Domain.Entities.UserManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface ISystemPermissionRepository : IReadRepository<SystemPermission>, IWriteRepository<SystemPermission>
{
}