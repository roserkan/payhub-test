using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.SafeManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class AffiliateSafeMoveRepository : RepositoryBase<AffiliateSafeMove, ApplicationDbContext>, IAffiliateSafeMoveRepository
{
    public AffiliateSafeMoveRepository(ApplicationDbContext context) : base(context)
    {
    }
}