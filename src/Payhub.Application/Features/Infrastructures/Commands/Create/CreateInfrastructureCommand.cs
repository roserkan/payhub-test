using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Infrastructures.Commands.Create;

public sealed record CreateInfrastructureCommand : ICommand<int>
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string DepositAddress { get; set; } = null!;
    public string WithdrawAddress { get; set; } = null!;
}