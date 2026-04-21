using Endure.Dispatcher.Attributes;
using System.Reflection;

namespace Endure.Dispatcher.Helpers;

public static class EventQueueAttributeHelper
{
    public static string GetEventQueueName(this Type baseEventMessage)
        => baseEventMessage.GetCustomAttribute<EventQueueAttribute>()?.QueueName ?? throw new NullReferenceException($"{baseEventMessage.Name}.QueueName is missing");
}
