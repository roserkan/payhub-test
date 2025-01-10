using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.TransactionManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class WithdrawRepository : RepositoryBase<Withdraw, ApplicationDbContext>, IWithdrawRepository
{
    public WithdrawRepository(ApplicationDbContext context) : base(context)
    {
    }
}