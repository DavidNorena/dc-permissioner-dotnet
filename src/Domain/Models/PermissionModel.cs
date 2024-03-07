namespace Dabitco.Permissioneer.Domain.Models;

public class PermissionModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsAssignable { get; set; }
}
