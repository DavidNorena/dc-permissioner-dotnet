namespace Dabitco.Permissioneer.Domain.Models;

public record RolePermissionSeedData
{
    public required Guid RoleId { get; init; }
    public required Guid PermissionId { get; init; }
    public bool IsAllowed { get; set; } = true;
    public bool IsSystem { get; set; }
}
