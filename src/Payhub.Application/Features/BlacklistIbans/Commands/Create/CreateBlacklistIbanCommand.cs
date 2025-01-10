using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.BlacklistIbans.Commands.Create;

public sealed record CreateBlacklistIbanCommand : ICommand<int>
{
    public string Iban { get; set; } = string.Empty;
}