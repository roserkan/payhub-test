namespace Payhub.Application.Common.DTOs.Sites;

public sealed record SpwDto
{
    public int Id { get; set; }
    public int PaymentWayId { get; set; }
    public bool IsActive { get; set; }
    public string ApiKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public decimal Commission { get; set; }
    public decimal MinBalanceLimit { get; set; }
    public decimal MaxBalanceLimit { get; set; }
}