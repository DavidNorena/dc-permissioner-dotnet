namespace Dabitco.Permissioneer.EntityFramework.EntityConfiguration;

using Dabitco.Permissioneer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ApiKeyEntityConfiguration : IEntityTypeConfiguration<ApiKeyEntity>
{
    public void Configure(EntityTypeBuilder<ApiKeyEntity> builder)
    {
        builder.ToTable("ApiKey");

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

        builder.Property(x => x.HashedKey)
            .IsRequired()
            .HasMaxLength(64); // TODO: Change properly when not using SHA256

        builder.Property(x => x.OwnerId)
            .IsRequired()
            .HasMaxLength(64);

        builder.HasMany(x => x.Permissions)
            .WithMany(x => x.ApiKeys)
            .UsingEntity<ApiKeyPermissionEntity>(
                x => x.HasOne(rp => rp.Permission)
                        .WithMany(p => p.ApiKeyPermissions)
                        .HasForeignKey(rp => rp.PermissionId),

                x => x.HasOne(rp => rp.ApiKey)
                        .WithMany(p => p.ApiKeyPermissions)
                        .HasForeignKey(rp => rp.ApiKeyId)
            );
    }
}
