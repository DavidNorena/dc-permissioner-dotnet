namespace Dabitco.Permissioneer.Domain.Abstract.Storage;

using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Models;

public abstract class PermissionsStorageBase
{
    public abstract Task<RoleModel> AddRoleAsync(RoleAddRequest roleAddRequest);
    public abstract Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
    public abstract Task<bool> IsPermissionGrantedAsync(string[] roleNames, string permissionName);
    public abstract Task<bool> ArePermissionsGrantedAsync(string[] roleNames, string[] permissionNames);
    public abstract Task DeleteRoleAsync(Guid roleId);
    public abstract Task<IEnumerable<PermissionEntity>> GetPermissionsAsync(Guid[] permissionsIds);
    public abstract Task<RoleEntity?> GetRoleAsync(Guid roleId);
    public abstract Task<IEnumerable<PermissionModel>> ListPermissionsAsync();
    public abstract Task<IEnumerable<RoleModel>> ListRolesAsync();
    public abstract Task UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
    public abstract Task UpdateRoleAsync(RoleEntity role);

    public virtual async Task AssignPermissionsToRoleAsync(Guid roleId, Guid[] permissionId, bool isAllowed = true)
    {
        foreach (var id in permissionId)
        {
            await AssignPermissionToRoleAsync(roleId, id, isAllowed);
        }
    }

    public virtual Task<bool> AreScopesGrantedAsync(string[] requiredScopes, string[] grantedScopes)
    {
        var isAnyScopeGranted = requiredScopes.Any(requiredScope => grantedScopes.Contains(requiredScope, StringComparer.OrdinalIgnoreCase));

        return Task.FromResult(isAnyScopeGranted);
    }

    public virtual async Task UnassignPermissionsFromRoleAsync(Guid roleId, Guid[] permissionId, bool isAllowed = true)
    {
        foreach (var id in permissionId)
        {
            await UnassignPermissionFromRoleAsync(roleId, id, isAllowed);
        }
    }
}
