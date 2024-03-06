namespace Dabitco.Permissioneer.AspNet.Authorization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

public class PermissioneerAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("Permissioneer;", StringComparison.OrdinalIgnoreCase))
        {
            var policyParts = policyName["Permissioneer;".Length..].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var scopes = policyParts[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var policyBuilder = new AuthorizationPolicyBuilder();
            policyBuilder.RequireAuthenticatedUser();
            policyBuilder.AddRequirements(new PermissioneerRequirement(scopes));

            return policyBuilder.Build();
        }

        return await base.GetPolicyAsync(policyName);
    }
}
