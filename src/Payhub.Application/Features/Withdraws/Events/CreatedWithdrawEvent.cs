using Payhub.Application.Features.Withdraws.Commands.Create;

namespace Payhub.Application.Features.Withdraws.Events;

public class CreatedWithdrawEvent
{
    public CreateWithdrawCommand WithdrawRequest { get; set; } = null!;
    public int SiteId { get; set; }
    public string SiteName { get; set; } = null!;
    public int? AccountId { get; set; }
    public string PaymentId { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public int? AffiliateId { get; set; }
}