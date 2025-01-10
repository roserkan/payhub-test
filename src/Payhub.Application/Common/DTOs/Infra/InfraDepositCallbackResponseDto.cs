namespace Payhub.Application.Common.DTOs.Infra;

public class InfraDepositCallbackResponseDto
{
    public string Message { get; set; } = null!;
    
    public bool Success { get; set; }
}