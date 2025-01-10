using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Consumers;
using Payhub.Application.Common.Services;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Services;
using Shared.CrossCuttingConcerns.Authorization;
using Shared.CrossCuttingConcerns.Exceptions.ProblemDetails.Models;

namespace Payhub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public LogsController(IUnitOfWork unitOfWork, HttpClient httpClient, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _httpClient = httpClient;
        _configuration = configuration;
    }
    
    [HttpGet("{processId}")]
    [HasPermission(["log-list"])]
    public async Task<IActionResult> GetLogs(string processId)
    {
        GeneralLog? transactionRequestLog = null;
        IEnumerable<GeneralLog>? transactionStatusLogs = null;
        GeneralLog? payoxCallbackLog = null;

        var deposit = await _unitOfWork.DepositRepository.GetAsync(i => i.ProcessId == processId, cancellationToken: default);
        var withdraw = await _unitOfWork.WithdrawRepository.GetAsync(i => i.ProcessId == processId, cancellationToken: default);
        if (deposit != null)
        {
            int depositId = deposit?.Id ?? 0;
        
            var depositGeneralLogsFromSeq = await GetSeqLog($"GeneralLog.Response.ProcessId = '{processId}' or GeneralLog.Response = {depositId} or (GeneralLog.Request.ProcessId='{processId}' and GeneralLog.HandlerName = 'PayoxCallbackCommand')");
            var depositGeneralLogs = depositGeneralLogsFromSeq
                .OrderByDescending(i => i.Timestamp)
                .SelectMany(log => log.Properties)
                .Where(prop => prop.Name == "GeneralLog")
                .Select(prop => prop.Value)
                .Select(value => JsonSerializer.Deserialize<GeneralLog>(value.ToString()));
            
            transactionRequestLog = depositGeneralLogs.FirstOrDefault(i => i.HandlerName == "CreateDepositCommand");
            transactionStatusLogs = depositGeneralLogs.Where(i => i.HandlerName == "UpdateDepositStatusCommand");
            payoxCallbackLog = depositGeneralLogs.FirstOrDefault(i => i.HandlerName == "PayoxCallbackCommand");
        }
        else if (withdraw != null)
        {
            int withdrawId = withdraw?.Id ?? 0;
        
            var withdrawGeneralLogsFromSeq = await GetSeqLog($"GeneralLog.Response.ProcessId = '{processId}' or GeneralLog.Response = {withdrawId} or (GeneralLog.Request.ProcessId='{processId}' and GeneralLog.HandlerName = 'PayoxCallbackCommand')");

            var withdrawGeneralLogs = withdrawGeneralLogsFromSeq
                .OrderByDescending(i => i.Timestamp)
                .SelectMany(log => log.Properties)
                .Where(prop => prop.Name == "GeneralLog")
                .Select(prop => prop.Value)
                .Select(value => JsonSerializer.Deserialize<GeneralLog>(value.ToString()));
            
            transactionRequestLog = withdrawGeneralLogs.FirstOrDefault(i => i.HandlerName == "CreateWithdrawCommand");
            transactionStatusLogs = withdrawGeneralLogs.Where(i => i.HandlerName == "UpdateWithdrawStatusCommand");
            payoxCallbackLog = withdrawGeneralLogs.FirstOrDefault(i => i.HandlerName == "PayoxCallbackCommand");
        }

       
        
        var infraLogsFromSeq = await GetSeqLog($"InfraLog.Request.ProcessId = '{processId}'");
        var infraLogs = infraLogsFromSeq
            .OrderByDescending(i => i.Timestamp)
            .SelectMany(log => log.Properties)
            .Where(prop => prop.Name == "InfraLog")
            .Select(prop => prop.Value)
            .Select(value => JsonSerializer.Deserialize<InfraLog>(value.ToString()));
        
        var payoxLogsFromSeq = await GetSeqLog($"PayoxLog.Request.processId = '{processId}'");
        var payoxLogs = payoxLogsFromSeq
            .OrderByDescending(i => i.Timestamp)
            .SelectMany(log => log.Properties)
            .Where(prop => prop.Name == "PayoxLog")
            .Select(prop => prop.Value)
            .Select(value => JsonSerializer.Deserialize<PayoxLog>(value.ToString()));
        var payoxLog = payoxLogs.FirstOrDefault(i => i.RequestType == "PayoxCreateDeposit");
        
        var consumerLogsFromSeq = await GetSeqLog($"ConsumerLog.ProcessId = '{processId}'");
        var consumerLogs = consumerLogsFromSeq
            .OrderByDescending(i => i.Timestamp)
            .SelectMany(log => log.Properties)
            .Where(prop => prop.Name == "ConsumerLog")
            .Select(prop => prop.Value)
            .Select(value => JsonSerializer.Deserialize<ConsumerLog>(value.ToString()));
        var consumerLog = consumerLogs.FirstOrDefault(i => i.ProcessId == processId);
        
        var result = new LogResult
        {
            RequestLog = transactionRequestLog,
            StatusLogs = transactionStatusLogs,
            InfraLog = infraLogs.FirstOrDefault(),
            PayoxLog = payoxLog,
            PayoxCallbackLog = payoxCallbackLog,
            ConsumerLog = consumerLog
        };
        
        return Ok(result);
    }

    private async Task<List<LogRoot>> GetSeqLog(string filter)
    {
        string seqApiUrl = _configuration["SeqConfig:ApiUrl"]!;
        string apiKey = _configuration["SeqConfig:ApiKey"]!;  // //"oAsoL4sTLLeZJ1tj2Rk6";
        int count = 100;

        // HTTP isteği için URL'yi oluştur
        var requestUrl = $"{seqApiUrl}?filter={filter}&count={count}";

        // API anahtarını header olarak ekle
        _httpClient.DefaultRequestHeaders.Remove("X-Seq-ApiKey"); // Daha önce eklenmişse kaldır.
        _httpClient.DefaultRequestHeaders.Add("X-Seq-ApiKey", apiKey);

        try
        {
            // GET isteğini gönder
            var response = await _httpClient.GetAsync(requestUrl);

            // Yanıt başarılı mı kontrol et
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"HTTP Error: {response.StatusCode}, Details: {await response.Content.ReadAsStringAsync()}");
            }

            // Yanıtı JSON olarak al
            var jsonLogs = await response.Content.ReadAsStringAsync();

            // JSON'u belirtilen türe deserialize et
            var logs = JsonSerializer.Deserialize<List<LogRoot>>(jsonLogs, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return logs;
        }
        catch (Exception ex)
        {
            throw new Exception($"Hata: {ex.Message}", ex);
        }
    }
}


// Loglar için kullanılan modeller
public class LogRoot
{
    public string Timestamp { get; set; } = null!;
    public List<LogProperty> Properties { get; set; } = null!;
}

public class LogProperty
{
    public string Name { get; set; } = null!;
    public object Value { get; set; } = null!;
}

public class LogResult
{
    public GeneralLog? RequestLog { get; set; }
    public IEnumerable<GeneralLog>? StatusLogs { get; set; }
    public InfraLog? InfraLog { get; set; }
    public PayoxLog? PayoxLog { get; set; }
    public GeneralLog? PayoxCallbackLog { get; set; }
    public ConsumerLog? ConsumerLog { get; set; }
}