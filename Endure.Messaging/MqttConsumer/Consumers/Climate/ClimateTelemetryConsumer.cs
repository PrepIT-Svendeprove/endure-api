using Endure.Dispatcher.Mqtt;
using Endure.Dispatcher.Mqtt.Topic.Climate;

namespace Endure.Messaging.MqttConsumer.Consumers.Climate;

internal class ClimateTelemetryConsumer(
        IMqttClientHelper mqttClientHelper,
        ILogger<ClimateTelemetryConsumer> logger
    )
    : BaseMqttConsumer<ClimateTelemetryTopic>(mqttClientHelper, logger)
{
    protected override async Task HandleMessageAsync(ClimateTelemetryTopic message, Guid requestId, CancellationToken cancellationToken)
    {
        Console.WriteLine("Message received");
    }
}
