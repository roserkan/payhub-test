using Microsoft.AspNetCore.Http;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Common.DTOs.Withdraws;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;
using Shared.Utils.Pagination;
using Shared.Utils.Responses;
using Shared.Utils.Security.Extensions;

namespace Payhub.Application.Features.Withdraws.Queries.GetList;

public sealed class GetListWithdrawsQueryHandler : IQueryHandler<GetListWithdrawsQuery, PaginatedResult<WithdrawDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public GetListWithdrawsQueryHandler(IUnitOfWork unitOfWork, IPermissionService permissionService, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<PaginatedResult<WithdrawDto>> Handle(GetListWithdrawsQuery request, CancellationToken cancellationToken)
    {
        var dto = request.WithdrawFilterDto;
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
        
        var withdrawsQuery = _unitOfWork.WithdrawRepository.Query(); 
        var customersQuery = _unitOfWork.CustomerRepository.Query(); 

       var query = (from withdraw in withdrawsQuery
             join customer in customersQuery
                 on withdraw.PanelCustomerId equals customer.PanelCustomerId
             where
                 // withdraw.CreatedDate >= dto.StartDateSettedTime &&
                 // withdraw.CreatedDate <= dto.EndDateSettedTime &&
                 (
                     (withdraw.Status == WithdrawStatus.Confirmed || withdraw.Status == WithdrawStatus.Declined)
                         ? (withdraw.TransactionDate >= dto.StartDateSettedTime && withdraw.TransactionDate <= dto.EndDateSettedTime)
                         : (withdraw.CreatedDate >= dto.StartDateSettedTime && withdraw.CreatedDate <= dto.EndDateSettedTime)
                 ) &&
                 withdraw.PaymentWayId == dto.PaymentWayId &&
                 (!dto.SiteId.HasValue || withdraw.SiteId == dto.SiteId.Value) &&
                 (!isAffiliateUser || sitePermissions.Contains(withdraw.SiteId)) &&
                 (!isSystemUser
                     ? affiliatePermissions.Contains(withdraw.AffiliateId.Value)
                     : (!withdraw.AffiliateId.HasValue || affiliatePermissions.Contains(withdraw.AffiliateId.Value))) &&
                 (!dto.AccountId.HasValue || withdraw.AccountId == dto.AccountId.Value) &&
                 (
                     dto.Status == null || (withdraw.Status == dto.Status)
                 ) &&
                 (string.IsNullOrEmpty(dto.SearchValue) ||
                  (!string.IsNullOrEmpty(customer.FullName) && customer.FullName.ToLower().Contains(dto.SearchValue.ToLower())) ||
                  (!string.IsNullOrEmpty(customer.SiteCustomerId) && customer.SiteCustomerId.ToLower().Contains(dto.SearchValue.ToLower())) ||
                  withdraw.ProcessId.ToLower().Contains(dto.SearchValue.ToLower()))
             select new WithdrawDto
             {
                 Id = withdraw.Id,
                 ProcessId = withdraw.ProcessId,
                 SiteName = !isAffiliateUser ? withdraw.Site.Name : null,
                 CustomerPanelId = withdraw.PanelCustomerId,
                 CustomerFullName = customer.FullName,
                 CustomerSiteId = customer.SiteCustomerId,
                 CustomerAccountNumber = withdraw.CustomerAccountNumber,
                 Amount = withdraw.Amount,
                 PayedAmount = withdraw.PayedAmount,
                 Status = withdraw.Status,
                 InfraConfirmed = withdraw.InfraConfirmed,
                 AccountId = withdraw.Account != null ? withdraw.AccountId : null,
                 BankIconUrl = withdraw.Account != null ? withdraw.Account.Bank.IconUrl : null,
                 BankName = withdraw.Account != null ? withdraw.Account.Bank.Name : null,
                 AccountName = withdraw.Account != null ? withdraw.Account.Name : null,
                 AccountNumber = withdraw.AccountId != null ? withdraw.Account!.AccountNumber : null,
                 CreatedDate = withdraw.CreatedDate,
                 TransactionDate = withdraw.TransactionDate,
                 ProcessOwnerName = withdraw.CreatedUser != null ? withdraw.CreatedUser.Name : null,
                 LastProcessOwnerName = withdraw.UpdatedUser != null ? withdraw.UpdatedUser.Name : null,
                 AutoUpdatedName = withdraw.AutoUpdatedName,
                 PaymentWayId = withdraw.PaymentWayId,
                 InfraCallbackType = withdraw.InfraCallbackType,
                 IsIbanBlacklisted = withdraw.IsIbanBlacklisted,
                 AffiliateName = withdraw.Affiliate != null ? withdraw.Affiliate.Name : null
             });
       
       if (dto.Status == WithdrawStatus.PendingWithdraw)
           query = query.OrderBy(x => x.CreatedDate);// Ascending
       else if (dto.Status == WithdrawStatus.Confirmed || dto.Status == WithdrawStatus.Declined)
           query = query.OrderByDescending(x => x.TransactionDate); // Descending
       else 
           query = query.OrderByDescending(x => x.TransactionDate); // Descending
       
       // query = dto.Status == WithdrawStatus.PendingWithdraw
       //     ? query.OrderBy(x => x.CreatedDate) // Ascending
       //     : query.OrderByDescending(x => x.CreatedDate); // Descending


        var result = await query.ToPaginateAsync(request.PageRequest.Index, request.PageRequest.Size, 0, cancellationToken);
        var paginatedResult = new PaginatedResult<WithdrawDto>
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