using Microsoft.AspNetCore.Http.HttpResults;

namespace Payhub.Application.Features.HavaleBots.BotResults;

public class BotResult<T>
{
    public T Data { get; set; }
    public string Message { get; set; } = null!;
    public bool Success { get; set; }
    
    public static BotResult<T> Ok(T data, string message = null!)
    {
        return new BotResult<T>
        {
            Data = data,
            Message = message,
            Success = true
        };
    }
    
    public static BotResult<T> Fail(T data, string message = null!)
    {
        return new BotResult<T>
        {
            Data = data,
            Message = message,
            Success = false
        };
    }
}