namespace Dabitco.Permissioneer.AspNet.Controllers;

using Dabitco.Permissioneer.AspNet.Authorization;
using Dabitco.Permissioneer.Domain.Abstract.Services;
using Dabitco.Permissioneer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class RolesController(IPermissioneerContext permissioneerContext) : ControllerBase
{
    [HttpPost(Name = nameof(CreateRoleAsync))]
    [Permissioneer("write:roles")]
    public async Task<IActionResult> CreateRoleAsync(RoleAddRequest request)
    {
        var response = await permissioneerContext.AddRoleAsync(request);

        return Ok(response);
    }

    [HttpGet(Name = nameof(GetRolesAsync))]
    [Permissioneer("read:roles")]
    public async Task<IActionResult> GetRolesAsync()
    {
        var response = await permissioneerContext.ListRolesAsync();

        return Ok(response);
    }

    [HttpDelete("{roleId}", Name = nameof(DeleteRoleAsync))]
    [Permissioneer("delete:roles")]
    public async Task<IActionResult> DeleteRoleAsync(Guid roleId)
    {
        await permissioneerContext.DeleteRoleAsync(roleId);

        return Ok();
    }

    [HttpPost("{roleId}/assign", Name = nameof(AssignPermissionAsync))]
    [Permissioneer("assign:permissions")]
    public async Task<IActionResult> AssignPermissionAsync(Guid roleId, RolePermissionAssignRequest request)
    {
        if (request.RoleId != roleId)
        {
            return BadRequest();
        }

        await permissioneerContext.AssignPermissionToRoleAsync(request);

        return Ok();
    }

    [HttpPost("{roleId}/unassign", Name = nameof(UnassignPermissionAsync))]
    [Permissioneer("unassign:permissions")]
    public async Task<IActionResult> UnassignPermissionAsync(Guid roleId, RolePermissionAssignRequest request)
    {
        if (request.RoleId != roleId)
        {
            return BadRequest();
        }

        await permissioneerContext.UnassignPermissionFromRoleAsync(request);

        return Ok();
    }
}
