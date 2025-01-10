using Payhub.Application.Common.DTOs.Infra;

namespace Payhub.Application.Abstractions.Services;

public interface IInfraService
{
    Task<InfraDepositCallbackResponseDto> SendDepositCallbackAsync(InfraDepositCallbackDto dto, string endpoint);
    Task<InfraWithdrawCallbackResponseDto> SendWithdrawCallbackAsync(InfraWithdrawCallbackDto dto, string endpoint);
}