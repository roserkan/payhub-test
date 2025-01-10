namespace Payhub.Application.Common.DTOs.Blacklists;

public class BlacklistDto
{
    public int Id { get; set; }
    public int? BlacklistId { get; set; }
    public string? SiteCustomerId { get; set; } // siteden gelen customer'ın ID si
    public string? FullName { get; set; }
    public string? Username { get; set; }
    public string? SignupDate { get; set; }
    public string? IdentityNumber { get; set; }
    public string? CustomerIpAddress { get; set; }
    public string PanelCustomerId { get; set; } = null!; // siteName + customerId
    public string SiteName { get; set; } = null!;

    public int ConfirmedDepositCount { get; set; }
    public int ConfirmedWithdrawCount { get; set; }
    public decimal ConfirmedDepositAmount { get; set; }
    public decimal ConfirmedWithdrawAmount { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}