namespace Dabitco.Permissioneer.AspNet.Authorization;

using Dabitco.Permissioneer.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

public class PermissioneerRequirement(string[] scopes, PermissionsOperatorType operatorType) : IAuthorizationRequirement
{
    public string[] Scopes { get; } = scopes;
    public PermissionsOperatorType OperatorType { get; } = operatorType;
}
