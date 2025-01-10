using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Common.DTOs.Accounts;
using Payhub.Application.Common.DTOs.WithdrawOrders;
using Shared.Abstractions.Messaging;
using Shared.Utils.Pagination;
using Shared.Utils.Responses;

namespace Payhub.Application.Features.WithdrawOrders.Queries.GetAll;

public sealed class GetAllWithdrawOrdersQueryHandler : IQueryHandler<GetAllWithdrawOrdersQuery, PaginatedResult<WithdrawOrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllWithdrawOrdersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<PaginatedResult<WithdrawOrderDto>> Handle(GetAllWithdrawOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.WithdrawOrders.Query()
            .Where(i => i.CreatedDate >= request.WithdrawOrderFilterDto.StartDateSettedTime
                        && i.CreatedDate <= request.WithdrawOrderFilterDto.EndDateSettedTime)
            .Select(r => new WithdrawOrderDto
            {
                Id = r.Id,
                Description = r.Description,
                CreatedDate = r.CreatedDate,
                UpdatedDate = r.UpdatedDate,
                TransactionDate = r.TransactionDate,
                Status = r.Status,
                Amount = r.Amount,
                ReceiverFullName = r.ReceiverFullName,
                ReceiverAccountNumber = r.ReceiverAccountNumber,
                SenderAccount = new AccountDto()
                {
                    Id = r.Account.Id,
                    Name = r.Account.Name,
                    AccountNumber = r.Account.AccountNumber,
                    IsActive = r.Account.IsActive,
                    Bank = new BankDto()
                    {
                        Id = r.Account.Bank.Id,
                        Name = r.Account.Bank.Name,
                        IconUrl = r.Account.Bank.IconUrl
                    }
                }
            });
        
        var result = await query.ToPaginateAsync(request.PageRequest.Index, request.PageRequest.Size, 0, cancellationToken);
        var paginatedResult = new PaginatedResult<WithdrawOrderDto>
        {
            Items = result.Items!,
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