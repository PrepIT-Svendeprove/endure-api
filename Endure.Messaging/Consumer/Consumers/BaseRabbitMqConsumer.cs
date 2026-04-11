using Endure.Dispatcher.EventMessage;
using Endure.Dispatcher.Helpers;
using Endure.Dispatcher.RabbitMQ;
using Endure.Dispatcher.Util;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Endure.Dispatcher.Consumer.Consumers;

internal abstract class BaseRabbitMqConsumer<TMessage>(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory serviceScopeFactory
    )
    : IRabbitMqConsumer
    where TMessage : BaseEventMessage
{
    protected readonly IRabbitMqConsumerConnection _rabbitMqConnection = rabbitMqConnection;
    protected readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    protected IConnection? _connection;
    protected IChannel? _channel;

    protected string QueueName => typeof(TMessage).GetEventQueueName();

    public async Task RegisterAsync(CancellationToken cancellationToken)
    {
        _connection ??= await _rabbitMqConnection.GetConnectionAsync(cancellationToken);
        _channel ??= await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.BasicQosAsync(0, 1, false, cancellationToken);

        await _channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            autoDelete: false, // It should persist the queues, even after the application has disconnected.
            exclusive: false, // It should not create the queue exclusivly for the current connection.
            cancellationToken: cancellationToken
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (_, args) => RecievedMessageAsync(_, args, cancellationToken);

        await _channel.BasicConsumeAsync(
            QueueName,
            false,
            consumer,
            cancellationToken
        );
    }

    private async Task RecievedMessageAsync(object sender, BasicDeliverEventArgs eventArgs, CancellationToken cancellationToken)
    {
        try
        {
            var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            var message = JsonSerializer.Deserialize<TMessage>(json, DispatchSerializerOptions.Options) 
                ?? throw new InvalidOperationException($"Could not deserialize {typeof(TMessage).Name}");

            await HandleMessageAsync(message, cancellationToken);
            await _channel!.BasicAckAsync(eventArgs.DeliveryTag, false, cancellationToken);
        }
        catch (Exception ex)
        {
            await OnError(ex, eventArgs, cancellationToken);
            // There is no reason for it to requeue it, so we should just send it to the dead-letter queue instead.
            await _channel!.BasicNackAsync(eventArgs.DeliveryTag, false, false, cancellationToken);
        }
    }

    protected abstract Task HandleMessageAsync(TMessage message, CancellationToken cancellationToken);

    protected virtual Task OnError(Exception ex, BasicDeliverEventArgs args, CancellationToken cancellationToken)
        => Task.CompletedTask;
}

internal interface IRabbitMqConsumer
{
    Task RegisterAsync(CancellationToken cancellation);
}
