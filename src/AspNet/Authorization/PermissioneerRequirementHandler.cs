namespace Dabitco.Permissioneer.AspNet.Authorization;

using System.Security.Claims;
using Dabitco.Permissioneer.Domain.Abstract.Services;
using Microsoft.AspNetCore.Authorization;

public class PermissioneerRequirementHandler(IPermissioneerContext permissioneerContext) : AuthorizationHandler<PermissioneerRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissioneerRequirement requirement)
    {
        var user = context.User;
        var scopeClaim = user.FindFirst("scope");
        var roleNames = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();

        if (scopeClaim != null)
        {
            var userScopes = scopeClaim.Value.Split(' ');
            if (await permissioneerContext.AreScopesGrantedAsync(requirement.Scopes, userScopes))
            {
                context.Succeed(requirement);
            }
        }
        else if (roleNames.Length > 0)
        {
            if (await permissioneerContext.ArePermissionsGrantedAsync(roleNames, requirement.Scopes))
            {
                context.Succeed(requirement);
            }
        }
    }
}
