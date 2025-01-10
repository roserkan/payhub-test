using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Common.DTOs.Infra;
using Shared.CrossCuttingConcerns.Exceptions.Types;

namespace Payhub.Application.Common.Services;

public class InfraService : IInfraService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _options;
    private readonly ILogger<InfraService> _logger;

    public InfraService(IHttpClientFactory httpClientFactory, ILogger<InfraService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    } 
    
    public async Task<TResponseDto> SendCallbackAsync<TRequestDto, TResponseDto>(TRequestDto dto, string endpoint, string callbackType)
    {
        var elapsedWatch = Stopwatch.StartNew();
        var infraLog = new InfraLog
        {
            Request = dto,
            Response = null,
            Endpoint = endpoint,
            CallbackType = callbackType,
            ElapsedTime = 0,
            Message = "Callback gönderiliyor",
            ErrorMessage = null
        };
        
        string responseData = string.Empty;
        try
        {
            var json = JsonSerializer.Serialize(dto, _options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync(endpoint, content).ConfigureAwait(false);
            infraLog.Message += " -- Callback gönderildi";
            responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                infraLog.ErrorMessage = "Callback gönderildi fakat yanıt alınamadı";
                throw new BusinessException($"{callbackType} Hatası: Yanıt alınamadı");
            }

            var responseModel = JsonSerializer.Deserialize<TResponseDto>(responseData, _options)
                ?? throw new BusinessException($"{callbackType} Hatası: Yanıt deserialize edilemedi");

            infraLog.Message += " -- Callback gönderme başarılı";
            infraLog.Response = responseModel;
            return responseModel;
        }
        catch (JsonException jsonEx)
        {
            infraLog.ErrorMessage = $"{callbackType} JSON Deserialize Hatası: {jsonEx.Message}";
            throw new BusinessException($"{callbackType} Hatası: JSON Deserialize Hatası: " + jsonEx.Message);
        }
        catch (Exception ex)
        {
            infraLog.ErrorMessage = $"{callbackType} İşlem Hatası: {ex.Message}";
            throw new BusinessException($"{callbackType} Hatası: " + ex.Message);
        }
        finally
        {
            elapsedWatch.Stop();
            infraLog.ElapsedTime = elapsedWatch.ElapsedMilliseconds;
            if (infraLog.ErrorMessage == null)
                _logger.LogInformation("{@InfraLog}", infraLog);
            else
                _logger.LogError("{@InfraLog}", infraLog);
        }
    }


    public async Task<InfraDepositCallbackResponseDto> SendDepositCallbackAsync(InfraDepositCallbackDto dto, string endpoint)
    {
        return await SendCallbackAsync<InfraDepositCallbackDto, InfraDepositCallbackResponseDto>(dto, endpoint, "Deposit");
    }

    public async Task<InfraWithdrawCallbackResponseDto> SendWithdrawCallbackAsync(InfraWithdrawCallbackDto dto, string endpoint)
    {
        return await SendCallbackAsync<InfraWithdrawCallbackDto, InfraWithdrawCallbackResponseDto>(dto, endpoint, "Withdraw");
    }
}

public class InfraLog
{
    public object? Request { get; set; }
    public object? Response { get; set; }
    public string? Endpoint { get; set; }
    public string? CallbackType { get; set; }
    public long ElapsedTime { get; set; }
    public string? Message { get; set; }
    public string? ErrorMessage { get; set; }
}