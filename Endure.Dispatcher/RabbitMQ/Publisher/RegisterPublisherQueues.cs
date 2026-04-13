using Endure.Dispatcher.Attributes;
using Endure.Dispatcher.RabbitMQ.Connection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Reflection;

namespace Endure.Dispatcher.RabbitMQ.Publisher;

/// <summary>
/// Registers all of the queues that exists within the given assembly.
/// </summary>
internal sealed class RegisterPublisherQueues(IRabbitMqPublisherConnection rabbitMqConnection) : IHostedService
{
    private readonly IRabbitMqPublisherConnection _rabbitMqConnection = rabbitMqConnection;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (Assembly.GetAssembly(GetType()) is not Assembly assembly)
            throw new NullReferenceException("Could not get assembly, when trying to register Queue Hosted Service.");

        var types = assembly.GetTypes().Select(x => x.GetCustomAttribute<EventQueueAttribute>()).Where(x => x is not null).ToList();

        await RegisterQueues(types ?? []);
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    private async Task RegisterQueues(List<EventQueueAttribute> queueAttributes)
    {
        try
        {
            if (queueAttributes.Count <= 0)
                return;

            var connection = await _rabbitMqConnection.GetConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync("endure.events", ExchangeType.Topic, durable: true, autoDelete: false);

            foreach (EventQueueAttribute attr in queueAttributes)
            {
                await channel.QueueDeclareAsync(
                    queue: attr.QueueName,
                    durable: true,
                    autoDelete: false, // It should persist the queues, even after the application has disconnected.
                    exclusive: false // It should not create the queue exclusivly for the current connection.
                );
            }
        }
        catch { }
    }
}
