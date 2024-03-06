namespace Dabitco.Permissioneer.Domain.Entities;

public class ApiKeyPermissionEntity
{
    public Guid ApiKeyId { get; set; }
    public Guid PermissionId { get; set; }

    public ApiKeyEntity ApiKey { get; set; } = null!;
    public PermissionEntity Permission { get; set; } = null!;
}
