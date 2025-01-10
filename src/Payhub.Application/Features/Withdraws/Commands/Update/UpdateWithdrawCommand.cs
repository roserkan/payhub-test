using Payhub.Application.Common.Pipelines.Logging;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Withdraws.Commands.Update;

public sealed record UpdateWithdrawCommand : ICommand<int>, ILogRequest
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string CustomerFullName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}