namespace Dabitco.Permissioneer.Domain.Entities;

public class RoleEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public ICollection<PermissionEntity> PermissionsAllowed { get; set; } = [];
    public ICollection<PermissionEntity> PermissionsDenied { get; set; } = [];
}
