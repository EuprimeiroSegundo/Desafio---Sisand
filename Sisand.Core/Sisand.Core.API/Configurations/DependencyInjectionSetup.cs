using Sisand.Core.Ioc.Configurations;

namespace Sisand.Core.API.Configurations;

public static class DependencyInjectionSetup
{
    public static void AddDependencyInjectionSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        DependencyInjectionContainer.RegisterServices(services);
    }
}
