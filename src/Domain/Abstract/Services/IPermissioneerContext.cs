namespace Dabitco.Permissioneer.Domain.Abstract.Services;

using Dabitco.Permissioneer.Domain.Entities;

public interface IPermissioneerContext
{
    Task<RoleEntity> AddRoleAsync(string roleName);
    Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
    Task<bool> IsGrantedAsync(string[] roleNames, Guid permissionId);
    Task<bool> AreGrantedAsync(string[] roleNames, Guid[] permissionIds);
    Task DeleteRoleAsync(Guid permissionId);
    Task<RoleEntity> GetRoleAsync(Guid roleId);
    Task<IEnumerable<RoleEntity>> ListRolesAsync();
    Task UpdateRoleAsync(Guid roleId, string newRoleName, bool isActive);
    Task UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
}
