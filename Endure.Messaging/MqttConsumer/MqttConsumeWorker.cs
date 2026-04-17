using Endure.Dispatcher.Mqtt;
using Endure.Messaging.MqttConsumer.Consumers;

namespace Endure.Messaging.MqttConsumer;

internal class MqttConsumeWorker(IEnumerable<IMqttConsumer> mqttConsumer, IMqttClientHelper mqttClientHelper) : BackgroundService
{
    private readonly IEnumerable<IMqttConsumer> _consumers = mqttConsumer;
    private readonly IMqttClientHelper _mqttClientHelper = mqttClientHelper;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var mqttClient = await _mqttClientHelper.CreateMqttClientAsync();

        await mqttClient.ConnectAsync(_mqttClientHelper.MqttOptions, cancellationToken);

        foreach (var consumer in _consumers)
            await consumer.RegisterAsync(cancellationToken, mqttClient);

        Console.WriteLine($"Declared {_consumers.Count()} MQTT Consumers");

        await Task.Delay(Timeout.Infinite, cancellationToken);
    }
}
