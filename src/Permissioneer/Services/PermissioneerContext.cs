namespace Dabitco.Permissioneer.Services;

using Dabitco.Permissioneer.Domain.Abstract.Services;
using Dabitco.Permissioneer.Domain.Entities;

public class PermissioneerContext(IPermissioneerStorage storage) : IPermissioneerContext
{
    public async Task<RoleEntity> AddRoleAsync(string roleName)
    {
        return await storage.AddRoleAsync(roleName);
    }

    public async Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        return await storage.AssignPermissionToRoleAsync(roleId, permissionId, isAllowed);
    }

    public async Task<bool> CheckRolesPermissionAsync(string[] roleNames, string permissionName)
    {
        return await storage.CheckRolesPermissionAsync(roleNames, permissionName);
    }

    public async Task<bool> CheckRolesPermissionsAsync(string[] roleNames, string[] permissionNames)
    {
        return await storage.CheckRolesPermissionsAsync(roleNames, permissionNames);
    }

    public async Task<bool> DeleteRoleAsync(Guid roleId)
    {
        return await storage.DeleteRoleAsync(roleId);
    }

    public async Task<RoleEntity> GetRoleAsync(Guid roleId)
    {
        return await storage.GetRoleAsync(roleId) ?? throw new Exception($"Role with Id {roleId} not found");
    }

    public async Task<IEnumerable<RoleEntity>> ListRolesAsync()
    {
        return await storage.ListRolesAsync();
    }

    public async Task<bool> RenameRoleAsync(Guid roleId, string newRoleName)
    {
        var role = await GetRoleAsync(roleId);
        role.Name = newRoleName;

        return await storage.UpdateRoleAsync(role);
    }

    public async Task<bool> UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        return await storage.UnassignPermissionFromRoleAsync(roleId, permissionId, isAllowed);
    }
}
