namespace Payhub.Application.Common.DTOs.Infrastructures;

public sealed record InfrastructureDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string DepositAddress { get; set; } = string.Empty;
    public string WithdrawAddress { get; set; } = string.Empty;
}