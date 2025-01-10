using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.UserManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class SystemPermissionRepository : RepositoryBase<SystemPermission, ApplicationDbContext>, ISystemPermissionRepository
{
    public SystemPermissionRepository(ApplicationDbContext context) : base(context)
    {
    }
}