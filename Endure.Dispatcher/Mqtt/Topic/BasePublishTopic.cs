namespace Endure.Dispatcher.Mqtt.Topic;

public abstract class BasePublishTopic : BaseTopic
{
    /// <summary>
    /// Converts a topic, and fills it with the relevant publish data.
    /// </summary>
    public abstract string ConvertTopic(string topic);
}
