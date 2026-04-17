using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.Mqtt.Topic.ClimateTelemetry;

[Topic("climate/telemetry/+", TopicAttribute.TopicType.Subscribe)]
public class ClimateTelemetryTopic : BaseTopic
{
    public Guid ClimateDeviceId { get; set; }

    public double Temperature { get; set; }

    public double Humidity { get; set; }
}
