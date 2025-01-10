using Shared.Domain;

namespace Payhub.Domain.Entities.PaymentWayManagement;

public class PaymentWay : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}