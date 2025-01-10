namespace Payhub.Application.Common.DTOs.PaymentWays;

public sealed record PaymentWayDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}