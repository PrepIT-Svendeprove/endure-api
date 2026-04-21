using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.RabbitMQ.EventMessage.StorageUnit;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.RabbitMqConsumer.Consumers.StorageUnit;

internal sealed class ConsumeStorageUnitCreated(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ConsumeStorageUnitCreated> loggerService
    )
    : BaseRabbitMqConsumer<StorageUnitCreatedEventMessage>(rabbitMqConnection, serviceScopeFactory, loggerService)
{
    private readonly ILogger<ConsumeStorageUnitCreated> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(StorageUnitCreatedEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var storageUnitService = scope.ServiceProvider.GetRequiredService<IDispatcherStorageUnitService>();

        if (!await storageUnitService.CreateStorageUnitAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not create entity
                        Type: {typeof(StorageUnitCreatedEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity created
                    Type: {typeof(StorageUnitCreatedEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
