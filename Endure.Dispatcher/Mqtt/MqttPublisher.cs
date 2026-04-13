using Endure.Dispatcher.Attributes;
using Endure.Dispatcher.Mqtt.Topic;
using Endure.Dispatcher.Util;
using MQTTnet;
using System.Reflection;
using System.Text.Json;

namespace Endure.Dispatcher.Mqtt;

internal sealed class MqttPublisher(
        IMqttClientHelper mqttClientHelper
    )
    : IMqttPublisher
{
    private readonly IMqttClientHelper _mqttClientHelper = mqttClientHelper;

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : BasePublishTopic
    {
        ArgumentNullException.ThrowIfNull(message);

        var payload = JsonSerializer.Serialize(message, DispatchSerializerOptions.Options);

        var topicAttr = typeof(T).GetCustomAttribute<TopicAttribute>();

        ArgumentNullException.ThrowIfNull(topicAttr);
        ArgumentException.ThrowIfNullOrWhiteSpace(topicAttr.TopicName);

        var mqttClient = await _mqttClientHelper.CreateMqttClientAsync();

        await mqttClient.ConnectAsync(_mqttClientHelper.MqttOptions, cancellationToken);

        var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(message.ConvertTopic(topicAttr.TopicName))
                .WithPayload(payload)
                .WithRetainFlag(true)
                .Build();

        await mqttClient.PublishAsync(applicationMessage, cancellationToken);

        await mqttClient.DisconnectAsync(cancellationToken: cancellationToken);

    }
}

public interface IMqttPublisher
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : BasePublishTopic;
}