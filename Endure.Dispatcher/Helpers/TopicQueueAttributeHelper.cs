using Endure.Dispatcher.Attributes;
using System.Reflection;

namespace Endure.Dispatcher.Helpers;

public static class TopicQueueAttributeHelper
{
    public static TopicAttribute? GetTopic(this Type baseTopic)
        => baseTopic.GetCustomAttribute<TopicAttribute>();
}
