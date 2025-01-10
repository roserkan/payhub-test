using Payhub.Domain.Entities.SiteManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface IInfrastructureRepository : IReadRepository<Infrastructure>, IWriteRepository<Infrastructure>{}