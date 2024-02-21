namespace Microsoft.Extensions.DependencyInjection;

using Dabitco.Permissioneer.Domain;
using Dabitco.Permissioneer.Domain.Abstract.Services;
using Dabitco.Permissioneer.Services;

public static class PermissioneerIServiceCollectionExtensions
{
    public static PermissioneerBuilder AddPermissioneer(this IServiceCollection services)
    {
        services.AddScoped<IPermissioneerContext, PermissioneerContext>();

        return new PermissioneerBuilder(services);
    }
}
