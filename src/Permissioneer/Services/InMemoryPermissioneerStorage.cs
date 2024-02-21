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
                List<PermissionEntity> allowedPermissions = [];
                List<PermissionEntity> deniedPermissions = [];

                if (roleSeedData.PermissionsAllowedIds is not null && roleSeedData.PermissionsAllowedIds.Any())
                {
                    allowedPermissions = permissions.Where(p => roleSeedData.PermissionsAllowedIds.Contains(p.Key)).Select(p => p.Value).ToList();
                }

                if (roleSeedData.PermissionsDeniedIds is not null && roleSeedData.PermissionsDeniedIds.Any())
                {
                    deniedPermissions = permissions.Where(p => roleSeedData.PermissionsDeniedIds.Contains(p.Key)).Select(p => p.Value).ToList();
                }

                roles.Add(roleSeedData.Id, new RoleEntity
                {
                    Id = roleSeedData.Id,
                    Name = roleSeedData.Name.ToLower(),
                    PermissionsAllowed = allowedPermissions,
                    PermissionsDenied = deniedPermissions,
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

    public Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        if (!roles.TryGetValue(roleId, out var role))
        {
            return Task.FromResult(false);
        }

        if (!permissions.TryGetValue(permissionId, out var permission))
        {
            return Task.FromResult(false);
        }

        if (role.PermissionsAllowed.Any(p => p.Id == permission.Id) || role.PermissionsDenied.Any(p => p.Id == permission.Id))
        {
            return Task.FromResult(false);
        }

        if (isAllowed)
        {
            role.PermissionsAllowed.Add(permission);
        }
        else
        {
            role.PermissionsDenied.Add(permission);
        }

        return Task.FromResult(true);
    }

    public Task<bool> CheckRolesPermissionAsync(string[] roleNames, string permissionName)
    {
        roleNames = roleNames.Select(rn => rn.ToLower()).ToArray();

        var permission = permissions.Values.FirstOrDefault(p => p.Name == permissionName);
        if (permission == null)
        {
            return Task.FromResult(false);
        }

        var rolesWithPermission = roles.Values
            .Where(r => roleNames.Contains(r.Name));

        var isPermissionDenied = rolesWithPermission.Any(r => r.PermissionsDenied.Contains(permission));
        if (isPermissionDenied)
        {
            return Task.FromResult(false);
        }

        var isPermissionAllowed = rolesWithPermission.Any(r => r.PermissionsAllowed.Contains(permission));
        return Task.FromResult(isPermissionAllowed);
    }

    public Task<bool> CheckRolesPermissionsAsync(string[] roleNames, string[] permissionNames)
    {
        roleNames = roleNames.Select(rn => rn.ToLower()).ToArray();

        var matchedPermissions = permissions.Values.Where(p => permissionNames.Contains(p.Name)).ToList();

        var rolesWithPermission = roles.Values
            .Where(r => roleNames.Contains(r.Name));

        foreach (var permission in matchedPermissions)
        {
            var isPermissionDenied = rolesWithPermission.Any(r => r.PermissionsDenied.Contains(permission));
            if (isPermissionDenied)
            {
                return Task.FromResult(false);
            }
        }

        var isAllPermissionsAllowed = matchedPermissions.All(mp =>
            rolesWithPermission.Any(r => r.PermissionsAllowed.Contains(mp)));

        return Task.FromResult(isAllPermissionsAllowed);
    }

    public Task<bool> DeleteRoleAsync(Guid roleId)
    {
        return Task.FromResult(roles.Remove(roleId));
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

    public Task<bool> UnassignPermissionFromRoleAsync(Guid roleId, Guid permissionId, bool isAllowed = true)
    {
        if (!roles.TryGetValue(roleId, out var role))
        {
            return Task.FromResult(false);
        }

        if (!permissions.TryGetValue(permissionId, out var permission))
        {
            return Task.FromResult(false);
        }

        if (!role.PermissionsAllowed.Any(p => p.Id == permission.Id) || !role.PermissionsDenied.Any(p => p.Id == permission.Id))
        {
            return Task.FromResult(false);
        }

        if (isAllowed)
        {
            role.PermissionsAllowed.Remove(permission);
        }
        else
        {
            role.PermissionsDenied.Remove(permission);
        }

        return Task.FromResult(true);
    }

    public Task<bool> UpdateRoleAsync(RoleEntity role)
    {
        if (!roles.ContainsKey(role.Id))
        {
            return Task.FromResult(false);
        }

        roles[role.Id] = role;

        return Task.FromResult(true);
    }
}
