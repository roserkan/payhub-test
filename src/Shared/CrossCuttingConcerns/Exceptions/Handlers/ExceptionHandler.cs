using Shared.CrossCuttingConcerns.Exceptions.Types;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Shared.CrossCuttingConcerns.Exceptions.Handlers;

public abstract class ExceptionHandler
{
    public Task HandleExceptionAsync(Exception exception)
    {
        if (exception is BusinessException businessException)
            return HandleException(businessException);

        if (exception is ValidationException validationException)
            return HandleException(validationException);

        if (exception is AuthorizationException authorizationException)
            return HandleException(authorizationException);

        if (exception is NotFoundException notFoundException)
            return HandleException(notFoundException);
        
        if (exception is DbUpdateException dbUpdateException)
        {
            if (dbUpdateException.InnerException is PostgresException postgresException &&
                postgresException.SqlState == "23505") // Unique constraint violation code
            {
                string constraintName = postgresException.ConstraintName ?? "Unkown";

                var ex = new BusinessException($"'{constraintName}' already exists");
                return HandleException(ex);
            }
        }
        return HandleException(exception);
    }

    protected abstract Task HandleException(BusinessException businessException);
    protected abstract Task HandleException(ValidationException validationException);
    protected abstract Task HandleException(AuthorizationException authorizationException);
    protected abstract Task HandleException(NotFoundException notFoundException);
    protected abstract Task HandleException(Exception exception);
}