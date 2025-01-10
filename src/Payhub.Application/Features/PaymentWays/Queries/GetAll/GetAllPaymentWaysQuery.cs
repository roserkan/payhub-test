using Payhub.Application.Common.DTOs.PaymentWays;

using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.PaymentWays.Queries.GetAll;

public sealed record GetAllPaymentWaysQuery : IQuery<IEnumerable<PaymentWayDto>>
{
}