namespace Dabitco.Permissioneer.EntityFramework;

using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Models;
using Dabitco.Permissioneer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PermissioneerDbContext(
    DbContextOptions<PermissioneerDbContext> options,
    PermissioneerStorageOptions storageOptions,
    IEnumerable<PermissionSeedData> permissionsSeedData,
    IEnumerable<RoleSeedData> rolesSeedData,
    IEnumerable<RolePermissionSeedData> rolePermissionsSeedData
) : DbContext(options)
{
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<PermissionEntity> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PermissioneerDbContext).Assembly);

        if (storageOptions.DefaultSchema is not null)
        {
            modelBuilder.HasDefaultSchema(storageOptions.DefaultSchema);
        }

        modelBuilder.Entity<PermissionEntity>().ConfigurePermissionsSeedData(permissionsSeedData);
        modelBuilder.Entity<RoleEntity>().ConfigureRolesSeedData(rolesSeedData);

        modelBuilder.Entity<RolePermissionEntity>().ConfigureRolePermissionsSeedData(rolePermissionsSeedData);
    }
}
