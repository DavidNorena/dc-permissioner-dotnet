namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Models;

public static class PermissioneerEntityTypeBuilderExtensions
{
    public static void ConfigurePermissionsSeedData(this EntityTypeBuilder<PermissionEntity> builder, IEnumerable<PermissionSeedData> permissionsSeedData)
    {
        if (permissionsSeedData is not null)
        {
            var seedData = permissionsSeedData.Select(x => new PermissionEntity
            {
                Id = x.Id,
                Name = x.Name
            });

            builder.HasData(seedData);
        }
    }

    public static void ConfigureRolesSeedData(this EntityTypeBuilder<RoleEntity> builder, IEnumerable<RoleSeedData> rolesSeedData)
    {
        if (rolesSeedData is not null)
        {
            var seedData = rolesSeedData.Select(x => new RoleEntity
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            });

            builder.HasData(seedData);
        }
    }

    public static void ConfigureRolePermissionsAllowedSeedData(this EntityTypeBuilder<RolePermissionAllowedEntity> builder, IEnumerable<RolePermissionAllowedSeedData> rolePermissionsAllowedSeedData)
    {
        if (rolePermissionsAllowedSeedData is not null)
        {
            var seedData = rolePermissionsAllowedSeedData.Select(x => new RolePermissionAllowedEntity
            {
                RoleId = x.RoleId,
                PermissionId = x.PermissionId
            });

            builder.HasData(seedData);
        }
    }

    public static void ConfigureRolePermissionsDeniedSeedData(this EntityTypeBuilder<RolePermissionDeniedEntity> builder, IEnumerable<RolePermissionDeniedSeedData> rolePermissionsDeniedSeedData)
    {
        if (rolePermissionsDeniedSeedData is not null)
        {
            var seedData = rolePermissionsDeniedSeedData.Select(x => new RolePermissionDeniedEntity
            {
                RoleId = x.RoleId,
                PermissionId = x.PermissionId
            });

            builder.HasData(seedData);
        }
    }
}
