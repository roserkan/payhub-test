using Payhub.Application.Common.DTOs.Payments;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.Payments.Queries.GetDepositForPayment;

public sealed record GetDepositForPaymentQuery : IQuery<DepositPaymentDto>
{
    public string PaymentId { get; set; }
    
    public GetDepositForPaymentQuery(string paymentId)
    {
        PaymentId = paymentId;
    }
}