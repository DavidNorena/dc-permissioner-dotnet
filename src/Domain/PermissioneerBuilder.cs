namespace Dabitco.Permissioneer.Domain;

using Microsoft.Extensions.DependencyInjection;

public class PermissioneerBuilder(IServiceCollection services)
{
    public IServiceCollection Services { get; } = services ?? throw new ArgumentNullException(nameof(services), "The services collection cannot be null.");
}
