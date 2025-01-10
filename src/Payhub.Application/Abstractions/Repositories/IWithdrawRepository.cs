using Payhub.Domain.Entities.TransactionManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface IWithdrawRepository : IReadRepository<Withdraw>, IWriteRepository<Withdraw>{}