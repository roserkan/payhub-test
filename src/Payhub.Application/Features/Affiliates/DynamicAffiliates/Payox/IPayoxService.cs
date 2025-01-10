using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.RequestModels;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models.ResponseModels;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Results;

namespace Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox;

public interface IPayoxService
{
    Task<ApiResponse<PayoxCreateDepositResponse>> CreateDepositAsync(PayoxCreateDepositRequest request, int paymentWayId);
    Task<ApiResponse<PayoxCreateWithdrawResponse>> CreateWithdrawAsync(PayoxCreateWithdrawRequest request, int paymentWayId);
    Task<ApiResponse<List<PayoxAvailableBankResponse>>> GetAvailableBanksAsync(int paymentWayId);
}