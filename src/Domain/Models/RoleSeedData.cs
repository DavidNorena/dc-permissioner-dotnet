namespace Dabitco.Permissioneer.Domain.Models;

public record RoleSeedData
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public bool IsActive { get; set; } = true;
    public IEnumerable<Guid>? PermissionsAllowedIds { get; init; }
    public IEnumerable<Guid>? PermissionsDeniedIds { get; init; }
}
