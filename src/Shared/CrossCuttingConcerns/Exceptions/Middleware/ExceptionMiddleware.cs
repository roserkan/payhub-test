using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.CrossCuttingConcerns.Exceptions.Handlers;
using Shared.CrossCuttingConcerns.Exceptions.ProblemDetails.Models;

namespace Shared.CrossCuttingConcerns.Exceptions.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger; // Türü burada belirtin
    private readonly HttpExceptionHandler _httpExceptionHandler;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _httpExceptionHandler = new HttpExceptionHandler();
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);

            var log = context.Items["ExceptionLog"] as GeneralLog;
            if (log != null)
            {
                log.Response = _httpExceptionHandler.ResponseContent;
                log.ExceptionMessage = ex.Message;
                log.StackTrace = ex.StackTrace;
                log.StatusCode = _httpExceptionHandler.Response.StatusCode;
                log.InnerExceptionMessage = ex.InnerException?.Message;

                _logger.LogError("{@GeneralLog}", log);
            }
            else
            {
                var errLog = new ExceptionLog
                {
                    HandlerName = "ExceptionMiddleware",
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerExceptionMessage = ex.InnerException?.Message,
                    ExceptionType = ex.GetType().Name
                };
                _logger.LogError("{@ExceptionLog}", errLog);
            }
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        _httpExceptionHandler.Response = context.Response;
        return _httpExceptionHandler.HandleExceptionAsync(exception);
    }

    public class ExceptionLog
    {
        public string? HandlerName { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public string? InnerExceptionMessage { get; set; }
        public string? ExceptionType { get; set; }
    }
}