namespace Dabitco.Permissioneer.TestAPI.Controllers;

using Dabitco.Permissioneer.Domain.Abstract.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class PermissionsController(IPermissioneerContext permissioneerContext) : ControllerBase
{

    [HttpGet("test", Name = nameof(TestUserPermissionAsync))]
    public async Task<IActionResult> TestUserPermissionAsync([FromQuery] string[] roles, [FromQuery] string permissionName)
    {
        var response = await permissioneerContext.CheckRolesPermissionAsync(roles, permissionName);

        return Ok(response);
    }

    [HttpGet("test-multiple", Name = nameof(TestMultipleUserPermissionAsync))]
    public async Task<IActionResult> TestMultipleUserPermissionAsync([FromQuery] string[] roles, [FromQuery] string[] permissionsName)
    {
        var response = await permissioneerContext.CheckRolesPermissionsAsync(roles, permissionsName);

        return Ok(response);
    }
}
