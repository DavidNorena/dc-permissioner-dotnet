namespace Dabitco.Permissioneer.EntityFramework.Options;

using Microsoft.EntityFrameworkCore;

public class PermissioneerStorageOptions
{
    public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; } = default;
    public string? DefaultSchema { get; set; }
}
