namespace Endure.Dispatcher.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class TopicAttribute(string topicName, TopicAttribute.TopicType type) : Attribute
{
    public string TopicName { get; } = topicName;

    public TopicType Type { get; } = type;

    public enum TopicType
    {
        Subscribe,
        Publish
    }
}
