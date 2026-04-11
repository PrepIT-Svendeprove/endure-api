namespace Endure.Dispatcher.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
internal class EventQueueAttribute(string queueName) : Attribute
{
    public string QueueName { get; init; } = queueName;

    public string RoutingKey { get => field ?? QueueName; init; }
}
