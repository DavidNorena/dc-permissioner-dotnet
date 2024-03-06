namespace Dabitco.Permissioneer.Domain;

using Dabitco.Permissioneer.Domain.Abstract.Storage;
using Dabitco.Permissioneer.Domain.Models;
using Dabitco.Permissioneer.Storage;
using Microsoft.Extensions.DependencyInjection;

public static class PermissioneerBuilderExtensions
{
    public static PermissioneerBuilder AddInMemoryStorage(this PermissioneerBuilder builder)
    {
        builder.Services.AddSingleton<PermissionsStorageBase, InMemoryPermissionsStorage>();
        builder.Services.AddSingleton<ApiKeysStorageBase, InMemoryApiKeysStorage>();

        return builder;
    }

    public static PermissioneerBuilder AddPermissionsSeedData(this PermissioneerBuilder builder, IEnumerable<PermissionSeedData>? permissionsSeedData)
    {
        var mergedPermissionsSeedData = DefaultPermissioneerDataSeed.PermissionsSeed;
        if (permissionsSeedData != null && permissionsSeedData.Any())
        {
            mergedPermissionsSeedData = mergedPermissionsSeedData.Concat(permissionsSeedData);
        }
        builder.Services.AddSingleton(mergedPermissionsSeedData);

        return builder;
    }

    public static PermissioneerBuilder AddRolesSeedData(this PermissioneerBuilder builder, IEnumerable<RoleSeedData> rolesSeedData)
    {
        builder.Services.AddSingleton(rolesSeedData);

        var rolePermissionsSeedData = rolesSeedData
            .SelectMany(role =>
                (role.PermissionsAllowedIds ?? [])
                .Select(permissionId => new RolePermissionSeedData
                {
                    RoleId = role.Id,
                    PermissionId = permissionId,
                    IsAllowed = true,
                    IsSystem = true
                })
                .Concat(
                    (role.PermissionsDeniedIds ?? [])
                    .Select(permissionId => new RolePermissionSeedData
                    {
                        RoleId = role.Id,
                        PermissionId = permissionId,
                        IsAllowed = false,
                        IsSystem = true
                    })
                )
            );

        builder.Services.AddSingleton(rolePermissionsSeedData);

        return builder;
    }
}
