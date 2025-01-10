using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.PaymentWays;
using Shared.Abstractions.Messaging;

namespace Payhub.Application.Features.PaymentWays.Queries.GetAll;

public sealed record GetAllPaymentWaysQueryHandler : IQueryHandler<GetAllPaymentWaysQuery, IEnumerable<PaymentWayDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllPaymentWaysQueryHandler(IUnitOfWork unitOfWork) 
        => _unitOfWork = unitOfWork;
    
    public async Task<IEnumerable<PaymentWayDto>> Handle(GetAllPaymentWaysQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.PaymentWayRepository.GetAllWithSelectorAsync<PaymentWayDto>(
            selector: pw => new PaymentWayDto
            {
                Id = pw.Id,
                Name = pw.Name
            }, cancellationToken: cancellationToken);
        
        return result;
    }
}