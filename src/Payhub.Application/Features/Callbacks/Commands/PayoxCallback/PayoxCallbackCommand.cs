using Payhub.Application.Common.DTOs.Callbacks;
using Payhub.Application.Common.Pipelines.Logging;
using Payhub.Application.Features.Affiliates.DynamicAffiliates.Payox.Models;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Callbacks.Commands.PayoxCallback;

public sealed class PayoxCallbackCommand : PayoxCallbackPayload, ICommand<CallbackReceivedDto>, ILogRequest
{
}