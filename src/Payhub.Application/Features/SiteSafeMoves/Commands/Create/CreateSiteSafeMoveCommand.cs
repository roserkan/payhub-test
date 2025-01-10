using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.SiteSafeMoves.Commands.Create;

public sealed record CreateSiteSafeMoveCommand : ICommand<int>
{
    public int SiteId { get; set; }
    public SiteSafeMoveType MoveType { get; set; }
    public decimal Amount { get; set; }
    public decimal CommissionRate { get; set; }
    public decimal CommissionAmount { get; set; }
    public SiteSafeMoveTransactionType TransactionType { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionDate { get; set; }
}