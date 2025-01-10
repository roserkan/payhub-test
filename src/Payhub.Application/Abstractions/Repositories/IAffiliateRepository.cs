using Payhub.Domain.Entities;
using Payhub.Domain.Entities.AffiliateManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface IAffiliateRepository : IReadRepository<Affiliate>, IWriteRepository<Affiliate>
{
}