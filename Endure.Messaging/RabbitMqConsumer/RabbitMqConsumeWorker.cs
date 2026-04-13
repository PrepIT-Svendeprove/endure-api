using Endure.Messaging.RabbitMqConsumer.Consumers;

namespace Endure.Messaging.RabbitMqConsumer;

internal sealed class RabbitMqConsumeWorker(IEnumerable<IRabbitMqConsumer> consumers) : BackgroundService
{
    private readonly IEnumerable<IRabbitMqConsumer> _consumers = consumers;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        foreach (var consumer in _consumers)
            await consumer.RegisterAsync(cancellationToken);

        Console.WriteLine($"Declared {_consumers.Count()} AMQP Consumers");
        Console.WriteLine("Waiting for messages");

        await Task.Delay(Timeout.Infinite, cancellationToken);
    }
}
