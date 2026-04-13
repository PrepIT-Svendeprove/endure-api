using Endure.Messaging.MqttConsumer.Consumers;

namespace Endure.Messaging.MqttConsumer;

internal class MqttConsumeWorker(IEnumerable<IMqttConsumer> mqttConsumer) : BackgroundService
{
    private readonly IEnumerable<IMqttConsumer> _consumers = mqttConsumer;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        foreach (var consumer in _consumers)
            await consumer.RegisterAsync(cancellationToken);

        Console.WriteLine($"Declared {_consumers.Count()} MQTT Consumers");

        await Task.Delay(Timeout.Infinite, cancellationToken);
    }
}
