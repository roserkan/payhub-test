using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.CustomerManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class CustomerRepository : RepositoryBase<Customer, ApplicationDbContext>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }
}