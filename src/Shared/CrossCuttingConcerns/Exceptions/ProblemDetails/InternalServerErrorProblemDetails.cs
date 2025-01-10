using Shared.CrossCuttingConcerns.Exceptions.ProblemDetails.Models;
using Microsoft.AspNetCore.Http;

namespace Shared.CrossCuttingConcerns.Exceptions.ProblemDetails;

public class InternalServerErrorProblemDetails : ProblemDetailModel
{
    public InternalServerErrorProblemDetails(string detail)
    {
        Title = "Internal Server Error";
        Detail = "Internal Server Error";
        Status = StatusCodes.Status500InternalServerError;
    }
}