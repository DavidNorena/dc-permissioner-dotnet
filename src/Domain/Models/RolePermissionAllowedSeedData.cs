namespace Dabitco.Permissioneer.Domain.Models;

public record RolePermissionAllowedSeedData
{
    public required Guid RoleId { get; init; }
    public required Guid PermissionId { get; init; }
}
