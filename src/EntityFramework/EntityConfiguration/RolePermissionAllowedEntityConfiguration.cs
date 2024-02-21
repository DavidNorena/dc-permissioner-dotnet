namespace Dabitco.Permissioneer.EntityFramework.EntityConfiguration;

using Dabitco.Permissioneer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RolePermissionAllowedEntityConfiguration : IEntityTypeConfiguration<RolePermissionAllowedEntity>
{
    public void Configure(EntityTypeBuilder<RolePermissionAllowedEntity> builder)
    {
        builder.ToTable("RolePermissionAllowed");
    }
}
