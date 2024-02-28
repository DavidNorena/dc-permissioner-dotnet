namespace Dabitco.Permissioneer.Domain.Entities;

public class RolePermissionEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public bool IsAllowed { get; set; } = true;
    public bool IsSystem { get; set; }

    public RoleEntity Role { get; set; } = null!;
    public PermissionEntity Permission { get; set; } = null!;
}
