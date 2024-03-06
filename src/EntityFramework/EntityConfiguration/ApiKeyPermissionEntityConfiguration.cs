namespace Dabitco.Permissioneer.EntityFramework.EntityConfiguration;

using Dabitco.Permissioneer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ApiKeyPermissionEntityConfiguration : IEntityTypeConfiguration<ApiKeyPermissionEntity>
{
    public void Configure(EntityTypeBuilder<ApiKeyPermissionEntity> builder)
    {
        builder.ToTable("ApiKeyPermission");
    }
}
