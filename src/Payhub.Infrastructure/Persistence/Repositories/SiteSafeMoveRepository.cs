using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.SafeManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class SiteSafeMoveRepository : RepositoryBase<SiteSafeMove, ApplicationDbContext>, ISiteSafeMoveRepository
{
    public SiteSafeMoveRepository(ApplicationDbContext context) : base(context)
    {
    }
}