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
        ILogger loggerService
    ) : IMqttConsumer
    where TTopic : BaseTopic
{
    private readonly IMqttClientHelper _mqttClientHelper = mqttClientHelper;
    private readonly ILogger _loggerService = loggerService;

    protected static TopicAttribute Topic => typeof(TTopic).GetTopic();

    protected IMqttClient? _mqttClient;

    public async Task RegisterAsync(CancellationToken cancellationToken)
    {
        _mqttClient ??= await _mqttClientHelper.CreateMqttClient();

        _mqttClient.ApplicationMessageReceivedAsync += RecievedMessageAsync;

        // Connect to the broker
        await _mqttClient.ConnectAsync(_mqttClientHelper.MqttOptions, cancellationToken);

        var subOptions = _mqttClientHelper.Factory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(
                    new MqttTopicFilterBuilder()
                        .WithTopic(Topic.TopicName)
                        .Build()
                )
                .Build();

        await _mqttClient.SubscribeAsync(subOptions, cancellationToken);

        Console.WriteLine($"Subscribed to topic: {Topic.TopicName}");
    }

    private async Task RecievedMessageAsync(MqttApplicationMessageReceivedEventArgs arg)
    {
        var requestId = Guid.NewGuid();

        _loggerService.LogInformation($"""
                Receied Topic Message
                    Type: {typeof(TTopic).Name}
                    RequestId: {requestId}
            """);

        try
        {
            var json = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
            var message = JsonSerializer.Deserialize<TTopic>(json, DispatchSerializerOptions.Options);

            _loggerService.LogInformation($"""
                    Beginning to handle message
                        Type: {typeof(TTopic).Name}
                        RequestId: {requestId}
                """);

        }
        catch (Exception ex)
        {
            _loggerService.LogError($"""
                    Failed while handling message
                        Type: {typeof(TTopic).Name}
                        RequestId: {requestId}
                        Exception: {ex.Message}
                """);
        }
    }

    protected abstract Task HandleMessageAsync(TTopic message, Guid requestId, CancellationToken cancellationToken);
}

internal interface IMqttConsumer
{
    Task RegisterAsync(CancellationToken cancellationToken);
}