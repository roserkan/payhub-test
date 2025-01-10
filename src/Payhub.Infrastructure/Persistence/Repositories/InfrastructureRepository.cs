using Payhub.Application.Abstractions.Repositories;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class InfrastructureRepository : RepositoryBase<Domain.Entities.SiteManagement.Infrastructure, ApplicationDbContext>, IInfrastructureRepository
{
    public InfrastructureRepository(ApplicationDbContext context) : base(context)
    {
    }
}