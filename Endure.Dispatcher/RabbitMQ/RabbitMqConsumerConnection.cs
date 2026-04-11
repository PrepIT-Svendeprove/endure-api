using Endure.Dispatcher.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Endure.Dispatcher.RabbitMQ;

internal class RabbitMqConsumerConnection(IOptions<RabbitMqConsumerOptions> options)
    : BaseRabbitMqConnection(options.Value), IRabbitMqConsumerConnection;

public interface IRabbitMqConsumerConnection : IBaseRabbitMqConnection;
