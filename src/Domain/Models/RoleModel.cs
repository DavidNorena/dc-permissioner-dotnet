namespace Dabitco.Permissioneer.Domain.Models;

public class RoleModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsSystem { get; set; }
    public IEnumerable<string> Permissions { get; set; } = [];
}
