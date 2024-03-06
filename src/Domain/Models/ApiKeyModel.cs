namespace Dabitco.Permissioneer.Domain.Models;

public class ApiKeyModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public required string OwnerId { get; set; }
    public IEnumerable<string> Permissions { get; set; } = [];
}
