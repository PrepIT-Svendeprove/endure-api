using Endure.Dispatcher.RabbitMQ;
using Endure.Messaging.RabbitMqConsumer.Consumers;

namespace Endure.Messaging.RabbitMqConsumer;

internal static class ServiceRegistration
{
    internal static IServiceCollection RegisterRabbitMqDispatcherServices(this IServiceCollection services, IConfigurationManager config)
    {
        services.RegisterRabbitMqConsumerExtensions(config);

        // Register this as the last registration
        services.RegisterRabbitMqConsumers();

        return services;
    }

    private static IServiceCollection RegisterRabbitMqConsumers(this IServiceCollection services)
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
