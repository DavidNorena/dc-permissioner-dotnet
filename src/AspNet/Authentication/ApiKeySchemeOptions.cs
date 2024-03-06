namespace Dabitco.Permissioneer.AspNet.Authentication;

using Microsoft.AspNetCore.Authentication;

public class ApiKeySchemeOptions : AuthenticationSchemeOptions
{
    public string HeaderName { get; set; } = "X-API-Key";
    public string QueryParameterName { get; set; } = "apiKey";
}
