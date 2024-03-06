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
                Name = x.Name,
                Description = x.Description,
                IsAssignable = x.IsAssignable,
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
                Description = x.Description,
                IsActive = x.IsActive,
                IsSystem = true
            });

            builder.HasData(seedData);
        }
    }

    public static void ConfigureRolePermissionsSeedData(this EntityTypeBuilder<RolePermissionEntity> builder, IEnumerable<RolePermissionSeedData> rolePermissionsSeedData)
    {
        if (rolePermissionsSeedData is not null)
        {
            var seedData = rolePermissionsSeedData.Select(x => new RolePermissionEntity
            {
                RoleId = x.RoleId,
                PermissionId = x.PermissionId,
                IsAllowed = x.IsAllowed,
                IsSystem = true
            });

            builder.HasData(seedData);
        }
    }
}
