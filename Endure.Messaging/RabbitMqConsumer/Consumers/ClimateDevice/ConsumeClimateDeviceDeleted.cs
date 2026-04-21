using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.RabbitMQ.EventMessage.ClimateDevice;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.RabbitMqConsumer.Consumers.ClimateDevice;

internal sealed class ConsumeClimateDeviceDeleted(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory scopeFactory,
        ILogger<ConsumeClimateDeviceDeleted> loggerService
    )
    : BaseRabbitMqConsumer<ClimateDeviceDeleteEventMessage>(rabbitMqConnection, scopeFactory, loggerService)
{
    private readonly ILogger<ConsumeClimateDeviceDeleted> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(ClimateDeviceDeleteEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dispatcherService = scope.ServiceProvider.GetRequiredService<IDispatcherClimateDeviceService>();

        if (!await dispatcherService.DeleteClimateDeviceAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not delete entity
                        Type: {typeof(ClimateDeviceDeleteEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity delete:
                    Type: {typeof(ClimateDeviceDeleteEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
