using Payhub.Domain.Entities.AccountManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface IBankRepository : IReadRepository<Bank>, IWriteRepository<Bank>{}