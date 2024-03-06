namespace Dabitco.Permissioneer.Domain.Abstract.Services;

using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Models;

public interface IPermissioneerContext
{
    Task<RoleModel> AddRoleAsync(RoleAddRequest roleAddRequest);
    Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
    Task AssignPermissionToRoleAsync(Guid roleId, Guid[] permissionId, bool isAllowed = true);
    Task<bool> IsPermissionGrantedAsync(string[] roleNames, string permissionName);
    Task<bool> ArePermissionsGrantedAsync(string[] roleNames, string[] permissionNames);
    Task<bool> AreScopesGrantedAsync(string[] requiredScopes, string[] grantedScopes);
    Task DeleteRoleAsync(Guid permissionId);
    Task<RoleEntity> GetRoleAsync(Guid roleId);
    Task<IEnumerable<PermissionModel>> ListPermissionsAsync();
    Task<IEnumerable<RoleModel>> ListRolesAsync();
    Task UpdateRoleAsync(Guid roleId, string newRoleName, bool isActive, string? newDescription = null);
    Task UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
    Task UnassignPermissionsFromRoleAsync(Guid roleId, Guid[] permissionId, bool isAllowed = true);

    Task<string> AddApiKeyAsync(ApiKeyAddRequest addRequest);
    Task<ApiKeyModel?> GetApiKeyAsync(string apiKey);
    Task RevokeApiKeyAsync(Guid apiKeyId);
    Task<IEnumerable<ApiKeyModel>> ListApiKeysAsync(string? ownerId = null);
}
