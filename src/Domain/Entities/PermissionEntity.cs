namespace Dabitco.Permissioneer.Domain.Entities;

public class PermissionEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<RoleEntity> Roles { get; set; } = [];
    public ICollection<RolePermissionEntity> RolePermissions { get; set; } = [];
}
