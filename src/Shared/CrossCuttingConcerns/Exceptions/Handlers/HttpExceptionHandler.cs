using System.Text.Json;
using Shared.CrossCuttingConcerns.Exceptions.ProblemDetails;
using Shared.CrossCuttingConcerns.Exceptions.Types;
using Microsoft.AspNetCore.Http;

namespace Shared.CrossCuttingConcerns.Exceptions.Handlers;

public class HttpExceptionHandler : ExceptionHandler
{
    public object? ResponseContent { get; private set; }
    public HttpResponse Response
    {
        get => _response ?? throw new ArgumentNullException(nameof(_response));
        set => _response = value;
    }

    private HttpResponse? _response;
    
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    protected override Task HandleException(BusinessException businessException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        ResponseContent = new BusinessProblemDetails(businessException.Message);
        string details = JsonSerializer.Serialize(ResponseContent, _jsonSerializerOptions);
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(ValidationException validationException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        ResponseContent = new ValidationProblemDetails(validationException.Errors);
        string details = JsonSerializer.Serialize(ResponseContent, _jsonSerializerOptions);
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(AuthorizationException authorizationException)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        ResponseContent = new AuthorizationProblemDetails(authorizationException.Message);
        string details = JsonSerializer.Serialize(ResponseContent, _jsonSerializerOptions);
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(NotFoundException notFoundException)
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        string details = JsonSerializer.Serialize(new NotFoundProblemDetails(notFoundException.Message), _jsonSerializerOptions);
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(Exception exception)
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        ResponseContent = new InternalServerErrorProblemDetails(exception.Message);
        string details = JsonSerializer.Serialize(ResponseContent, _jsonSerializerOptions);
        return Response.WriteAsync(details);
    }
}