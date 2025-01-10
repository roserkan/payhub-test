using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Abstractions.Messaging;
using Shared.CrossCuttingConcerns.Exceptions.ProblemDetails.Models;
using Shared.Utils.Security.Extensions;

namespace Payhub.Application.Common.Pipelines.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Stopwatch _timer;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger, 
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _timer = new Stopwatch();
    }

    public async Task<TResponse> Handle(TRequest request, 
                                        RequestHandlerDelegate<TResponse> next, 
                                        CancellationToken cancellationToken)    
    {
        // TRequest'in ICommand<T> arayüzünden türediğini kontrol edin.
        var isCommand = typeof(TRequest).GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>));

        // Eğer request ICommand<T> değilse işlemi devam ettir ama loglama yapma.
        if (!isCommand)
        {
            return await next();
        }
        _timer.Start();

        var log = new GeneralLog
        {
            UserId = _httpContextAccessor.HttpContext?.User.GetUserId(),
            IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
            LocalIpAddress = _httpContextAccessor.HttpContext?.Connection.LocalIpAddress?.ToString(),
            Request = request,
            Response = null,
            Url = _httpContextAccessor.HttpContext?.Request.Path,
            HttpMethod = _httpContextAccessor.HttpContext?.Request.Method,
            UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString(),
            Referer = _httpContextAccessor.HttpContext?.Request.Headers["Referer"].ToString(),
            RequestHeaders = _httpContextAccessor.HttpContext?.Request.Headers
                .ToDictionary(h => h.Key, h => h.Value.ToString()),
            StatusCode = 0,
            ElapsedTime = null,
            HandlerName = typeof(TRequest).Name,
            ExceptionMessage = null,
            StackTrace = null,
            InnerExceptionMessage = null,
            ExceptionType = null,
            CustomMessage = null
        };

        TResponse response;
        try
        {
            response = await next();
            _timer.Stop();

            log.Response = response;
            log.ElapsedTime = _timer.ElapsedMilliseconds;
            
            _logger.LogInformation("{@GeneralLog}", log);
        }
        catch (Exception)
        {
            _timer.Stop();
            log.ElapsedTime = _timer.ElapsedMilliseconds;
            
            _httpContextAccessor.HttpContext!.Items["ExceptionLog"] = log;
            throw; 
        }

        return response;
    }
}
