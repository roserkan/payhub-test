using Payhub.Domain.Entities.SafeManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface ISiteSafeMoveRepository: IReadRepository<SiteSafeMove>, IWriteRepository<SiteSafeMove>
{
}