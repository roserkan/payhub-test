using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.AffiliateManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class AffiliateRepository : RepositoryBase<Affiliate, ApplicationDbContext>, IAffiliateRepository
{
    public AffiliateRepository(ApplicationDbContext context) : base(context)
    {
    }
}