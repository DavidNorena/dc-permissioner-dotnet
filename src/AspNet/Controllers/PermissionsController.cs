namespace Dabitco.Permissioneer.AspNet.Controllers;

using Dabitco.Permissioneer.AspNet.Authorization;
using Dabitco.Permissioneer.Domain.Abstract.Services;
using Dabitco.Permissioneer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("permissioneer/[controller]")]
public class PermissionsController(IPermissioneerContext permissioneerContext) : ControllerBase
{
    [HttpGet(Name = nameof(GetPermissionsAsync))]
    [Permissioneer("read:permissions")]
    public async Task<IActionResult> GetPermissionsAsync()
    {
        var response = await permissioneerContext.ListPermissionsAsync();

        return Ok(response);
    }

    [HttpPost("role", Name = nameof(AddRoleAsync))]
    [Permissioneer("write:roles")]
    public async Task<IActionResult> AddRoleAsync(RoleAddRequest roleAddRequest)
    {
        var response = await permissioneerContext.AddRoleAsync(roleAddRequest);

        return Ok(response);
    }
}
