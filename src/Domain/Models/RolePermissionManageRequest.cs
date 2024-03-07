namespace Dabitco.Permissioneer.Domain.Models;

public class RolePermissionAssignRequest
{
    public required Guid RoleId { get; set; }
    public required Guid PermissionId { get; set; }
    public required bool IsAllowed { get; set; }
}
