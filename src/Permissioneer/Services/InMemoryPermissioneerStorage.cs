namespace Dabitco.Permissioneer.Services;

using Dabitco.Permissioneer.Domain.Abstract.Services;
using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Models;

public class InMemoryPermissioneerStorage : IPermissioneerStorage
{
    private readonly Dictionary<Guid, RoleEntity> roles = [];
    private readonly Dictionary<Guid, PermissionEntity> permissions = [];

    public InMemoryPermissioneerStorage(IEnumerable<PermissionSeedData> permissionsSeedData, IEnumerable<RoleSeedData> rolesSeedData)
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
                    RolePermissions = rolePermissions,
                    IsSystem = true,
                    IsActive = roleSeedData.IsActive,
                });
            }
        }
    }

    public Task<RoleEntity> AddRoleAsync(string roleName)
    {
        var role = roles.FirstOrDefault(r => r.Value.Name == roleName).Value;
        if (role is not null)
        {
            throw new InvalidOperationException($"Role with name {roleName} already exists.");
        }

        var newRole = new RoleEntity
        {
            Id = Guid.NewGuid(),
            Name = roleName,
        };

        roles.Add(newRole.Id, newRole);
        return Task.FromResult(newRole);
    }

    public async Task AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        var role = await GetRoleAsync(roleId)
            ?? throw new InvalidOperationException($"Role with id {roleId} does not exist");

        if (role.IsSystem)
        {
            throw new InvalidOperationException("System roles cannot be modified");
        }

        if (!permissions.TryGetValue(permissionId, out var permission))
        {
            throw new InvalidOperationException($"Permission with id {permissionId} does not exist");
        }

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
    }

    public Task<bool> IsGrantedAsync(string[] roleNames, Guid permissionId)
    {
        var normalizedRoleNames = new HashSet<string>(roleNames.Select(rn => rn.ToLowerInvariant()));

        if (!permissions.Values.Any(p => p.Id == permissionId))
        {
            return Task.FromResult(false);
        }

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

    public Task<bool> AreGrantedAsync(string[] roleNames, Guid[] permissionIds)
    {
        var normalizedRoleNames = new HashSet<string>(roleNames.Select(rn => rn.ToLowerInvariant()));
        if (!permissionIds.All(pid => permissions.Values.Any(p => p.Id == pid)))
        {
            return Task.FromResult(false);
        }

        var deniedPermissions = roles.Values
            .Where(role => normalizedRoleNames.Contains(role.Name.ToLowerInvariant()))
            .SelectMany(role => role.RolePermissions)
            .Where(rp => !rp.IsAllowed && permissionIds.Contains(rp.PermissionId))
            .Select(rp => rp.PermissionId)
            .ToHashSet();

        if (deniedPermissions.Count != 0)
        {
            return Task.FromResult(false);
        }

        var allowedPermissions = roles.Values
            .Where(role => normalizedRoleNames.Contains(role.Name.ToLowerInvariant()))
            .SelectMany(role => role.RolePermissions)
            .Where(rp => rp.IsAllowed && permissionIds.Contains(rp.PermissionId))
            .Select(rp => rp.PermissionId)
            .ToHashSet();

        return Task.FromResult(permissionIds.All(allowedPermissions.Contains));
    }

    public Task DeleteRoleAsync(Guid roleId)
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

    public Task<RoleEntity?> GetRoleAsync(Guid roleId)
    {
        roles.TryGetValue(roleId, out var role);

        return Task.FromResult(role);
    }

    public Task<IEnumerable<RoleEntity>> ListRolesAsync()
    {
        return Task.FromResult(roles.Values.AsEnumerable());
    }

    public async Task UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        var role = await GetRoleAsync(roleId)
            ?? throw new InvalidOperationException($"Role with id {roleId} does not exist");

        if (role.IsSystem)
        {
            throw new InvalidOperationException("System roles cannot be modified");
        }

        if (!permissions.TryGetValue(permissionId, out var permission))
        {
            throw new InvalidOperationException($"Permission with id {permissionId} does not exist");
        }

        var existingRolePermission = role.RolePermissions.FirstOrDefault(rp => rp.PermissionId == permissionId)
            ?? throw new InvalidOperationException($"Role with id {roleId} does not have permission with id {permissionId}");

        role.RolePermissions.Remove(existingRolePermission);
    }

    public Task UpdateRoleAsync(RoleEntity role)
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
