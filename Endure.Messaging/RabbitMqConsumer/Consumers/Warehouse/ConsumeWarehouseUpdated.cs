using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.RabbitMQ.EventMessage.Warehouse;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.RabbitMqConsumer.Consumers.Warehouse;

internal sealed class ConsumeWarehouseUpdated(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ConsumeWarehouseUpdated> loggerService
    )
    : BaseRabbitMqConsumer<WarehouseUpdatedEventMessage>(
        rabbitMqConnection,
        serviceScopeFactory,
        loggerService
    )
{
    private readonly ILogger<ConsumeWarehouseUpdated> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(WarehouseUpdatedEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var warehouseService = scope.ServiceProvider.GetRequiredService<IDispatcherWarehouseService>();

        if (!await warehouseService.UpdateWarehouseAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not update entity
                        Type: {typeof(WarehouseUpdatedEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity updated
                    Type: {typeof(WarehouseUpdatedEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
