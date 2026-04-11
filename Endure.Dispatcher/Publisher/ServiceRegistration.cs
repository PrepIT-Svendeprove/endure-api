using Endure.Dispatcher.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Endure.Dispatcher.Publisher;

public static class ServiceRegistration
{
    /// <summary>
    /// Registers the services, and other configuration for the Publisher.
    /// </summary>
    public static IServiceCollection RegisterDispatcherServices(this IServiceCollection services, IConfigurationManager config)
    {
        services.RegisterPublisherExtensions(config);

        services.AddHostedService<RegisterPublisherQueues>();

        return services;
    }
}
