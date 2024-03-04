namespace Dabitco.Permissioneer.Domain;

using Dabitco.Permissioneer.AspNet.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

public static class AspNetPermissioneerBuilderExtensions
{
    public static PermissioneerBuilder AddAspNetAuthorization(this PermissioneerBuilder builder)
    {
        builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissioneerAuthorizationPolicyProvider>();
        builder.Services.AddScoped<IAuthorizationHandler, PermissioneerRequirementHandler>();

        return builder;
    }
}
