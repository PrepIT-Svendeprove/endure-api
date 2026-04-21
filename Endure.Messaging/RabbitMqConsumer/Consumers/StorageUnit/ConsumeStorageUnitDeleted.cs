using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.RabbitMQ.EventMessage.StorageUnit;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.RabbitMqConsumer.Consumers.StorageUnit;

internal sealed class ConsumeStorageUnitDeleted(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ConsumeStorageUnitDeleted> loggerService
    )
    : BaseRabbitMqConsumer<StorageUnitDeletedEventMessage>(
        rabbitMqConnection,
        serviceScopeFactory,
        loggerService
    )
{
    private readonly ILogger<ConsumeStorageUnitDeleted> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(StorageUnitDeletedEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var storageUnitService = scope.ServiceProvider.GetRequiredService<IDispatcherStorageUnitService>();

        if (!await storageUnitService.DeleteStorageUnitAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not delete entity
                        Type: {typeof(StorageUnitDeletedEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity deleted
                    Type: {typeof(StorageUnitDeletedEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
