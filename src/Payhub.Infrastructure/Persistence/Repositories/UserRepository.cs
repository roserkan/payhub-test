using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.UserManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class UserRepository : RepositoryBase<User, ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}