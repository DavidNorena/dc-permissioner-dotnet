namespace Dabitco.Permissioneer.TestAPI.Configuration;

using Dabitco.Permissioneer.AspNet.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

internal class AuthorizationOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context == null || context?.MethodInfo?.DeclaringType == null)
        {
            return;
        }

        var controllerAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true);
        var methodAttributes = context.MethodInfo.GetCustomAttributes(true);

        var authorizeAttributes = controllerAttributes.Union(methodAttributes)
            .Where(attr => typeof(AuthorizeAttribute).IsAssignableFrom(attr.GetType()));

        if (authorizeAttributes.Any())
        {
            AddDefaultResponses(operation);

            var schemes = authorizeAttributes
                .OfType<AuthorizeAttribute>()
                .SelectMany(attr => attr.AuthenticationSchemes?.Split(',') ?? [])
                .Distinct()
                .ToList();

            if (schemes.Count > 0)
            {
                foreach (var scheme in schemes)
                {
                    string schemeId = GetSchemeId(scheme.Trim());
                    AddSecurityRequirement(operation, schemeId);
                }
            }
            else
            {
                AddSecurityRequirement(operation, JwtBearerDefaults.AuthenticationScheme);
            }

        }
    }

    private static void AddDefaultResponses(OpenApiOperation operation)
    {
        if (!operation.Responses.ContainsKey("401"))
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        }
        if (!operation.Responses.ContainsKey("403"))
        {
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
        }
    }

    private static string GetSchemeId(string scheme)
    {
        return scheme switch
        {
            "ApiKey" => ApiKeySchemeDefaults.Scheme,// Ensure this matches the ID used in AddSecurityDefinition
            "Bearer" => JwtBearerDefaults.AuthenticationScheme,// Adjust as necessary
            _ => scheme,// Default case, might adjust based on your scheme naming
        };
    }

    private static void AddSecurityRequirement(OpenApiOperation operation, string schemeId)
    {
        var securityRequirement = new OpenApiSecurityRequirement
        {
            [new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = schemeId
                }
            }] = Array.Empty<string>()
        };

        operation.Security ??= [];
        operation.Security.Add(securityRequirement);
    }
}
