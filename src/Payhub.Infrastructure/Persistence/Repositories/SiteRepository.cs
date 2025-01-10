using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.SiteManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class SiteRepository : RepositoryBase<Site, ApplicationDbContext>, ISiteRepository
{
    public SiteRepository(ApplicationDbContext context) : base(context)
    {
    }
}