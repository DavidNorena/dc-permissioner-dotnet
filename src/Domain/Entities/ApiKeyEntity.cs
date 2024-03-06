namespace Dabitco.Permissioneer.Domain.Entities;

public class ApiKeyEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string HashedKey { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public required string OwnerId { get; set; }

    public ICollection<PermissionEntity> Permissions { get; set; } = [];
    public ICollection<ApiKeyPermissionEntity> ApiKeyPermissions { get; set; } = [];
}
