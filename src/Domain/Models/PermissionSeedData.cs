namespace Dabitco.Permissioneer.Domain.Models;

public record PermissionSeedData
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; set; }
    public bool IsAssignable { get; set; } = true;
}
