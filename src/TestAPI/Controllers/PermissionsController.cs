namespace Dabitco.Permissioneer.TestAPI.Controllers;

using Dabitco.Permissioneer.Domain.Abstract.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class PermissionsController(IPermissioneerContext permissioneerContext) : ControllerBase
{

    [HttpGet("is-granted", Name = nameof(TestIsGrantedAsync))]
    public async Task<IActionResult> TestIsGrantedAsync([FromQuery] string[] roles, [FromQuery] Guid permissionId)
    {
        var response = await permissioneerContext.IsGrantedAsync(roles, permissionId);

        return Ok(response);
    }

    [HttpGet("are-granted", Name = nameof(TestAreGrantedAsync))]
    public async Task<IActionResult> TestAreGrantedAsync([FromQuery] string[] roles, [FromQuery] Guid[] permissionIds)
    {
        var response = await permissioneerContext.AreGrantedAsync(roles, permissionIds);

        return Ok(response);
    }
}
