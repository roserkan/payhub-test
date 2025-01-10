using Payhub.Domain.Entities.AccountManagement;
using Payhub.Domain.Entities.BotManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface IHavaleBotMoveRepository : IReadRepository<HavaleBotMove>, IWriteRepository<HavaleBotMove>{}