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

    public async Task RegisterAsync(CancellationToken cancellationToken)
    {
        // We only want to consume topics that are marked as subscribers.
        if (Topic.Type is not TopicAttribute.TopicType.Subscribe)
            return;

        _mqttClient ??= await _mqttClientHelper.CreateMqttClientAsync();

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
        var cancellationToken = new CancellationTokenSource().Token;

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
            
            await HandleMessageAsync(message, arg.ClientId, requestId, cancellationToken);

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

    protected abstract Task HandleMessageAsync(TTopic message, string clientId, Guid requestId, CancellationToken cancellationToken);
}

internal interface IMqttConsumer
{
    Task RegisterAsync(CancellationToken cancellationToken);
}