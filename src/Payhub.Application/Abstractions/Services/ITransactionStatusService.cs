using Payhub.Domain.Entities.TransactionManagement;
using Payhub.Domain.Enums;

namespace Payhub.Application.Abstractions.Services;

public interface ITransactionStatusService
{
    Task<Deposit> UpdateDepositStatusAsync(Deposit? deposit, DepositStatus status, bool sendToInfra, string? updatedName, CancellationToken cancellationToken);
    Task<Withdraw> UpdateWithdrawStatusAsync(Withdraw? withdraw, WithdrawStatus status, bool sendToInfra, int? accountId, string? updatedName, CancellationToken cancellationToken);
}
