using Endure.Dispatcher.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Endure.Dispatcher.RabbitMQ.Publisher;

public static class ServiceRegistration
{
    /// <summary>
    /// Registers the services, and other configuration for the Publisher.
    /// </summary>
    public static IServiceCollection RegisterDispatcherServices(this IServiceCollection services, IConfigurationManager config)
    {
        services.RegisterRabbitMqPublisherExtensions(config);

        services.AddHostedService<RegisterPublisherQueues>();

        return services;
    }
}
