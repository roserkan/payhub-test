using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Infrastructures.Commands.Update;

public sealed record UpdateInfrastructureCommand : ICommand<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string DepositAddress { get; set; } = null!;
    public string WithdrawAddress { get; set; } = null!;
}