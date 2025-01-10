using Payhub.Application.Features.Deposits.Commands.Create;

namespace Payhub.Application.Features.Deposits.Events;

public class CreatedDepositEvent
{
    public CreateDepositCommand DepositRequest { get; set; } = null!;
    public int SiteId { get; set; }
    public string SiteName { get; set; } = null!;
    public int? AccountId { get; set; }
    public string PaymentId { get; set; } = null!;
    public decimal Commission { get; set; }
    public string? DynamicAccountName { get; set; }
    public string? DynamicAccountNumber { get; set; }
    public int? AffiliateId { get; set; }
    public DateTime CreatedDate { get; set; }

    public string? AccountName { get; set; } // Create For Account Command
    public string? AccountNumber { get; set; } // Creare For Account Command
}