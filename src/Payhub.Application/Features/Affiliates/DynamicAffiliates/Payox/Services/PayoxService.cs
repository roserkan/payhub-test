using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Helpers;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.RequestModels;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.ResponseModels;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Results;
using Payhub.Domain.Enums;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Services;

public class PayoxService : IPayoxService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _options;
    private readonly ILogger<PayoxService> _logger;

    public PayoxService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<PayoxService> logger)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private (string ApiKey, string ApiSecret, string Url) GetSettings(int paymentWayId, string type)
    {
        string prefix = paymentWayId == (int)PaymentWayEnum.Havale ? "Payox" : "PayoxPapara";
        return (
            _configuration[$"AffiliateSettings:{prefix}:ApiKey"]!,
            _configuration[$"AffiliateSettings:{prefix}:ApiSecret"]!,
            _configuration[$"AffiliateSettings:{prefix}:{type}Url"]!
        );
    }

    private async Task<T> SendRequestAsync<T>(string url, HttpMethod method, Dictionary<string, string> requestData, string apiKey, string apiSecret, string requestType, int paymentWayId)
    {
        var elapsedWatch = Stopwatch.StartNew();
        var log = new PayoxLog
        {
            RequestType = requestType,
            PaymentWayId = paymentWayId,
            Url = url,
            Request = requestData,
            Response = null,
            ElapsedTime = 0,
            Message = "Payox isteği başlıyor",
            ErrorMessage = null
        };
        string sign = SignatureHelper.CreateSignature(requestData, apiSecret);

        using var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("appKey", apiKey);
        client.DefaultRequestHeaders.Add("sign", sign);

        HttpResponseMessage response;
        try
        {
            if (method == HttpMethod.Post)
            {
                var content = new FormUrlEncodedContent(requestData);
                response = await client.PostAsync(url, content).ConfigureAwait(false);
                log.Message += " -- İstek gönderildi";
            }
            else
            {
                response = await client.GetAsync(url).ConfigureAwait(false);
                log.Message += " -- İstek gönderildi";
            }

            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            log.Response = responseData;
            log.Message += " -- Cevap alındı";

            if (!response.IsSuccessStatusCode)
            {
                log.ErrorMessage = $"Payox Error: bir sorun oluştu";
                throw new BusinessException($"Payox Error: {method.Method} request failed");
            }

            var responseModel = JsonSerializer.Deserialize<T>(responseData, _options);
            log.Response = responseData;

            if (responseModel == null)
            {
                log.ErrorMessage = $"Payox Error: Response parsing failed";
                throw new BusinessException("Payox Error: Response parsing failed");
            }

            return responseModel;
        }
        catch (Exception ex)
        {
            log.ErrorMessage = $"Payox Error: {ex.Message}";
            throw;
        }
        finally
        {
            elapsedWatch.Stop();
            log.ElapsedTime = elapsedWatch.ElapsedMilliseconds;
            if (log.ErrorMessage == null)
                _logger.LogInformation("{@PayoxLog}", log);
            else
                _logger.LogError("{@PayoxLog}", log);
        }
    }

    public async Task<ApiResponse<PayoxCreateDepositResponse>> CreateDepositAsync(PayoxCreateDepositRequest request, int paymentWayId)
    {
        var (apiKey, apiSecret, url) = GetSettings(paymentWayId, "Deposit");
        
        var requestData = new Dictionary<string, string>
        {
            { "bankId", request.BankId },
            { "amount", request.Amount.ToString() },
            { "userId", request.UserId },
            { "name", request.Name },
            { "userName", request.UserName },
            { "processId", request.ProcessId }
        };

        return await SendRequestAsync<ApiResponse<PayoxCreateDepositResponse>>(url, HttpMethod.Post, requestData, apiKey, apiSecret, "PayoxCreateDeposit", paymentWayId);
    }

    public async Task<ApiResponse<PayoxCreateWithdrawResponse>> CreateWithdrawAsync(PayoxCreateWithdrawRequest request, int paymentWayId)
    {
        var (apiKey, apiSecret, url) = GetSettings(paymentWayId, "Withdraw");

        var requestData = new Dictionary<string, string>
        {
            { "bankId", request.BankId },
            { "amount", request.Amount.ToString() },
            { "userId", request.UserId },
            { "name", request.Name },
            { "userName", request.UserName },
            { "accountName", request.AccountName },
            { "iban", request.Iban },
            { "processId", request.ProcessId }
        };

        return await SendRequestAsync<ApiResponse<PayoxCreateWithdrawResponse>>(url, HttpMethod.Post, requestData, apiKey, apiSecret, "PayoxCreateWithdraw", paymentWayId);
    }

    public async Task<ApiResponse<List<PayoxAvailableBankResponse>>> GetAvailableBanksAsync(int paymentWayId)
    {
        var (apiKey, apiSecret, url) = GetSettings(paymentWayId, "AvailableBanks"); 

        return await SendRequestAsync<ApiResponse<List<PayoxAvailableBankResponse>>>(url, HttpMethod.Get, null!, apiKey, apiSecret, "PayoxGetAvailableBanks", paymentWayId);
    }
}

public class PayoxLog
{
    public string? RequestType { get; set; }
    public int? PaymentWayId { get; set; }
    public string? Url { get; set; }
    public object? Request { get; set; }
    public object? Response { get; set; }
    public long ElapsedTime { get; set; }
    public string? Message { get; set; }
    public string? ErrorMessage { get; set; }
}
