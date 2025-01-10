namespace Payhub.Application.Common.DTOs.Consumers;

public class ConsumerLog
{
    public string? ConsumeType { get; set; }
    public string? Message { get; set; }
    public string? ErrorMessage { get; set; }
    public string ProcessId { get; set; } = null!;
    public long ElapsedTime { get; set; }
}