namespace Dabitco.Permissioneer.Domain.Models;

public record PermissionSeedData
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public bool IsActive { get; set; } = true;
}
