namespace Dabitco.Permissioneer.Domain.Abstract.Services;

using Dabitco.Permissioneer.Domain.Entities;

public interface IPermissioneerStorage
{
    Task<RoleEntity> AddRoleAsync(string roleName);
    Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
    Task<bool> IsGrantedAsync(string[] roleNames, Guid permissionId);
    Task<bool> AreGrantedAsync(string[] roleNames, Guid[] permissionIds);
    Task DeleteRoleAsync(Guid roleId);
    Task<RoleEntity?> GetRoleAsync(Guid roleId);
    Task<IEnumerable<RoleEntity>> ListRolesAsync();
    Task UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
    Task UpdateRoleAsync(RoleEntity role);
}
