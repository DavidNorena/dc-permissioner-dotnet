namespace Dabitco.Permissioneer.EntityFramework.EntityConfiguration;

using Dabitco.Permissioneer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("Role");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.IsSystem)
            .IsRequired();

        builder.HasMany(e => e.Permissions)
            .WithMany(r => r.Roles)
            .UsingEntity<RolePermissionEntity>(
                x => x.HasOne(rp => rp.Permission)
                        .WithMany(p => p.RolePermissions)
                        .HasForeignKey(rp => rp.PermissionId),

                x => x.HasOne(rp => rp.Role)
                        .WithMany(p => p.RolePermissions)
                        .HasForeignKey(rp => rp.RoleId)
            );
    }
}
