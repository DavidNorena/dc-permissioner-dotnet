namespace Dabitco.Permissioneer.AspNet.Controllers;

using System.Security.Claims;
using Dabitco.Permissioneer.AspNet.Authorization;
using Dabitco.Permissioneer.Domain.Abstract.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class PermissionsController(IPermissioneerContext permissioneerContext, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    private readonly HttpContext httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor), "The HTTP context cannot be null.");


    [HttpGet(Name = nameof(GetPermissionsAsync))]
    [Permissioneer("read:permissions")]
    public async Task<IActionResult> GetPermissionsAsync()
    {
        var response = await permissioneerContext.ListPermissionsAsync();

        return Ok(response);
    }

    [HttpGet("owned", Name = nameof(GetPermissionsOwnAsync))]
    [Permissioneer("read:own-permissions")]
    public async Task<IActionResult> GetPermissionsOwnAsync()
    {
        var user = httpContext.User;
        var scopeClaim = user.FindFirst("scope");
        var roleNames = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();

        List<string> response = [];

        if (roleNames.Length > 0)
        {
            var roles = await permissioneerContext.GetRolesAsync(roleNames);
            response.AddRange(roles.SelectMany(r => r.Permissions));
        }

        if (scopeClaim != null)
        {
            var userScopes = scopeClaim.Value.Split(' ');
            response.AddRange(userScopes);
        }

        return Ok(response);
    }
}
