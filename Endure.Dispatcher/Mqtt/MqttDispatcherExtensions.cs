using Endure.Messaging.Mqtt.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Endure.Dispatcher.Mqtt;

public static class MqttDispatcherExtensions
{
    public static IServiceCollection RegisterMqttConsumerExtensions(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddSingleton<IMqttClientHelper, MqttClientHelper>();
        services.Configure<MqttOptions>(config.GetSection(MqttOptions.SectionName));

        return services;
    }

    public static IServiceCollection RegisterMqttPublisherExtensions(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddSingleton<IMqttClientHelper, MqttClientHelper>();
        services.AddSingleton<IMqttPublisher, MqttPublisher>();

        services.Configure<MqttOptions>(config.GetSection(MqttOptions.SectionName));

        return services;
    }
}
