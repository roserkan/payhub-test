using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.AffiliateSafeMoves.Commands.Create;

public sealed record CreateAffiliateSafeMoveCommand : ICommand<int>
{
    public int AffiliateId { get; set; }
    public AffiliateSafeMoveType MoveType { get; set; }
    public decimal Amount { get; set; }
    public decimal CommissionRate { get; set; }
    public decimal CommissionAmount { get; set; }
    public AffiliateSafeMoveTransactionType TransactionType { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; }
}