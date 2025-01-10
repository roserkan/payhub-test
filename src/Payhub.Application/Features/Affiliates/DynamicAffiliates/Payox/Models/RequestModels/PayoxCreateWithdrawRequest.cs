using System.Text.Json.Serialization;

namespace Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.RequestModels;

public class PayoxCreateWithdrawRequest
{
    [JsonPropertyName("bankId")]
    public string BankId { get; set; } = null!;
    
    [JsonPropertyName("processId")]
    public string ProcessId { get; set; } = null!;
    
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; } 
    
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = null!;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    
    [JsonPropertyName("userName")]
    public string UserName { get; set; } = null!;

    [JsonPropertyName("accountName")]
    public string AccountName { get; set; } = null!;

    [JsonPropertyName("iban")]
    public string Iban { get; set; } = null!;
}