using Payhub.Application.Common.Pipelines.Logging;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Deposits.Commands.UpdateStatus;

public sealed record UpdateDepositStatusCommand : ICommand<int>, ILogRequest
{
    public int DepositId { get; set; }
    public DepositStatus Status { get; set; }
    public bool SendToInfra { get; set; } 
}