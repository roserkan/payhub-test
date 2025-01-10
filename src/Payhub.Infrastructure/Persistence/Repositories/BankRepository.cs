using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.AccountManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class BankRepository : RepositoryBase<Bank, ApplicationDbContext>, IBankRepository
{
    public BankRepository(ApplicationDbContext context) : base(context)
    {
    }
}