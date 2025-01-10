using System.Text.Json.Serialization;

namespace Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.ResponseModels;

public class PayoxAvailableBankResponse
{
    [JsonPropertyName("_id")] 
    public string Id { get; set; } = null!;
}