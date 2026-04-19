using Endure.Dispatcher.Mqtt;
using Endure.Dispatcher.Mqtt.Topic.ClimateDevice;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.MqttConsumer.Consumers.ClimateDevice;

internal class ClimateLastWillConsumer(
        IMqttClientHelper mqttClientHelper,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ClimateLastWillConsumer> loggerService
    )
    : BaseMqttConsumer<ClimateLastWillTopic>(mqttClientHelper, serviceScopeFactory, loggerService)
{
    private readonly ILogger<ClimateLastWillConsumer> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(ClimateLastWillTopic message, string clientId, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dispatcherServie = scope.ServiceProvider.GetRequiredService<IDispatcherClimateDeviceService>();

        if (!await dispatcherServie.DisconnectClimateDeviceAsync(message.ClimateDeviceCode))
        {
            _loggerService.LogWarning($"""
                    Could not mark ClimateDevice as disconnected
                        Type: {typeof(ClimateLastWillTopic).Name}
                        RequestId: {requestId}
                        ClientId: {clientId}
                """);

            return;
        }

        _loggerService.LogWarning($"""
                    Marked ClimateDevice as disconnected
                        Type: {typeof(ClimateLastWillTopic).Name}
                        RequestId: {requestId}
                        ClientId: {clientId}
                """);
    }
}
