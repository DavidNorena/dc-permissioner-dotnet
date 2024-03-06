namespace Dabitco.Permissioneer.Domain;

using Dabitco.Permissioneer.Domain.Abstract.Storage;
using Dabitco.Permissioneer.EntityFramework;
using Dabitco.Permissioneer.EntityFramework.Options;
using Dabitco.Permissioneer.EntityFramework.Services;
using Microsoft.Extensions.DependencyInjection;

public static class PermissioneerBuilderExtensions
{
    public static PermissioneerBuilder AddEntityFrameworkStorage(this PermissioneerBuilder builder, Action<PermissioneerStorageOptions> storageOptions)
    {
        var options = new PermissioneerStorageOptions();
        storageOptions.Invoke(options);
        builder.Services.AddSingleton(options);

        builder.Services.AddScoped<PermissionsStorageBase, DbPermissionsStorage>();
        builder.Services.AddScoped<ApiKeysStorageBase, DbApiKeysStorage>();

        builder.Services.AddDbContext<PermissioneerDbContext>(dbCtxBuilder =>
        {
            options.ConfigureDbContext?.Invoke(dbCtxBuilder);
        });

        return builder;
    }
}
