namespace Dabitco.Permissioneer.Domain.Entities;

public class RoleEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsSystem { get; set; }

    public ICollection<RolePermissionEntity> RolePermissions { get; set; } = [];
    public ICollection<PermissionEntity> Permissions { get; set; } = [];
}
