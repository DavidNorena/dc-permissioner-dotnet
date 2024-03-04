namespace Dabitco.Permissioneer.EntityFramework.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Dabitco.Permissioneer.Domain.Abstract.Services;
using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

public class DbPermissioneerStorage(PermissioneerDbContext dbContext) : IPermissioneerStorage
{
    public async Task<RoleEntity> AddRoleAsync(string roleName)
    {
        var existingRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        if (existingRole != null)
        {
            throw new InvalidOperationException($"Role with name {roleName} already exists");
        }

        var newRole = new RoleEntity
        {
            Id = Guid.NewGuid(),
            Name = roleName,
            IsActive = true,
        };

        await dbContext.Roles.AddAsync(newRole);
        await dbContext.SaveChangesAsync();

        return newRole;
    }

    public async Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        var role = await GetRoleAsync(roleId)
            ?? throw new InvalidOperationException($"Role with id {roleId} does not exist");

        if (role.IsSystem)
        {
            throw new InvalidOperationException("System roles cannot be modified");
        }

        var permission = await dbContext.Permissions.FirstOrDefaultAsync(p => p.Id == permissionId)
            ?? throw new InvalidOperationException($"Permission with id {permissionId} does not exist");

        if (role.RolePermissions.Any(rp => rp.PermissionId == permission.Id))
        {
            throw new InvalidOperationException($"Role with id {roleId} already has permission with id {permissionId}");
        }

        role.RolePermissions.Add(new RolePermissionEntity
        {
            RoleId = roleId,
            PermissionId = permissionId,
            IsAllowed = isAllowed,
            IsSystem = false,
        });

        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsPermissionGrantedAsync(string[] roleNames, string permissionName)
    {
        var permission = await dbContext.Permissions
            .Where(p => p.Name == permissionName)
            .SingleOrDefaultAsync();

        if (permission == null)
        {
            return false;
        }

        var rolesWithPermission = await dbContext.Roles
            .Where(r => roleNames.Contains(r.Name))
            .SelectMany(r => r.RolePermissions)
            .Where(rp => rp.PermissionId == permission.Id)
            .ToListAsync();

        var deniedPermissionsSet = rolesWithPermission
            .Where(rp => !rp.IsAllowed)
            .Select(rp => rp.PermissionId)
            .ToHashSet();

        if (deniedPermissionsSet.Contains(permission.Id))
        {
            return false;
        }

        var allowedPermissionsSet = rolesWithPermission
            .Where(rp => rp.IsAllowed)
            .Select(rp => rp.PermissionId)
            .ToHashSet();

        return allowedPermissionsSet.Contains(permission.Id);
    }

    public async Task<bool> ArePermissionsGrantedAsync(string[] roleNames, string[] permissionNames, PermissionsOperatorType operatorType = PermissionsOperatorType.And)
    {
        var permissionIds = await dbContext.Permissions
            .Where(p => permissionNames.Contains(p.Name))
            .Select(p => p.Id)
            .ToListAsync();

        if (permissionIds.Count == 0)
        {
            return false;
        }

        var permissionIdSet = permissionIds.ToHashSet();

        var rolesWithPermissions = await dbContext.Roles
            .Where(r => roleNames.Contains(r.Name))
            .SelectMany(r => r.RolePermissions)
            .ToListAsync();

        if (operatorType == PermissionsOperatorType.And)
        {
            var deniedPermissionsSet = rolesWithPermissions
                .Where(rp => !rp.IsAllowed && permissionIdSet.Contains(rp.PermissionId))
                .Select(rp => rp.PermissionId)
                .ToHashSet();

            if (deniedPermissionsSet.Count != 0)
            {
                return false;
            }

            var allowedPermissionsSet = rolesWithPermissions
                .Where(rp => rp.IsAllowed && permissionIdSet.Contains(rp.PermissionId))
                .Select(rp => rp.PermissionId)
                .ToHashSet();

            return permissionIdSet.IsSubsetOf(allowedPermissionsSet);
        }
        else if (operatorType == PermissionsOperatorType.Or)
        {
            return rolesWithPermissions.Any(rp => rp.IsAllowed && permissionIdSet.Contains(rp.PermissionId));
        }

        return false;
    }

    public Task<bool> AreScopesGrantedAsync(string[] requiredScopes, string[] grantedScopes, PermissionsOperatorType operatorType = PermissionsOperatorType.And)
    {
        var result = operatorType switch
        {
            PermissionsOperatorType.And => requiredScopes.All(requiredScope => grantedScopes.Contains(requiredScope)),
            PermissionsOperatorType.Or => requiredScopes.Any(requiredScope => grantedScopes.Contains(requiredScope)),
            _ => throw new NotImplementedException($"Unsupported operator type: {operatorType}"),
        };

        return Task.FromResult(result);
    }

    public async Task DeleteRoleAsync(Guid roleId)
    {
        var role = await GetRoleAsync(roleId)
            ?? throw new InvalidOperationException($"Role with id {roleId} does not exist");

        if (role.IsSystem)
        {
            throw new InvalidOperationException("System roles cannot be deleted");
        }

        dbContext.Roles.Remove(role);

        await dbContext.SaveChangesAsync();
    }

    public async Task<RoleEntity?> GetRoleAsync(Guid roleId)
    {
        return await dbContext.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == roleId);
    }

    public async Task<IEnumerable<PermissionEntity>> ListPermissionsAsync()
    {
        return await dbContext.Permissions.ToListAsync();
    }

    public async Task<IEnumerable<RoleEntity>> ListRolesAsync()
    {
        return await dbContext.Roles
            .Include(r => r.RolePermissions)
            .ToListAsync();
    }

    public async Task UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        var role = await GetRoleAsync(roleId)
            ?? throw new InvalidOperationException($"Role with id {roleId} does not exist");

        if (role.IsSystem)
        {
            throw new InvalidOperationException("System roles cannot be modified");
        }

        var permission = await dbContext.Permissions.FirstOrDefaultAsync(p => p.Id == permissionId)
            ?? throw new InvalidOperationException($"Permission with id {permissionId} does not exist");

        var rolePermission = role.RolePermissions.FirstOrDefault(rp => rp.PermissionId == permissionId);
        if (rolePermission == null)
        {
            throw new InvalidOperationException($"Role with id {roleId} does not have permission with id {permissionId}");
        }

        role.RolePermissions.Remove(rolePermission);

        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateRoleAsync(RoleEntity role)
    {
        var existingRole = await dbContext.Roles.FindAsync(role.Id)
            ?? throw new InvalidOperationException($"Role with id {role.Id} does not exist");

        if (existingRole.IsSystem)
        {
            throw new InvalidOperationException("System roles cannot be modified");
        }

        if (await dbContext.Roles.AnyAsync(r => r.Name == role.Name && r.Id != role.Id))
        {
            throw new InvalidOperationException($"Role with name {role.Name} already exists");
        }

        existingRole.Name = role.Name;
        existingRole.IsActive = role.IsActive;

        dbContext.Entry(existingRole).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
    }
}
