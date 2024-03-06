namespace Dabitco.Permissioneer.AspNet.Authentication;

using System.Security.Claims;
using System.Text.Encodings.Web;
using Dabitco.Permissioneer.Domain.Abstract.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

#pragma warning disable CS0618 // Type or member is obsolete

public class ApiKeySchemeHandler(
    IPermissioneerContext permissioneerContext,
    IOptionsMonitor<ApiKeySchemeOptions> options, ILoggerFactory logger,
    UrlEncoder encoder, ISystemClock clock
) : AuthenticationHandler<ApiKeySchemeOptions>(options, logger, encoder, clock)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string apiKeyValue;

        if (Request.Headers.TryGetValue(Options.HeaderName, out StringValues headerApiKey))
        {
            apiKeyValue = headerApiKey.ToString();
        }
        else if (Request.Query.TryGetValue(Options.QueryParameterName, out StringValues queryApiKey))
        {
            apiKeyValue = queryApiKey.ToString();
        }
        else
        {
            return AuthenticateResult.Fail("API key not found");
        }

        // TODO: Implement caching mechanism here
        // Check if API key is cached
        // If not, retrieve from database and cache the result

        var apiKey = await permissioneerContext.GetApiKeyAsync(apiKeyValue);
        if (apiKey is null)
        {
            return AuthenticateResult.Fail("Invalid API key");
        }

        var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, apiKey.Id.ToString()),
        new(ClaimTypes.Name, apiKey.Name),
        new("scope", string.Join(" ", apiKey.Permissions))
    };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
