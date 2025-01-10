using Microsoft.AspNetCore.Http;
using Payhub.Application.Abstractions.Repositories;
using Payhub.Application.Abstractions.Services;
using Payhub.Application.Common.DTOs.Accounts;
using Payhub.Domain.Enums;
using Shared.Abstractions.Messaging;
using Shared.Utils.Responses;
using Shared.Utils.Security.Extensions;

namespace Payhub.Application.Features.Accounts.Queries.GetForSelect;

public class GetAccountForSelectQueryHandler : IQueryHandler<GetAccountForSelectQuery, IEnumerable<AccountSelectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionService _permissionService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public GetAccountForSelectQueryHandler(IUnitOfWork unitOfWork, IPermissionService permissionService, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _permissionService = permissionService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    
    public async Task<IEnumerable<AccountSelectDto>> Handle(GetAccountForSelectQuery request, CancellationToken cancellationToken)
    {
        var affiliatePermissions = await _permissionService.GetAffiliatePermissionsAsync();
        var role = _httpContextAccessor.HttpContext?.User.GetUserRole();
        bool isSystemUser = role?.Contains("System") == true || role?.Contains("Admin") == true;
        
        var accounts = await _unitOfWork.AccountRepository.GetAllWithSelectorAsync<AccountSelectDto>(
            predicate: i =>
                (request.AccountType == null || 
                 (request.AccountType == AccountType.Yatirim && 
                  (i.AccountType == AccountType.Yatirim || i.AccountType == AccountType.UstYatirim)) || 
                 i.AccountType == request.AccountType) &&
                (request.IsActive == null || i.IsActive == request.IsActive) &&
                i.IsDeleted == false &&
                (request.PaymentWayId == null || i.PaymentWayId == request.PaymentWayId) &&
                (!isSystemUser
                    ? affiliatePermissions.Contains(i.AffiliateId.Value)
                    : (!i.AffiliateId.HasValue || affiliatePermissions.Contains(i.AffiliateId.Value))),
            selector: r => new AccountSelectDto
            {
                Id = r.Id,
                Name = r.Name,
                AccountNumber = r.AccountNumber,
                BankName = r.Bank.Name,
                BankIconUrl = r.Bank.IconUrl
            },
            cancellationToken: cancellationToken);

        return accounts;
    }


}