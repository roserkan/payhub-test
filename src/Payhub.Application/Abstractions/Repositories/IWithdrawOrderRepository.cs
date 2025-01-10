using Payhub.Domain.Entities.BotManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface IWithdrawOrderRepository : IReadRepository<WithdrawOrder>, IWriteRepository<WithdrawOrder>{}