using Endure.Dispatcher.Mqtt;
using Endure.Dispatcher.Mqtt.Topic.ClimateDevice;
using Endure.Dispatcher.Mqtt.Topic.ClimateTelemetry;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.MqttConsumer.Consumers.ClimateTelemetry;

internal class ClimateTelemetryConsumer(
        IMqttClientHelper mqttClientHelper,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ClimateTelemetryConsumer> loggerService
    )
    : BaseMqttConsumer<ClimateTelemetryTopic>(mqttClientHelper, serviceScopeFactory, loggerService)
{
    private readonly ILogger<ClimateTelemetryConsumer> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(ClimateTelemetryTopic message, string clientId, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dispatcherService = scope.ServiceProvider.GetRequiredService<IDispatcherClimateTelemetryService>();

        if (!await dispatcherService.CreateClimateTelemetryAsync(message, message.ClimateDeviceId))
        {
            _loggerService.LogWarning($"""
                    Could not create ClimateTelemetry data.
                        Type: {typeof(ClimateTelemetryTopic).Name}
                        RequestId: {requestId}
                        ClientId: {clientId}
                """);

            return;
        }

        _loggerService.LogWarning($"""
                    Created ClimateTelemetry data.
                        Type: {typeof(ClimateTelemetryTopic).Name}
                        RequestId: {requestId}
                        ClientId: {clientId}
                """);
    }
}
