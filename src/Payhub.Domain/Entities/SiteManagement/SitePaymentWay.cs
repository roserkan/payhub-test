using Shared.Domain;

namespace Payhub.Domain.Entities.SiteManagement;

public class SitePaymentWay : BaseEntity
{
    public int SiteId { get; set; }
    public int PaymentWayId { get; set; }
    public string ApiKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public decimal Commission { get; set; }
    public decimal MinBalanceLimit { get; set; }
    public decimal MaxBalanceLimit { get; set; }
    public bool IsActive { get; set; }
}