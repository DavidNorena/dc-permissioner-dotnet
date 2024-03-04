namespace Dabitco.Permissioneer.Services;

using Dabitco.Permissioneer.Domain.Abstract.Services;
using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Enums;

public class PermissioneerContext(IPermissioneerStorage storage) : IPermissioneerContext
{
    public async Task<RoleEntity> AddRoleAsync(string roleName)
    {
        return await storage.AddRoleAsync(roleName);
    }

    public async Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        await storage.AssignPermissionToRoleAsync(roleId, permissionId, isAllowed);
    }

    public async Task<bool> IsPermissionGrantedAsync(string[] roleNames, string permissionName)
    {
        return await storage.IsPermissionGrantedAsync(roleNames, permissionName);
    }

    public async Task<bool> ArePermissionsGrantedAsync(string[] roleNames, string[] permissionNames, PermissionsOperatorType operatorType = PermissionsOperatorType.And)
    {
        return await storage.ArePermissionsGrantedAsync(roleNames, permissionNames, operatorType);
    }

    public async Task<bool> AreScopesGrantedAsync(string[] requiredScopes, string[] grantedScopes, PermissionsOperatorType operatorType = PermissionsOperatorType.And)
    {
        return await storage.AreScopesGrantedAsync(requiredScopes, grantedScopes, operatorType);
    }

    public async Task DeleteRoleAsync(Guid roleId)
    {
        await storage.DeleteRoleAsync(roleId);
    }

    public async Task<RoleEntity> GetRoleAsync(Guid roleId)
    {
        return await storage.GetRoleAsync(roleId) ?? throw new Exception($"Role with Id {roleId} not found");
    }

    public async Task<IEnumerable<PermissionEntity>> ListPermissionsAsync()
    {
        return await storage.ListPermissionsAsync();
    }

    public async Task<IEnumerable<RoleEntity>> ListRolesAsync()
    {
        return await storage.ListRolesAsync();
    }

    public async Task UpdateRoleAsync(Guid roleId, string newRoleName, bool isActive)
    {
        var role = await GetRoleAsync(roleId);
        role.Name = newRoleName;
        role.IsActive = isActive;

        await storage.UpdateRoleAsync(role);
    }

    public async Task UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        await storage.UnassignPermissionFromRoleAsync(roleId, permissionId, isAllowed);
    }
}
