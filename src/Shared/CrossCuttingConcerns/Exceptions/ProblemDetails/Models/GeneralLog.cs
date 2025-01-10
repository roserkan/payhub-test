namespace Shared.CrossCuttingConcerns.Exceptions.ProblemDetails.Models;

public class GeneralLog
{
    public int? UserId { get; set; }
    public string? IpAddress { get; set; }
    public string? LocalIpAddress { get; set; }
    public object? Request { get; set; }
    public object? Response { get; set; }
    public string? Url { get; set; }
    public string? HttpMethod { get; set; }
    public string? UserAgent { get; set; }
    public string? Referer { get; set; }
    public Dictionary<string, string>? RequestHeaders { get; set; }
    public int StatusCode { get; set; }
    public long? ElapsedTime { get; set; }
    public string? HandlerName { get; set; }
    public string? ExceptionMessage { get; set; }
    public string? StackTrace { get; set; }
    public string? InnerExceptionMessage { get; set; }
    public string? ExceptionType { get; set; }
    public string? CustomMessage { get; set; }
}
