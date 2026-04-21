using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.RabbitMQ.EventMessage.ClimateDevice;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.RabbitMqConsumer.Consumers.ClimateDevice;

internal sealed class ConsumeClimateDeviceUpdated(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory scopeFactory,
        ILogger<ConsumeClimateDeviceUpdated> loggerService
    )
    : BaseRabbitMqConsumer<ClimateDeviceUpdatedEventMessage>(
            rabbitMqConnection,
            scopeFactory,
            loggerService
        )
{
    private readonly ILogger<ConsumeClimateDeviceUpdated> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(ClimateDeviceUpdatedEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dispatcherService = scope.ServiceProvider.GetRequiredService<IDispatcherClimateDeviceService>();

        if (!await dispatcherService.UpdateClimateDeviceAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not create entity
                        Type: {typeof(ClimateDeviceUpdatedEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity created:
                    Type: {typeof(ClimateDeviceUpdatedEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
