namespace Dabitco.Permissioneer.EntityFramework.Services;

using Dabitco.Permissioneer.Domain.Abstract.Storage;
using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Models;
using Microsoft.EntityFrameworkCore;

public class DbPermissionsStorage(PermissioneerDbContext dbContext) : PermissionsStorageBase
{
    public async override Task<RoleModel> AddRoleAsync(RoleAddRequest request)
    {
        var existingRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == request.Name);
        if (existingRole != null)
        {
            throw new InvalidOperationException($"Role with name {request.Name} already exists");
        }

        var newRole = new RoleEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsActive = true,
        };

        await dbContext.Roles.AddAsync(newRole);
        await dbContext.SaveChangesAsync();

        return new RoleModel
        {
            Id = newRole.Id,
            Name = newRole.Name,
            Description = newRole.Description,
            IsActive = newRole.IsActive,
            IsSystem = newRole.IsSystem,
            Permissions = newRole.RolePermissions.Select(rp => rp.Permission.Name),
        };
    }

    public async override Task AssignPermissionToRoleAsync(RolePermissionAssignRequest request)
    {
        var role = await GetRoleAsync(request.RoleId)
            ?? throw new InvalidOperationException($"Role with id {request.RoleId} does not exist");

        if (role.IsSystem)
        {
            throw new InvalidOperationException("System roles cannot be modified");
        }

        var permission = await dbContext.Permissions.FirstOrDefaultAsync(p => p.Id == request.PermissionId)
            ?? throw new InvalidOperationException($"Permission with id {request.PermissionId} does not exist");

        if (!permission.IsAssignable)
        {
            throw new InvalidOperationException($"Permission with id {request.PermissionId} is not assignable");
        }

        if (role.RolePermissions.Any(rp => rp.PermissionId == permission.Id))
        {
            throw new InvalidOperationException($"Role with id {request.RoleId} already has permission with id {request.PermissionId}");
        }

        role.RolePermissions.Add(new RolePermissionEntity
        {
            RoleId = request.RoleId,
            PermissionId = request.PermissionId,
            IsAllowed = request.IsAllowed,
            IsSystem = false,
        });

        await dbContext.SaveChangesAsync();
    }

    public async override Task<bool> IsPermissionGrantedAsync(string[] roleNames, string permissionName)
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

    public async override Task<bool> ArePermissionsGrantedAsync(string[] roleNames, string[] permissionNames)
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

        var deniedPermissionsSet = rolesWithPermissions
            .Where(rp => !rp.IsAllowed && permissionIdSet.Contains(rp.PermissionId))
            .Select(rp => rp.PermissionId)
            .ToHashSet();

        if (deniedPermissionsSet.Count > 0)
        {
            return false;
        }

        var isAnyPermissionGranted = rolesWithPermissions.Any(rp => rp.IsAllowed && permissionIdSet.Contains(rp.PermissionId));

        return isAnyPermissionGranted;
    }

    public async override Task DeleteRoleAsync(Guid roleId)
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

    public override async Task<IEnumerable<PermissionEntity>> GetPermissionsAsync(Guid[] permissionsIds)
    {
        return await dbContext.Permissions
            .Where(p => permissionsIds.Contains(p.Id))
            .ToListAsync();
    }

    public async override Task<RoleEntity?> GetRoleAsync(Guid roleId)
    {
        return await dbContext.Roles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == roleId);
    }

    public async override Task<IEnumerable<RoleModel>> GetRolesAsync(string[] roleNames)
    {
        var roles = await dbContext.Roles
            .Include(r => r.RolePermissions)
            .Where(r => roleNames.Contains(r.Name))
            .OrderBy(r => r.Name)
            .ToListAsync();

        return roles.Select(r => new RoleModel
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            IsActive = r.IsActive,
            IsSystem = r.IsSystem,
            Permissions = r.RolePermissions.Select(rp => rp.Permission.Name),
        });
    }

    public async override Task<IEnumerable<PermissionModel>> ListPermissionsAsync()
    {
        return await dbContext.Permissions.Select(p => new PermissionModel
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            IsAssignable = p.IsAssignable,
        }).ToListAsync();
    }

    public async override Task<IEnumerable<RoleModel>> ListRolesAsync()
    {
        var roles = await dbContext.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .OrderBy(r => r.Name)
            .ToListAsync();

        return roles.Select(r => new RoleModel
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            IsActive = r.IsActive,
            IsSystem = r.IsSystem,
            Permissions = r.RolePermissions.Select(rp => rp.Permission.Name),
        });
    }

    public async override Task UnassignPermissionFromRoleAsync(RolePermissionAssignRequest request)
    {
        var role = await GetRoleAsync(request.RoleId)
            ?? throw new InvalidOperationException($"Role with id {request.RoleId} does not exist");

        if (role.IsSystem)
        {
            throw new InvalidOperationException("System roles cannot be modified");
        }

        var permission = await dbContext.Permissions.FirstOrDefaultAsync(p => p.Id == request.PermissionId)
            ?? throw new InvalidOperationException($"Permission with id {request.PermissionId} does not exist");

        if (!permission.IsAssignable)
        {
            throw new InvalidOperationException($"Permission with id {request.PermissionId} is not assignable");
        }

        var rolePermission = role.RolePermissions.FirstOrDefault(rp => rp.PermissionId == request.PermissionId)
            ?? throw new InvalidOperationException($"Role with id {request.RoleId} does not have permission with id {request.PermissionId}");

        role.RolePermissions.Remove(rolePermission);

        await dbContext.SaveChangesAsync();
    }

    public async override Task UpdateRoleAsync(RoleEntity role)
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
