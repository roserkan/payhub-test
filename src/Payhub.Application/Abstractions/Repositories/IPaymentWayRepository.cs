using Payhub.Domain.Entities.PaymentWayManagement;
using Shared.Persistence.Abstraction;

namespace Payhub.Application.Abstractions.Repositories;

public interface IPaymentWayRepository : IReadRepository<PaymentWay>, IWriteRepository<PaymentWay>
{
}