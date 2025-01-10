using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.TransactionManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class DepositRepository : RepositoryBase<Deposit, ApplicationDbContext>, IDepositRepository
{
    public DepositRepository(ApplicationDbContext context) : base(context)
    {
    }
}