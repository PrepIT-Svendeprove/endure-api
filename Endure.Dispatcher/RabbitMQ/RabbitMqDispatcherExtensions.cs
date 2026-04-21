using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.RabbitMQ.Options;
using Endure.Dispatcher.RabbitMQ.Publisher;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Endure.Dispatcher.RabbitMQ;

public static class RabbitMqDispatcherExtensions
{
    /// <summary>
    /// Registers the RabbitMQ Conncetions that the Publisher needs to work..
    /// </summary>
    public static IServiceCollection RegisterRabbitMqPublisherExtensions(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddSingleton<IRabbitMqPublisherConnection, RabbitMqPublisherConnection>();
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        services.Configure<RabbitMqPublisherOptions>(config.GetSection(RabbitMqPublisherOptions.SectionName).GetSection("Publish"));

        return services;
    }

    /// <summary>
    /// Registers the RabbitMQ Connections that the Consumer needs to work.
    /// </summary>
    public static IServiceCollection RegisterRabbitMqConsumerExtensions(this IServiceCollection services, IConfigurationManager config)
    {
        RegisterRabbitMqPublisherExtensions(services, config);

        services.Configure<RabbitMqConsumerOptions>(config.GetSection(RabbitMqConsumerOptions.SectionName).GetSection("Consume"));
        services.AddSingleton<IRabbitMqConsumerConnection, RabbitMqConsumerConnection>();

        return services;
    }
}
