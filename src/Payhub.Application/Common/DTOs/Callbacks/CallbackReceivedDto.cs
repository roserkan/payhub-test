namespace Payhub.Application.Common.DTOs.Callbacks;

public sealed record CallbackReceivedDto
{
    public string Message { get; set; }
    public bool Success { get; set; }
    
    public CallbackReceivedDto(string message, bool success)
    {
        Message = message;
        Success = success;
    }
}