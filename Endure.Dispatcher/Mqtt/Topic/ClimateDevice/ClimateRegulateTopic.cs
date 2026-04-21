using Endure.Dispatcher.Attributes;
using System.Text.Json.Serialization;

namespace Endure.Dispatcher.Mqtt.Topic.ClimateDevice;

[Topic("climate/control/{id}", TopicAttribute.TopicType.Publish)]
public class ClimateRegulateTopic : BasePublishTopic
{
    [JsonIgnore]
    public string ClimateDeviceCode { get; set; }

    public double Humidity { get; set; }

    public double Temperature { get; set; }

    /// <summary>
    /// Formats the topic from climate/control/{id} -> climate/control/Id.
    /// </summary>
    public override string ConvertTopic(string topic)
        => topic.Replace("{id}", ClimateDeviceCode);
}
