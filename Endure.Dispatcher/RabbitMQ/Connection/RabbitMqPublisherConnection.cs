using Endure.Dispatcher.RabbitMQ.Options;
using Microsoft.Extensions.Options;

namespace Endure.Dispatcher.RabbitMQ.Connection;

internal class RabbitMqPublisherConnection(IOptions<RabbitMqPublisherOptions> options)
    : BaseRabbitMqConnection(options.Value), IRabbitMqPublisherConnection;

public interface IRabbitMqPublisherConnection : IBaseRabbitMqConnection;