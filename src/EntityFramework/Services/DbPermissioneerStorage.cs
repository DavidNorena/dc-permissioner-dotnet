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

    public async Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        var role = await GetRoleAsync(roleId);
        if (role == null)
        {
            return false;
        }

        var permission = await dbContext.Permissions.FirstOrDefaultAsync(p => p.Id == permissionId);
        if (permission == null)
        {
            return false;
        }

        if (role.PermissionsAllowed.Any(p => p.Id == permission.Id) || role.PermissionsDenied.Any(p => p.Id == permission.Id))
        {
            return false;
        }

        if (isAllowed)
        {
            role.PermissionsAllowed.Add(permission);
        }
        else
        {
            role.PermissionsDenied.Add(permission);
        }

        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CheckRolesPermissionAsync(string[] roleNames, string permissionName)
    {
        var roles = await dbContext.Roles
            .Where(r => roleNames.Contains(r.Name))
            .Select(r => new
            {
                Allowed = r.PermissionsAllowed.Any(pa => pa.Name == permissionName),
                Denied = r.PermissionsDenied.Any(pd => pd.Name == permissionName)
            })
            .ToListAsync();

        if (roles.Any(r => r.Denied))
        {
            return false;
        }

        return roles.Any(r => r.Allowed);
    }

    public async Task<bool> CheckRolesPermissionsAsync(string[] roleNames, string[] permissionNames)
    {
        var roles = await dbContext.Roles
            .Where(r => roleNames.Contains(r.Name))
            .Select(r => new
            {
                AllowedPermissions = r.PermissionsAllowed.Select(pa => pa.Name).ToList(),
                DeniedPermissions = r.PermissionsDenied.Select(pd => pd.Name).ToList()
            })
            .ToListAsync();

        foreach (var permissionName in permissionNames)
        {
            if (roles.Any(role => role.DeniedPermissions.Contains(permissionName)))
            {
                return false;
            }

            if (!roles.Any(role => role.AllowedPermissions.Contains(permissionName)))
            {
                return false;
            }
        }

        return true;
    }

    public async Task<bool> DeleteRoleAsync(Guid roleId)
    {
        var role = await GetRoleAsync(roleId);
        if (role == null)
        {
            return false;
        }

        dbContext.Roles.Remove(role);

        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<RoleEntity?> GetRoleAsync(Guid roleId)
    {
        return await dbContext.Roles
            .Include(r => r.PermissionsAllowed)
            .Include(r => r.PermissionsDenied)
            .FirstOrDefaultAsync(r => r.Id == roleId);
    }

    public Task<RoleEntity?> GetRoleAsync(string roleName)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<RoleEntity>> ListRolesAsync()
    {
        return await dbContext.Roles
            .Include(r => r.PermissionsAllowed)
            .Include(r => r.PermissionsDenied)
            .ToListAsync();
    }

    public async Task<bool> UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        var role = await GetRoleAsync(roleId);
        if (role == null)
        {
            return false;
        }

        var permission = await dbContext.Permissions.FirstOrDefaultAsync(p => p.Id == permissionId);
        if (permission == null)
        {
            return false;
        }

        if (!role.PermissionsAllowed.Any(p => p.Id == permission.Id) || !role.PermissionsDenied.Any(p => p.Id == permission.Id))
        {
            return false;
        }

        if (isAllowed)
        {
            role.PermissionsAllowed.Remove(permission);
        }
        else
        {
            role.PermissionsDenied.Remove(permission);
        }

        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateRoleAsync(RoleEntity role)
    {
        var existingRole = await dbContext.Roles.FindAsync(role.Id);
        if (existingRole == null)
        {
            return false;
        }

        existingRole.Name = role.Name;
        existingRole.IsActive = role.IsActive;
        existingRole.PermissionsAllowed = role.PermissionsAllowed;
        existingRole.PermissionsDenied = role.PermissionsDenied;

        dbContext.Entry(existingRole).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        return true;
    }
}
