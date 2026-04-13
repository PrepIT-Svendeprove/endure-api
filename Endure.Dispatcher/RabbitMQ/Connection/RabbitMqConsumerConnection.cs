using Endure.Dispatcher.RabbitMQ.Options;
using Microsoft.Extensions.Options;

namespace Endure.Dispatcher.RabbitMQ.Connection;

internal class RabbitMqConsumerConnection(IOptions<RabbitMqConsumerOptions> options)
    : BaseRabbitMqConnection(options.Value), IRabbitMqConsumerConnection;

public interface IRabbitMqConsumerConnection : IBaseRabbitMqConnection;
