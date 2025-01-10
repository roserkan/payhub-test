using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.CustomerManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class BlacklistRepository : RepositoryBase<Blacklist, ApplicationDbContext>, IBlacklistRepository
{
    public BlacklistRepository(ApplicationDbContext context) : base(context)
    {
    }
}