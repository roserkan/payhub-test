using Payhub.Application.Abstractions.Repositories;
using Payhub.Domain.Entities.PaymentWayManagement;
using Payhub.Infrastructure.Persistence.Contexts;
using Shared.Persistence.EfCore;

namespace Payhub.Infrastructure.Persistence.Repositories;

public class PaymentWayRepository : RepositoryBase<PaymentWay, ApplicationDbContext>, IPaymentWayRepository
{
    public PaymentWayRepository(ApplicationDbContext context) : base(context)
    {
    }
}