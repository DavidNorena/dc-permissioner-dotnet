namespace Dabitco.Permissioneer.Domain.Abstract.Services;

using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Models;

public interface IPermissioneerContext
{
    Task<RoleModel> AddRoleAsync(RoleAddRequest request);
    Task AssignPermissionToRoleAsync(RolePermissionAssignRequest request);
    Task<bool> IsPermissionGrantedAsync(string[] roleNames, string permissionName);
    Task<bool> ArePermissionsGrantedAsync(string[] roleNames, string[] permissionNames);
    Task<bool> AreScopesGrantedAsync(string[] requiredScopes, string[] grantedScopes);
    Task DeleteRoleAsync(Guid roleId);
    Task<RoleEntity> GetRoleAsync(Guid roleId);
    Task<IEnumerable<RoleModel>> GetRolesAsync(string[] roleNames);
    Task<IEnumerable<PermissionModel>> ListPermissionsAsync();
    Task<IEnumerable<RoleModel>> ListRolesAsync();
    Task UpdateRoleAsync(Guid roleId, string newRoleName, bool isActive, string? newDescription = null);
    Task UnassignPermissionFromRoleAsync(RolePermissionAssignRequest request);

    Task<string> AddApiKeyAsync(ApiKeyAddRequest addRequest);
    Task<ApiKeyModel?> GetApiKeyAsync(string apiKey);
    Task<ApiKeyModel?> GetApiKeyAsync(Guid apiKeyId);
    Task RevokeApiKeyAsync(Guid apiKeyId);
    Task<IEnumerable<ApiKeyModel>> ListApiKeysAsync(string? ownerId = null);
}
