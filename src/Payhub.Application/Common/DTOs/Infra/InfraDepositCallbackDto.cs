namespace Payhub.Application.Common.DTOs.Infra;

public class InfraDepositCallbackDto
{
    public string CustomerId { get; set; } = null!;
    public string ProcessId { get; set; } = null!;
    public decimal Amount { get; set; }
    public string SecurityKey { get; set; } = null!;
    public bool Status { get; set; }
}