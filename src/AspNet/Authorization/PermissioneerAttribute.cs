namespace Dabitco.Permissioneer.AspNet.Authorization;

using Microsoft.AspNetCore.Authorization;

public class PermissioneerAttribute : AuthorizeAttribute
{
    public PermissioneerAttribute(string scope, string? authenticationSchemes = null)
    {
        if (string.IsNullOrWhiteSpace(scope))
        {
            throw new ArgumentException("Scope cannot be null or empty", nameof(scope));
        }

        Policy = $"Permissioneer;{scope}";
        AuthenticationSchemes = authenticationSchemes;
    }
}
