using Payhub.Domain.Entities.AccountManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface IBlacklistIbanRepository : IReadRepository<BlacklistIban>, IWriteRepository<BlacklistIban>{}