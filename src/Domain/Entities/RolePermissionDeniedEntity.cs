namespace Dabitco.Permissioneer.Domain.Entities;

public class RolePermissionDeniedEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}
