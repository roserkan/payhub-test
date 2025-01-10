using Payhub.Application.Common.Pipelines.Logging;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Payments.Commands.Payed;

public sealed record DepositPayedCommand : ICommand<int>, ILogRequest
{
    public int DepositId { get; set; }
    
    public DepositPayedCommand(int depositId)
    {
        DepositId = depositId;
    }
}