using Endure.Dispatcher.EventMessage.StorageUnit;
using Endure.Dispatcher.RabbitMQ;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.Consumer.Consumers.StorageUnit;

internal sealed class ConsumeStorageUnitUpdated(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ConsumeStorageUnitUpdated> loggerService
    )
    : BaseRabbitMqConsumer<StorageUnitUpdatedEventMessage>(
        rabbitMqConnection,
        serviceScopeFactory,
        loggerService
    )
{
    private readonly ILogger<ConsumeStorageUnitUpdated> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(StorageUnitUpdatedEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var storageUnitService = scope.ServiceProvider.GetRequiredService<IDispatcherStorageUnitService>();

        if (!await storageUnitService.UpdateStorageUnitAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not update entity
                        Type: {typeof(StorageUnitUpdatedEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity updated
                    Type: {typeof(StorageUnitUpdatedEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
