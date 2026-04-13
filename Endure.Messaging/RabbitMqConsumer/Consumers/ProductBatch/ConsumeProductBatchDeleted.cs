using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.RabbitMQ.EventMessage.ProductBatch;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.RabbitMqConsumer.Consumers.ProductBatch;

internal sealed class ConsumeProductBatchDeleted(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ConsumeProductBatchDeleted> loggerService
    ) : BaseRabbitMqConsumer<ProductBatchDeletedEventMessage>(
        rabbitMqConnection,
        serviceScopeFactory,
        loggerService
    )
{
    private readonly ILogger<ConsumeProductBatchDeleted> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(ProductBatchDeletedEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productBatchService = scope.ServiceProvider.GetRequiredService<IDispatcherProductBatchService>();

        if (!await productBatchService.DeleteProductBatchAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not delete entity
                        Type: {typeof(ProductBatchDeletedEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity deleted
                    Type: {typeof(ProductBatchDeletedEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
