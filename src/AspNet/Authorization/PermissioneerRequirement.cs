namespace Dabitco.Permissioneer.AspNet.Authorization;

using Microsoft.AspNetCore.Authorization;

public class PermissioneerRequirement(string[] scopes) : IAuthorizationRequirement
{
    public string[] Scopes { get; } = scopes;
}
