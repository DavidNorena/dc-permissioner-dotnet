namespace Dabitco.Permissioneer.Services;

using Dabitco.Permissioneer.Domain.Abstract.Services;
using Dabitco.Permissioneer.Domain.Abstract.Storage;
using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Models;

public class PermissioneerContext(PermissionsStorageBase permissionsStorage, ApiKeysStorageBase apiKeysStorage) : IPermissioneerContext
{
    public async Task<RoleModel> AddRoleAsync(RoleAddRequest roleAddRequest)
    {
        return await permissionsStorage.AddRoleAsync(roleAddRequest);
    }

    public async Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        await permissionsStorage.AssignPermissionToRoleAsync(roleId, permissionId, isAllowed);
    }

    public async Task AssignPermissionToRoleAsync(Guid roleId, Guid[] permissionId, bool isAllowed = true)
    {
        await permissionsStorage.AssignPermissionsToRoleAsync(roleId, permissionId, isAllowed);
    }

    public async Task<bool> IsPermissionGrantedAsync(string[] roleNames, string permissionName)
    {
        return await permissionsStorage.IsPermissionGrantedAsync(roleNames, permissionName);
    }

    public async Task<bool> ArePermissionsGrantedAsync(string[] roleNames, string[] permissionNames)
    {
        return await permissionsStorage.ArePermissionsGrantedAsync(roleNames, permissionNames);
    }

    public async Task<bool> AreScopesGrantedAsync(string[] requiredScopes, string[] grantedScopes)
    {
        return await permissionsStorage.AreScopesGrantedAsync(requiredScopes, grantedScopes);
    }

    public async Task DeleteRoleAsync(Guid roleId)
    {
        await permissionsStorage.DeleteRoleAsync(roleId);
    }

    public async Task<RoleEntity> GetRoleAsync(Guid roleId)
    {
        return await permissionsStorage.GetRoleAsync(roleId) ?? throw new Exception($"Role with Id {roleId} not found");
    }

    public async Task<IEnumerable<PermissionModel>> ListPermissionsAsync()
    {
        return await permissionsStorage.ListPermissionsAsync();
    }

    public async Task<IEnumerable<RoleModel>> ListRolesAsync()
    {
        return await permissionsStorage.ListRolesAsync();
    }

    public async Task UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        await permissionsStorage.UnassignPermissionFromRoleAsync(roleId, permissionId, isAllowed);
    }

    public async Task UnassignPermissionsFromRoleAsync(Guid roleId, Guid[] permissionId, bool isAllowed = true)
    {
        await permissionsStorage.UnassignPermissionsFromRoleAsync(roleId, permissionId, isAllowed);
    }

    public async Task UpdateRoleAsync(Guid roleId, string newRoleName, bool isActive, string? newDescription = null)
    {
        var role = await GetRoleAsync(roleId);
        role.Name = newRoleName;
        role.IsActive = isActive;
        if (newDescription != null)
        {
            role.Description = newDescription;
        }

        await permissionsStorage.UpdateRoleAsync(role);
    }

    public async Task<string> AddApiKeyAsync(ApiKeyAddRequest addRequest)
    {
        return await apiKeysStorage.AddApiKeyAsync(addRequest);
    }

    public async Task<ApiKeyModel?> GetApiKeyAsync(string apiKey)
    {
        return await apiKeysStorage.GetApiKeyAsync(apiKey);
    }

    public async Task RevokeApiKeyAsync(Guid apiKeyId)
    {
        await apiKeysStorage.RevokeApiKeyAsync(apiKeyId);
    }

    public async Task<IEnumerable<ApiKeyModel>> ListApiKeysAsync(string? ownerId = null)
    {
        return await apiKeysStorage.ListApiKeysAsync(ownerId);
    }
}
