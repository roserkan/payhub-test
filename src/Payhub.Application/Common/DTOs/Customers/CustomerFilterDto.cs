namespace Payhub.Application.Common.DTOs.Customers;

public sealed record CustomerFilterDto
{
    public string? SearchValue { get; set; }
}