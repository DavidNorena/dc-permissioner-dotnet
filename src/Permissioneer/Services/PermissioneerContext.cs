namespace Dabitco.Permissioneer.Services;

using Dabitco.Permissioneer.Domain.Abstract.Services;
using Dabitco.Permissioneer.Domain.Entities;

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

    public async Task<bool> IsGrantedAsync(string[] roleNames, Guid permissionId)
    {
        return await storage.IsGrantedAsync(roleNames, permissionId);
    }

    public async Task<bool> AreGrantedAsync(string[] roleNames, Guid[] permissionIds)
    {
        return await storage.AreGrantedAsync(roleNames, permissionIds);
    }

    public async Task DeleteRoleAsync(Guid roleId)
    {
        await storage.DeleteRoleAsync(roleId);
    }

    public async Task<RoleEntity> GetRoleAsync(Guid roleId)
    {
        return await storage.GetRoleAsync(roleId) ?? throw new Exception($"Role with Id {roleId} not found");
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
