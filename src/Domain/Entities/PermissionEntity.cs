namespace Dabitco.Permissioneer.Domain.Entities;

public class PermissionEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsAssignable { get; set; }

    public ICollection<RoleEntity> Roles { get; set; } = [];
    public ICollection<ApiKeyEntity> ApiKeys { get; set; } = [];

    public ICollection<ApiKeyPermissionEntity> ApiKeyPermissions { get; set; } = [];
    public ICollection<RolePermissionEntity> RolePermissions { get; set; } = [];
}
