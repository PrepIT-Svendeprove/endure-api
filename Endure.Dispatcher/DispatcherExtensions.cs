using Endure.Dispatcher.Options;
using Endure.Dispatcher.Publisher;
using Endure.Dispatcher.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Endure.Dispatcher;

public static class DispatcherExtensions
{
    /// <summary>
    /// Registers the RabbitMQ Conncetions that the Publisher needs to work..
    /// </summary>
    public static IServiceCollection RegisterPublisherExtensions(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddSingleton<IRabbitMqPublisherConnection, RabbitMqPublisherConnection>();
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();

        services.Configure<RabbitMqPublisherOptions>(config.GetSection(RabbitMqPublisherOptions.SectionName).GetSection("Publish"));

        return services;
    }

    /// <summary>
    /// Registers the RabbitMQ Connections that the Consumer needs to work.
    /// </summary>
    public static IServiceCollection RegisterConsumerExtensions(this IServiceCollection services, IConfigurationManager config)
    {
        RegisterPublisherExtensions(services, config);

        services.Configure<RabbitMqConsumerOptions>(config.GetSection(RabbitMqConsumerOptions.SectionName).GetSection("Consume"));
        services.AddSingleton<IRabbitMqConsumerConnection, RabbitMqConsumerConnection>();

        return services;
    }
}
