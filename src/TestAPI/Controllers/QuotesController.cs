namespace Dabitco.Permissioneer.TestAPI.Controllers;

using Dabitco.Permissioneer.AspNet.Authentication;
using Dabitco.Permissioneer.AspNet.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class QuotesController : ControllerBase
{
    [HttpGet(Name = nameof(GetQuote))]
    [Permissioneer("read:quotes", ApiKeySchemeDefaults.Scheme)]
    public IActionResult GetQuote()
    {
        return Ok(new
        {
            Message = "Hello, World from Protected API Key Endpoint!",
        });
    }
}
