namespace Dabitco.Permissioneer.Domain;

using Dabitco.Permissioneer.Domain.Abstract.Services;
using Dabitco.Permissioneer.Domain.Models;
using Dabitco.Permissioneer.Services;
using Microsoft.Extensions.DependencyInjection;

public static class PermissioneerBuilderExtensions
{
    public static PermissioneerBuilder AddInMemoryStorage(this PermissioneerBuilder builder)
    {
        builder.Services.AddSingleton<IPermissioneerStorage, InMemoryPermissioneerStorage>();

        return builder;
    }

    public static PermissioneerBuilder AddPermissionsSeedData(this PermissioneerBuilder builder, IEnumerable<PermissionSeedData> permissionsSeedData)
    {
        builder.Services.AddSingleton(permissionsSeedData);

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
