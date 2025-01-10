namespace Payhub.Application.Common.DTOs.Sites;

public sealed record CreateSpwDto
{
    public int PaymentWayId { get; set; }
    public bool IsActive { get; set; }
    public decimal Commission { get; set; }
    public decimal MinBalanceLimit { get; set; }
    public decimal MaxBalanceLimit { get; set; }
}