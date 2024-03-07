namespace Dabitco.Permissioneer.AspNet.Controllers;

using Dabitco.Permissioneer.AspNet.Authorization;
using Dabitco.Permissioneer.Domain.Abstract.Services;
using Dabitco.Permissioneer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ApiKeysController(IPermissioneerContext permissioneerContext) : ControllerBase
{
    [HttpPost(Name = nameof(CreateApiKeyAsync))]
    [Permissioneer("write:api-keys")]
    public async Task<IActionResult> CreateApiKeyAsync(ApiKeyAddRequest addRequest)
    {
        var apiKey = await permissioneerContext.AddApiKeyAsync(addRequest);

        return Ok(new
        {
            apiKey,
        });
    }

    [HttpGet(Name = nameof(GetApiKeysAsync))]
    [Permissioneer("read:api-keys")]
    public async Task<IActionResult> GetApiKeysAsync()
    {
        var response = await permissioneerContext.ListApiKeysAsync();

        return Ok(response);
    }

    [HttpGet("{apiKeyId}", Name = nameof(GetApiKeyAsync))]
    [Permissioneer("read:api-keys")]
    public async Task<IActionResult> GetApiKeyAsync(Guid apiKeyId)
    {
        var response = await permissioneerContext.GetApiKeyAsync(apiKeyId);
        if (response == null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpDelete("{apiKeyId}", Name = nameof(RevokeApiKeyAsync))]
    [Permissioneer("revoke:api-keys")]
    public async Task<IActionResult> RevokeApiKeyAsync(Guid apiKeyId)
    {
        await permissioneerContext.RevokeApiKeyAsync(apiKeyId);

        return Ok();
    }
}
