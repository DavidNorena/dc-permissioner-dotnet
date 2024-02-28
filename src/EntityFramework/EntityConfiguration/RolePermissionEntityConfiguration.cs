namespace Dabitco.Permissioneer.EntityFramework.EntityConfiguration;

using Dabitco.Permissioneer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RolePermissionEntityConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
{
    public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
    {
        builder.ToTable("RolePermission");

        builder.Property(e => e.IsAllowed).IsRequired();
        builder.Property(e => e.IsSystem).IsRequired();
    }
}
