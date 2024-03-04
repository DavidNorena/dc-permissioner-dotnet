namespace Dabitco.Permissioneer.Domain.Abstract.Services;

using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Enums;

public interface IPermissioneerContext
{
    Task<RoleEntity> AddRoleAsync(string roleName);
    Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
    Task<bool> IsPermissionGrantedAsync(string[] roleNames, string permissionName);
    Task<bool> ArePermissionsGrantedAsync(string[] roleNames, string[] permissionNames, PermissionsOperatorType operatorType = PermissionsOperatorType.And);
    Task<bool> AreScopesGrantedAsync(string[] requiredScopes, string[] grantedScopes, PermissionsOperatorType operatorType = PermissionsOperatorType.And);
    Task DeleteRoleAsync(Guid permissionId);
    Task<RoleEntity> GetRoleAsync(Guid roleId);
    Task<IEnumerable<PermissionEntity>> ListPermissionsAsync();
    Task<IEnumerable<RoleEntity>> ListRolesAsync();
    Task UpdateRoleAsync(Guid roleId, string newRoleName, bool isActive);
    Task UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true);
}
