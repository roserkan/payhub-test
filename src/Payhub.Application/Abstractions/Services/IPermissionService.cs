namespace Payhub.Application.Abstractions.Services;

public interface IPermissionService
{
    Task<IEnumerable<int>> GetSitePermissionsAsync();
    Task<IEnumerable<int>> GetAffiliatePermissionsAsync();
}