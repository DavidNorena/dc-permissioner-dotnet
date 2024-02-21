namespace Dabitco.Permissioneer.Domain.Models;

public record RolePermissionDeniedSeedData
{
    public required Guid RoleId { get; init; }
    public required Guid PermissionId { get; init; }
}
