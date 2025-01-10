using Payhub.Domain.Entities.SafeManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface IAffiliateSafeMoveRepository: IReadRepository<AffiliateSafeMove>, IWriteRepository<AffiliateSafeMove>
{
}