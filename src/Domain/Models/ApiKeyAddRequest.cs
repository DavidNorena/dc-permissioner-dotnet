namespace Dabitco.Permissioneer.Domain.Models;

public class ApiKeyAddRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public required string OwnerId { get; set; }
    public IEnumerable<Guid> PermissionsIds { get; set; } = [];
}
