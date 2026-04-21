using Endure.Dispatcher.Attributes;
using Endure.Dispatcher.Helpers;
using Endure.Dispatcher.Mqtt;
using Endure.Dispatcher.Mqtt.Topic;
using Endure.Dispatcher.Util;
using MQTTnet;
using System.Text;
using System.Text.Json;

namespace Endure.Messaging.MqttConsumer.Consumers;

internal abstract class BaseMqttConsumer<TTopic>(
        IMqttClientHelper mqttClientHelper,
        IServiceScopeFactory serviceScopeFactory,
        ILogger loggerService
    ) : IMqttConsumer
    where TTopic : BaseTopic
{
    protected readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ILogger _loggerService = loggerService;

    private readonly IMqttClientHelper _mqttClientHelper = mqttClientHelper;

    protected static TopicAttribute Topic => typeof(TTopic).GetTopic();

    protected IMqttClient? _mqttClient;

    public async Task RegisterAsync(CancellationToken cancellationToken, IMqttClient mqttClient)
    {
        // We only want to consume topics that are marked as subscribers.
        if (Topic.Type is not TopicAttribute.TopicType.Subscribe)
            return;

        mqttClient.ApplicationMessageReceivedAsync += RecievedMessageAsync;

        var subOptions = _mqttClientHelper.Factory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(
                    new MqttTopicFilterBuilder()
                        .WithTopic(Topic.TopicName)
                        .Build()
                )
                .Build();

        await mqttClient.SubscribeAsync(subOptions, cancellationToken);

        Console.WriteLine($"Subscribed to topic: {Topic.TopicName}");
    }

    private async Task RecievedMessageAsync(MqttApplicationMessageReceivedEventArgs arg)
    {
        var cancellationToken = CancellationToken.None;
        var requestId = Guid.NewGuid();
        var incomingTopic = arg.ApplicationMessage.Topic;

        if (!TopicMatches(Topic.TopicName, incomingTopic))
            return;

        _loggerService.LogInformation($"""
            Received Topic Message
                Type: {typeof(TTopic).Name}
                Topic: {incomingTopic}
                RequestId: {requestId}
        """);

        try
        {
            var json = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
            var message = JsonSerializer.Deserialize<TTopic>(json, DispatchSerializerOptions.Options)
                ?? throw new InvalidOperationException("Failed to deserialize MQTT payload.");

            _loggerService.LogInformation($"""
                Beginning to handle message
                    Type: {typeof(TTopic).Name}
                    Topic: {incomingTopic}
                    RequestId: {requestId}
            """);

            await HandleMessageAsync(message, arg.ClientId, requestId, cancellationToken);
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"""
                Failed while handling message
                    Type: {typeof(TTopic).Name}
                    Topic: {incomingTopic}
                    RequestId: {requestId}
                    Exception: {ex}
            """);
        }
    }

    private static bool TopicMatches(string subscribedTopic, string incomingTopic)
    {
        var subscribedParts = subscribedTopic.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var incomingParts = incomingTopic.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (subscribedParts.Length != incomingParts.Length)
            return false;

        for (int i = 0; i < subscribedParts.Length; i++)
        {
            if (subscribedParts[i] == "+")
                continue;

            if (!string.Equals(subscribedParts[i], incomingParts[i], StringComparison.OrdinalIgnoreCase))
                return false;
        }

        return true;
    }

    protected abstract Task HandleMessageAsync(TTopic message, string clientId, Guid requestId, CancellationToken cancellationToken);
}

internal interface IMqttConsumer
{
    Task RegisterAsync(CancellationToken cancellationToken, IMqttClient mqttClient);
}