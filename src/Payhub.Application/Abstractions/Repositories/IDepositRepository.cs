using Payhub.Domain.Entities.TransactionManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface IDepositRepository : IReadRepository<Deposit>, IWriteRepository<Deposit> {}