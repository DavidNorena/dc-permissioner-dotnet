namespace Dabitco.Permissioneer.EntityFramework.EntityConfiguration;

using Dabitco.Permissioneer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RolePermissionDeniedEntityConfiguration : IEntityTypeConfiguration<RolePermissionDeniedEntity>
{
    public void Configure(EntityTypeBuilder<RolePermissionDeniedEntity> builder)
    {
        builder.ToTable("RolePermissionDenied");
    }
}
