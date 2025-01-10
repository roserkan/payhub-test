namespace Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Results;

public class ApiResponse<T>
{
    public T Data { get; set; } = default!;
    public int Status { get; set; }
    public string? Message { get; set; }
}