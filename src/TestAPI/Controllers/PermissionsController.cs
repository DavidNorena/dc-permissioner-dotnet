namespace Dabitco.Permissioneer.TestAPI.Controllers;

using Dabitco.Permissioneer.AspNet.Authorization;
using Dabitco.Permissioneer.Domain.Abstract.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class PermissionsController(IPermissioneerContext permissioneerContext) : ControllerBase
{

    [HttpGet("is-granted", Name = nameof(TestIsGrantedAsync))]
    public async Task<IActionResult> TestIsGrantedAsync([FromQuery] string[] roles, [FromQuery] string permissionName)
    {
        var response = await permissioneerContext.IsPermissionGrantedAsync(roles, permissionName);

        return Ok(response);
    }

    [HttpGet("are-granted", Name = nameof(TestAreGrantedAsync))]
    public async Task<IActionResult> TestAreGrantedAsync([FromQuery] string[] roles, [FromQuery] string[] permissionNames)
    {
        var response = await permissioneerContext.ArePermissionsGrantedAsync(roles, permissionNames);

        return Ok(response);
    }

    [HttpGet("test-permission", Name = nameof(TestPermission))]
    [Permissioneer("read:roles")]
    public IActionResult TestPermission()
    {
        return Ok(new
        {
            Message = "You have the read:roles permission",
        });
    }
}
