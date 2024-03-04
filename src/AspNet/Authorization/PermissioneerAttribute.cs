namespace Dabitco.Permissioneer.AspNet.Authorization;

using Dabitco.Permissioneer.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

public class PermissioneerAttribute : AuthorizeAttribute
{
    public PermissioneerAttribute(string scope)
    {
        if (string.IsNullOrWhiteSpace(scope))
        {
            throw new ArgumentException("Scope cannot be null or empty", nameof(scope));
        }

        Policy = $"Permissioneer;{scope};{OperatorType}";
    }

    public PermissionsOperatorType OperatorType { get; set; } = PermissionsOperatorType.And;
}
