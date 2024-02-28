namespace Dabitco.Permissioneer.EntityFramework.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Dabitco.Permissioneer.Domain.Abstract.Services;
using Dabitco.Permissioneer.Domain.Entities;
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

    public async Task<bool> IsGrantedAsync(string[] roleNames, Guid permissionId)
    {
        var rolesWithPermission = await dbContext.Roles
            .Where(r => roleNames.Contains(r.Name))
            .SelectMany(r => r.RolePermissions)
            .Where(rp => rp.PermissionId == permissionId)
            .ToListAsync();

        var deniedPermissionsSet = rolesWithPermission
            .Where(rp => !rp.IsAllowed)
            .Select(rp => rp.PermissionId)
            .ToHashSet();

        if (deniedPermissionsSet.Contains(permissionId))
        {
            return false;
        }

        var allowedPermissionsSet = rolesWithPermission
            .Where(rp => rp.IsAllowed)
            .Select(rp => rp.PermissionId)
            .ToHashSet();

        return allowedPermissionsSet.Contains(permissionId);
    }

    public async Task<bool> AreGrantedAsync(string[] roleNames, Guid[] permissionIds)
    {
        var permissionIdSet = permissionIds.ToHashSet();

        var rolesWithPermissions = await dbContext.Roles
            .Where(r => roleNames.Contains(r.Name))
            .SelectMany(r => r.RolePermissions)
            .ToListAsync();

        var deniedPermissionsSet = rolesWithPermissions
            .Where(rp => !rp.IsAllowed)
            .Select(rp => rp.PermissionId)
            .ToHashSet();

        if (deniedPermissionsSet.Overlaps(permissionIdSet))
        {
            return false;
        }

        var allowedPermissionsSet = rolesWithPermissions
            .Where(rp => rp.IsAllowed)
            .Select(rp => rp.PermissionId)
            .ToHashSet();

        return permissionIdSet.IsSubsetOf(allowedPermissionsSet);
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
