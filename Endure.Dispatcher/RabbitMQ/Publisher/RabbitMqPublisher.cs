using Endure.Dispatcher.Attributes;
using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.Util;
using RabbitMQ.Client;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Endure.Dispatcher.RabbitMQ.Publisher;

internal sealed class RabbitMqPublisher(IRabbitMqPublisherConnection rabbitMqConnection) : IMessagePublisher
{
    private readonly IRabbitMqPublisherConnection _rabbitMqConnection = rabbitMqConnection;

    public async Task PublishAsync<T>(T message, CancellationToken ctx = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        var payload = JsonSerializer.Serialize(message, DispatchSerializerOptions.Options);
        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json"
        };

        var connection = await _rabbitMqConnection.GetConnectionAsync(ctx);
        using var channel = await connection.CreateChannelAsync(cancellationToken: ctx);

        var queue = typeof(T).GetCustomAttribute<EventQueueAttribute>();

        ArgumentNullException.ThrowIfNull(queue);
        ArgumentException.ThrowIfNullOrWhiteSpace(queue.QueueName);

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: queue.QueueName,
            mandatory: true,
            basicProperties: properties,
            body: Encoding.UTF8.GetBytes(payload),
            cancellationToken: ctx
        );
    }
}
