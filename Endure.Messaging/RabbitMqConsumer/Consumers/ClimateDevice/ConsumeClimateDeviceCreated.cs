using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.RabbitMQ.EventMessage.ClimateDevice;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.RabbitMqConsumer.Consumers.ClimateDevice;

internal sealed class ConsumeClimateDeviceCreated(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory scopeFactory,
        ILogger<ConsumeClimateDeviceCreated> loggerService
    )
    : BaseRabbitMqConsumer<ClimateDeviceCreatedEventMessage>(rabbitMqConnection, scopeFactory, loggerService)
{
    private readonly ILogger<ConsumeClimateDeviceCreated> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(ClimateDeviceCreatedEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dispatcherService = scope.ServiceProvider.GetRequiredService<IDispatcherClimateDeviceService>();

        if (!await dispatcherService.CreateClimateDeviceAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not create entity
                        Type: {typeof(ClimateDeviceCreatedEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity created:
                    Type: {typeof(ClimateDeviceCreatedEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
