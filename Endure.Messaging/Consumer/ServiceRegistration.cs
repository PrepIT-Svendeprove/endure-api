using Endure.Dispatcher;
using Endure.Messaging.Consumer.Consumers;

namespace Endure.Messaging.Consumer;

internal static class ServiceRegistration
{
    internal static IServiceCollection RegisterDispatcherServices(this IServiceCollection services, IConfigurationManager config)
    {
        services.RegisterConsumerExtensions(config);

        // Register this as the last registration
        services.RegisterRabbitMqConsumers();

        return services;
    }

    internal static IServiceCollection RegisterRabbitMqConsumers(this IServiceCollection services)
    {
        var consumers = typeof(ServiceRegistration)
                .Assembly
                .GetTypes()
                .Where(x => x is { IsClass: true, IsAbstract: false } && typeof(IRabbitMqConsumer).IsAssignableFrom(x));

        foreach (var consumer in consumers)
            services.AddSingleton(typeof(IRabbitMqConsumer), consumer);

        services.AddHostedService<RabbitMqConsumeWorker>();

        return services;
    }
}
