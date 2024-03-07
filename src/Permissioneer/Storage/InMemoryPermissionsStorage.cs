namespace Dabitco.Permissioneer.Storage;

using Dabitco.Permissioneer.Domain.Abstract.Storage;
using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Models;

public class InMemoryPermissionsStorage : PermissionsStorageBase
{
    private readonly Dictionary<Guid, RoleEntity> roles = [];
    private readonly Dictionary<Guid, PermissionEntity> permissions = [];

    public InMemoryPermissionsStorage(IEnumerable<PermissionSeedData> permissionsSeedData, IEnumerable<RoleSeedData> rolesSeedData)
    {
        BuildPermissionsFromSeedData(permissionsSeedData);
        BuildRolesFromSeedData(rolesSeedData);
    }

    public void BuildPermissionsFromSeedData(IEnumerable<PermissionSeedData> permissionsSeedData)
    {
        if (permissionsSeedData is null)
        {
            return;
        }

        foreach (var permissionSeedData in permissionsSeedData)
        {
            if (!permissions.ContainsKey(permissionSeedData.Id))
            {
                permissions.Add(permissionSeedData.Id, new PermissionEntity
                {
                    Id = permissionSeedData.Id,
                    Name = permissionSeedData.Name,
                    Description = permissionSeedData.Description,
                    IsAssignable = permissionSeedData.IsAssignable,
                });
            }
        }
    }

    public void BuildRolesFromSeedData(IEnumerable<RoleSeedData> rolesSeedData)
    {
        if (rolesSeedData is null)
        {
            return;
        }

        foreach (var roleSeedData in rolesSeedData)
        {
            if (!roles.ContainsKey(roleSeedData.Id))
            {
                var rolePermissions = new List<RolePermissionEntity>();

                if (roleSeedData.PermissionsAllowedIds is not null)
                {
                    foreach (var allowedId in roleSeedData.PermissionsAllowedIds)
                    {
                        if (permissions.ContainsKey(allowedId))
                        {
                            rolePermissions.Add(new RolePermissionEntity
                            {
                                PermissionId = allowedId,
                                RoleId = roleSeedData.Id,
                                IsSystem = true
                            });
                        }
                    }
                }

                if (roleSeedData.PermissionsDeniedIds is not null)
                {
                    foreach (var deniedId in roleSeedData.PermissionsDeniedIds)
                    {
                        if (permissions.ContainsKey(deniedId))
                        {
                            rolePermissions.Add(new RolePermissionEntity
                            {
                                PermissionId = deniedId,
                                RoleId = roleSeedData.Id,
                                IsAllowed = false,
                                IsSystem = true
                            });
                        }
                    }
                }

                roles.Add(roleSeedData.Id, new RoleEntity
                {
                    Id = roleSeedData.Id,
                    Name = roleSeedData.Name.ToLower(),
                    Description = roleSeedData.Description,
                    RolePermissions = rolePermissions,
                    IsSystem = true,
                    IsActive = roleSeedData.IsActive,
                });
            }
        }
    }

    public override Task<RoleModel> AddRoleAsync(RoleAddRequest request)
    {
        var role = roles.FirstOrDefault(r => r.Value.Name == request.Name).Value;
        if (role is not null)
        {
            throw new InvalidOperationException($"Role with name {request.Name} already exists.");
        }

        var newRole = new RoleEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
        };

        roles.Add(newRole.Id, newRole);

        return Task.FromResult(new RoleModel
        {
            Id = newRole.Id,
            Name = newRole.Name,
            Description = newRole.Description,
            IsActive = newRole.IsActive,
        });
    }

    public override async Task AssignPermissionToRoleAsync(RolePermissionAssignRequest request)
    {
        var role = await GetRoleAsync(request.RoleId)
            ?? throw new InvalidOperationException($"Role with id {request.RoleId} does not exist");

        if (role.IsSystem)
        {
            throw new InvalidOperationException("System roles cannot be modified");
        }

        if (!permissions.TryGetValue(request.PermissionId, out var permission))
        {
            throw new InvalidOperationException($"Permission with id {request.PermissionId} does not exist");
        }

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
    }

    public override Task<bool> IsPermissionGrantedAsync(string[] roleNames, string permissionName)
    {
        var normalizedRoleNames = new HashSet<string>(roleNames.Select(rn => rn.ToLowerInvariant()));

        var permission = permissions.Values.FirstOrDefault(p => p.Name.Equals(permissionName, StringComparison.OrdinalIgnoreCase));
        if (permission == null)
        {
            return Task.FromResult(false);
        }

        var permissionId = permission.Id;

        var isPermissionDenied = roles.Values.Any(role =>
            normalizedRoleNames.Contains(role.Name.ToLowerInvariant()) &&
            role.RolePermissions.Any(rp => rp.PermissionId == permissionId && !rp.IsAllowed));

        if (isPermissionDenied)
        {
            return Task.FromResult(false);
        }

        var isPermissionAllowed = roles.Values.Any(role =>
            normalizedRoleNames.Contains(role.Name.ToLowerInvariant()) &&
            role.RolePermissions.Any(rp => rp.PermissionId == permissionId && rp.IsAllowed));

        return Task.FromResult(isPermissionAllowed);
    }

    public override Task<bool> ArePermissionsGrantedAsync(string[] roleNames, string[] permissionNames)
    {
        var normalizedRoleNames = new HashSet<string>(roleNames.Select(rn => rn.ToLowerInvariant()));

        var permissionIds = permissions.Values
                                .Where(p => permissionNames.Contains(p.Name, StringComparer.OrdinalIgnoreCase))
                                .Select(p => p.Id)
                                .ToList();

        if (permissionIds.Count != permissionNames.Length)
        {
            return Task.FromResult(false);
        }

        var rolePermissions = roles.Values
            .Where(role => normalizedRoleNames.Contains(role.Name.ToLowerInvariant()))
            .SelectMany(role => role.RolePermissions)
            .ToList();

        var deniedPermissions = rolePermissions
            .Where(rp => permissionIds.Contains(rp.PermissionId) && !rp.IsAllowed)
            .Select(rp => rp.PermissionId)
            .ToHashSet();

        if (deniedPermissions.Count > 0)
        {
            return Task.FromResult(false);
        }

        var isAnyPermissionGranted = rolePermissions
            .Any(rp => permissionIds.Contains(rp.PermissionId) && rp.IsAllowed);

        return Task.FromResult(isAnyPermissionGranted);
    }

    public override Task DeleteRoleAsync(Guid roleId)
    {
        var existingRole = roles.FirstOrDefault(r => r.Value.Id == roleId).Value
            ?? throw new InvalidOperationException($"Role with id {roleId} does not exist");

        if (existingRole.IsSystem)
        {
            throw new InvalidOperationException("System roles cannot be deleted");
        }

        roles.Remove(roleId);

        return Task.CompletedTask;
    }

    public override Task<IEnumerable<PermissionEntity>> GetPermissionsAsync(Guid[] permissionsIds)
    {
        var permissionsList = permissionsIds
            .Select(id => permissions.TryGetValue(id, out var permission) ? permission : null)
            .Where(permission => permission != null)
            .Select(permission => permission!);

        return Task.FromResult(permissionsList);
    }

    public override Task<RoleEntity?> GetRoleAsync(Guid roleId)
    {
        roles.TryGetValue(roleId, out var role);

        return Task.FromResult(role);
    }

    public override Task<IEnumerable<RoleModel>> GetRolesAsync(string[] roleNames)
    {
        var normalizedRoleNames = new HashSet<string>(roleNames.Select(rn => rn.ToLowerInvariant()));

        var rolesList = roles.Values
            .Where(role => normalizedRoleNames.Contains(role.Name.ToLowerInvariant()))
            .Select(role => new RoleModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                IsActive = role.IsActive,
                Permissions = role.RolePermissions.Select(rp => permissions[rp.PermissionId].Name),
            });

        return Task.FromResult(rolesList);
    }

    public override Task<IEnumerable<PermissionModel>> ListPermissionsAsync()
    {
        return Task.FromResult(permissions.Values.Select(p => new PermissionModel
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            IsAssignable = p.IsAssignable,
        }));
    }

    public override Task<IEnumerable<RoleModel>> ListRolesAsync()
    {
        return Task.FromResult(roles.Values.Select(r => new RoleModel
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            IsActive = r.IsActive,
        }));
    }

    public override async Task UnassignPermissionFromRoleAsync(RolePermissionAssignRequest request)
    {
        var role = await GetRoleAsync(request.RoleId)
            ?? throw new InvalidOperationException($"Role with id {request.RoleId} does not exist");

        if (role.IsSystem)
        {
            throw new InvalidOperationException("System roles cannot be modified");
        }

        if (!permissions.TryGetValue(request.PermissionId, out var permission))
        {
            throw new InvalidOperationException($"Permission with id {request.PermissionId} does not exist");
        }

        if (!permission.IsAssignable)
        {
            throw new InvalidOperationException($"Permission with id {request.PermissionId} is not assignable");
        }

        var existingRolePermission = role.RolePermissions.FirstOrDefault(rp => rp.PermissionId == request.PermissionId)
            ?? throw new InvalidOperationException($"Role with id {request.RoleId} does not have permission with id {request.PermissionId}");

        role.RolePermissions.Remove(existingRolePermission);
    }

    public override Task UpdateRoleAsync(RoleEntity role)
    {
        var existingRole = roles.FirstOrDefault(r => r.Value.Id == role.Id).Value
            ?? throw new InvalidOperationException($"Role with id {role.Id} does not exist");

        if (existingRole.IsSystem)
        {
            throw new InvalidOperationException("System roles cannot be modified");
        }

        existingRole.Name = role.Name;
        existingRole.IsActive = role.IsActive;

        roles[role.Id] = existingRole;

        return Task.CompletedTask;
    }
}
