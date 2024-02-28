namespace Dabitco.Permissioneer.Domain.Entities;

public class RoleEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsSystem { get; set; }

    public ICollection<RolePermissionEntity> RolePermissions { get; set; } = [];
    public ICollection<PermissionEntity> Permissions { get; set; } = [];
}
