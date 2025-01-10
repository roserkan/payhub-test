using Payhub.Application.Common.Pipelines.Logging;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Withdraws.Commands.UpdateStatus;

public sealed record UpdateWithdrawStatusCommand : ICommand<int>, ILogRequest
{
    public int WithdrawId { get; set; }
    public WithdrawStatus Status { get; set; }
    public bool SendToInfra { get; set; }
    public int? AccountId { get; set; }
}