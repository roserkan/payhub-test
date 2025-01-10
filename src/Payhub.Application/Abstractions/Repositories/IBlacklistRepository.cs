using Payhub.Domain.Entities.CustomerManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface IBlacklistRepository : IReadRepository<Blacklist>, IWriteRepository<Blacklist>{}