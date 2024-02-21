namespace Dabitco.Permissioneer.Domain.Abstract.Services;

using Dabitco.Permissioneer.Domain.Entities;

public interface IPermissioneerStorage
{
    Task<RoleEntity> AddRoleAsync(string roleName);
    Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
    Task<bool> CheckRolesPermissionAsync(string[] roleNames, string permissionName);
    Task<bool> CheckRolesPermissionsAsync(string[] roleNames, string[] permissionNames);
    Task<bool> DeleteRoleAsync(Guid roleId);
    Task<RoleEntity?> GetRoleAsync(Guid roleId);
    Task<IEnumerable<RoleEntity>> ListRolesAsync();
    Task<bool> UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
    Task<bool> UpdateRoleAsync(RoleEntity role);
}
