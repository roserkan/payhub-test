using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.AccountManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class BlacklistIbanRepository : RepositoryBase<BlacklistIban, ApplicationDbContext>, IBlacklistIbanRepository
{
    public BlacklistIbanRepository(ApplicationDbContext context) : base(context)
    {
    }
}