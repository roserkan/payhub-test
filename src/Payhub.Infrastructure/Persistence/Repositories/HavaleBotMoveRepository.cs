using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.BotManagement;
using Payhub.Domain.Entities.CustomerManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class HavaleBotMoveRepository : RepositoryBase<HavaleBotMove, ApplicationDbContext>, IHavaleBotMoveRepository
{
    public HavaleBotMoveRepository(ApplicationDbContext context) : base(context)
    {
    }
}