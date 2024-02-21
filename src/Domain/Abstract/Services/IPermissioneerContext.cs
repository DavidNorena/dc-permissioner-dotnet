namespace Dabitco.Permissioneer.Domain.Abstract.Services;

using Dabitco.Permissioneer.Domain.Entities;

public interface IPermissioneerContext
{
    Task<RoleEntity> AddRoleAsync(string roleName);
    Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
    Task<bool> CheckRolesPermissionAsync(string[] roleNames, string permissionName);
    Task<bool> CheckRolesPermissionsAsync(string[] roleNames, string[] permissionNames);
    Task<bool> DeleteRoleAsync(Guid permissionId);
    Task<RoleEntity> GetRoleAsync(Guid roleId);
    Task<IEnumerable<RoleEntity>> ListRolesAsync();
    Task<bool> RenameRoleAsync(Guid roleId, string newRoleName);
    Task<bool> UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
}
