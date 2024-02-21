namespace Dabitco.Permissioneer.EntityFramework.EntityConfiguration;

using Dabitco.Permissioneer.Domain.Entities;
using Dabitco.Permissioneer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("Role");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(x => x.PermissionsAllowed)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermissionAllowedEntity>(
                l => l.HasOne<PermissionEntity>().WithMany().HasForeignKey(x => x.PermissionId),
                l => l.HasOne<RoleEntity>().WithMany().HasForeignKey(x => x.RoleId)
            );

        builder.HasMany(x => x.PermissionsDenied)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermissionDeniedEntity>(
                l => l.HasOne<PermissionEntity>().WithMany().HasForeignKey(x => x.PermissionId),
                l => l.HasOne<RoleEntity>().WithMany().HasForeignKey(x => x.RoleId)
            );
    }
}
