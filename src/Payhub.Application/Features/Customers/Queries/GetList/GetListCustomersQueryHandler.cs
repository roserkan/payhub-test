using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Customers;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;
using Shared.Utils.Pagination;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.Customers.Queries.GetList;

public sealed class GetListCustomersQueryHandler : IQueryHandler<GetListCustomersQuery, PaginatedResult<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetListCustomersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<PaginatedResult<CustomerDto>> Handle(GetListCustomersQuery request, CancellationToken cancellationToken)
    {
        var query = from customer in _unitOfWork.CustomerRepository.Query()
                    join deposit in _unitOfWork.DepositRepository.Query()
                        on customer.PanelCustomerId equals deposit.PanelCustomerId into customerDeposits
                    from deposit in customerDeposits.DefaultIfEmpty()
                    join withdraw in _unitOfWork.WithdrawRepository.Query()
                        on customer.PanelCustomerId equals withdraw.PanelCustomerId into customerWithdraws
                    from withdraw in customerWithdraws.DefaultIfEmpty()
                    where string.IsNullOrEmpty(request.CustomerFilterDto.SearchValue) || 
                          (customer.PanelCustomerId != null && customer.PanelCustomerId.ToLower().Contains(request.CustomerFilterDto.SearchValue.ToLower())) ||
                          (customer.FullName != null && customer.FullName.ToLower().Contains(request.CustomerFilterDto.SearchValue.ToLower())) ||
                          (customer.Username != null && customer.Username.ToLower().Contains(request.CustomerFilterDto.SearchValue.ToLower())) ||
                          (customer.CustomerIpAddress != null && customer.CustomerIpAddress.ToLower().Contains(request.CustomerFilterDto.SearchValue.ToLower()))

                    group new { deposit, withdraw } by new
                    {
                        customer.Id,
                        customer.SiteCustomerId,
                        customer.FullName,
                        customer.Username,
                        customer.CustomerIpAddress,
                        customer.SignupDate,
                        customer.IdentityNumber,
                        customer.PanelCustomerId,
                        SiteName = customer.Site.Name,
                        customer.CreatedDate,
                        customer.UpdatedDate
                    } into g
                    select new CustomerDto
                    {
                        Id = g.Key.Id,
                        SiteCustomerId = g.Key.SiteCustomerId,
                        FullName = g.Key.FullName,
                        Username = g.Key.Username,
                        CustomerIpAddress = g.Key.CustomerIpAddress,
                        SignupDate = g.Key.SignupDate,
                        IdentityNumber = g.Key.IdentityNumber,
                        PanelCustomerId = g.Key.PanelCustomerId,
                        SiteName = g.Key.SiteName,
                        ConfirmedDepositCount = g.Count(x => x.deposit != null && x.deposit.Status == DepositStatus.Confirmed),
                        ConfirmedDepositAmount = g.Where(x => x.deposit != null && x.deposit.Status == DepositStatus.Confirmed)
                                                  .Sum(x => (decimal?)x.deposit.Amount) ?? 0,
                        ConfirmedWithdrawCount = g.Count(x => x.withdraw != null && x.withdraw.Status == WithdrawStatus.Confirmed),
                        ConfirmedWithdrawAmount = g.Where(x => x.withdraw != null && x.withdraw.Status == WithdrawStatus.Confirmed)
                                                   .Sum(x => (decimal?)x.withdraw.Amount) ?? 0,
                        CreatedDate = g.Key.CreatedDate,
                        UpdatedDate = g.Key.UpdatedDate
                    };

        var result = await query.ToPaginateAsync(request.PageRequest.Index, request.PageRequest.Size, 0, cancellationToken);
        var paginatedResult = new PaginatedResult<CustomerDto>
        {
            Items = result.Items,
            Index = result.Index,
            Size = result.Size,
            Count = result.Count,
            Pages = result.Pages,
            HasPrevious = result.HasPrevious,
            HasNext = result.HasNext
        };
        return paginatedResult;
    }

}