namespace Shared.CrossCuttingConcerns.Exceptions.ProblemDetails.Models;

public class ProblemDetailModel
{
    public string Title { get; set; } = "An error occurred.";
    public string Detail { get; set; } = "An error occurred while processing your request.";
    public int Status { get; set; }
}