using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.Mqtt.Topic.ClimateDevice;

[Topic("climate/lw/+", TopicAttribute.TopicType.Subscribe)]
public class ClimateLastWillTopic : BaseTopic
{
    public string ClimateDeviceCode { get; set; }
}
