using Endure.Dispatcher.Mqtt;
using Endure.Messaging.MqttConsumer.Consumers;

namespace Endure.Messaging.MqttConsumer;

internal static class ServiceRegistration
{
    internal static IServiceCollection RegisterMqttDispatcherServices(this IServiceCollection services, IConfigurationManager config)
    {
        services.RegisterMqttConsumerExtensions(config);
        services.RegisterMqttConsumers();

        return services;
    }

    private static IServiceCollection RegisterMqttConsumers(this IServiceCollection services)
    {
        var consumers = typeof(ServiceRegistration)
                    .Assembly
                    .GetTypes()
                    .Where(x => x is { IsClass: true, IsAbstract: false } && typeof(IMqttConsumer).IsAssignableFrom(x));

        foreach (var consumer in consumers)
            services.AddSingleton(typeof(IMqttConsumer), consumer);

        services.AddHostedService<MqttConsumeWorker>();

        return services;
    }
}
