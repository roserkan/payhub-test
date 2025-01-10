using Microsoft.AspNetCore.Http;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Common.DTOs.Deposits;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;
using Shared.Utils.Pagination;
using Shared.Utils.Responses;
using Shared.Utils.Security.Extensions;

namespace Payhub.Application.Features.Deposits.Queries.GetList;

public sealed class GetListDepositsQueryHandler : IQueryHandler<GetListDepositsQuery, PaginatedResult<DepositDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public GetListDepositsQueryHandler(IUnitOfWork unitOfWork, IPermissionService permissionService, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<PaginatedResult<DepositDto>> Handle(GetListDepositsQuery request, CancellationToken cancellationToken)
    {
        var dto = request.DepositFilterDto;
        if (!string.IsNullOrEmpty(dto.SearchValue))
        {
            dto.StartDateSettedTime = DateTime.MinValue;
            dto.EndDateSettedTime = DateTime.MaxValue;
            dto.SiteId = null;
            dto.AccountId = null;
        }

        var sitePermissions = await _permissionService.GetSitePermissionsAsync();
        var affiliatePermissions = await _permissionService.GetAffiliatePermissionsAsync();
        var role = _httpContextAccessor.HttpContext?.User.GetUserRole();
        bool isSystemUser = role?.Contains("System") == true || role?.Contains("Admin") == true;
        bool isAffiliateUser = role?.Contains("Affiliate") == true;
        
        var depositsQuery = _unitOfWork.DepositRepository.Query(); 
        var customersQuery = _unitOfWork.CustomerRepository.Query();

       var query = (from deposit in depositsQuery
             join customer in customersQuery
                 on deposit.PanelCustomerId equals customer.PanelCustomerId
             where
                 // deposit.CreatedDate >= dto.StartDateSettedTime &&
                 // deposit.CreatedDate <= dto.EndDateSettedTime &&
                 (
                     (deposit.Status == DepositStatus.Confirmed || deposit.Status == DepositStatus.Declined)
                         ? (deposit.TransactionDate >= dto.StartDateSettedTime && deposit.TransactionDate <= dto.EndDateSettedTime)
                         : (deposit.CreatedDate >= dto.StartDateSettedTime && deposit.CreatedDate <= dto.EndDateSettedTime)
                 ) &&
                 deposit.PaymentWayId == dto.PaymentWayId &&
                 (!dto.SiteId.HasValue || deposit.SiteId == dto.SiteId.Value) &&
                 (!isAffiliateUser || sitePermissions.Contains(deposit.SiteId)) &&
                 (!isSystemUser
                     ? affiliatePermissions.Contains(deposit.AffiliateId.Value)
                     : (!deposit.AffiliateId.HasValue || affiliatePermissions.Contains(deposit.AffiliateId.Value))) &&
                 (!dto.AccountId.HasValue || deposit.AccountId == dto.AccountId.Value) &&
                 (
                     dto.Status == null ||
                     (dto.Status == DepositStatus.PendingConfirmation &&
                      (deposit.Status == DepositStatus.PendingConfirmation ||
                       deposit.Status == DepositStatus.PendingDeposit)) ||
                     (dto.Status != DepositStatus.PendingConfirmation && deposit.Status == dto.Status)
                 ) &&
                 (string.IsNullOrEmpty(dto.SearchValue) ||
                  (!string.IsNullOrEmpty(customer.FullName) &&
                   customer.FullName.ToLower().Contains(dto.SearchValue.ToLower())) ||
                  (!string.IsNullOrEmpty(customer.SiteCustomerId) &&
                   customer.SiteCustomerId.ToLower().Contains(dto.SearchValue.ToLower())) ||
                  deposit.ProcessId.ToLower().Contains(dto.SearchValue.ToLower()))
             select new DepositDto
             {
                 Id = deposit.Id,
                 ProcessId = deposit.ProcessId,
                 Amount = deposit.Amount,
                 PayedAmount = deposit.PayedAmount,
                 Status = deposit.Status,
                 InfraConfirmed = deposit.InfraConfirmed,
                 DynamicAccountName = deposit.DynamicAccountName,
                 DynamicAccountNumber = deposit.DynamicAccountNumber,
                 SiteName = !isAffiliateUser ? deposit.Site.Name : null,
                 PaymentWayId = deposit.PaymentWayId,
                 SiteId = deposit.SiteId,
                 CustomerRequestName = deposit.CustomerFullName,
                 CustomerFullName = customer.FullName,
                 CustomerUserName = customer.Username,
                 CustomerId = customer.Id,
                 SiteCustomerId = customer.SiteCustomerId,
                 PanelCustomerId = deposit.PanelCustomerId,
                 BankAccountName = deposit.Account != null ? deposit.Account.Name : null,
                 AccountNumber = deposit.Account != null ? deposit.Account.AccountNumber : null,
                 AccountId = deposit.Account != null ? deposit.AccountId : null,
                 BankIconUrl = deposit.Account != null ? deposit.Account.Bank.IconUrl : null,
                 BankName = deposit.Account != null ? deposit.Account.Bank.Name : null,
                 CreatedDate = deposit.CreatedDate,
                 TransactionDate = deposit.TransactionDate,
                 ProcessOwnerName = deposit.CreatedUser != null ? deposit.CreatedUser.Name : null,
                 LastProcessOwnerName = deposit.UpdatedUser != null ? deposit.UpdatedUser.Name : null,
                 AutoUpdatedName = deposit.AutoUpdatedName,
                 InfraCallbackType = deposit.InfraCallbackType,
                 AffiliateName = deposit.Affiliate != null ? deposit.Affiliate.Name : null
             });

       if (dto.Status == DepositStatus.PendingConfirmation || dto.Status == DepositStatus.PendingDeposit)
           query = query.OrderBy(x => x.CreatedDate);// Ascending
       else if (dto.Status == DepositStatus.Confirmed || dto.Status == DepositStatus.Declined)
           query = query.OrderByDescending(x => x.TransactionDate); // Descending
       else 
           query = query.OrderByDescending(x => x.TransactionDate); // Descending
       
       // query = dto.Status == DepositStatus.PendingConfirmation
       //     ? query.OrderBy(x => x.CreatedDate) // Ascending
       //     : query.OrderByDescending(x => x.CreatedDate); // Descending
       
        var result = await query.ToPaginateAsync(request.PageRequest.Index, request.PageRequest.Size, 0, cancellationToken);
        var paginatedResult = new PaginatedResult<DepositDto>
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