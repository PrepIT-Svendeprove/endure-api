using Endure.Dispatcher.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Endure.Dispatcher.RabbitMQ;

internal class RabbitMqPublisherConnection(IOptions<RabbitMqPublisherOptions> options)
    : BaseRabbitMqConnection(options.Value), IRabbitMqPublisherConnection;

public interface IRabbitMqPublisherConnection : IBaseRabbitMqConnection;