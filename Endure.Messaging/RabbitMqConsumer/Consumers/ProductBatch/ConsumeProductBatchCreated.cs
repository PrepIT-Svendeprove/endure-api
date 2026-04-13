using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.RabbitMQ.EventMessage.ProductBatch;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.RabbitMqConsumer.Consumers.ProductBatch;

internal sealed class ConsumeProductBatchCreated(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ConsumeProductBatchCreated> loggerService
    )
    : BaseRabbitMqConsumer<ProductBatchCreatedEventMessage>(
        rabbitMqConnection,
        serviceScopeFactory,
        loggerService
    )
{
    private readonly ILogger<ConsumeProductBatchCreated> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(ProductBatchCreatedEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productBatchService = scope.ServiceProvider.GetRequiredService<IDispatcherProductBatchService>();

        if (!await productBatchService.CreateProductBatchAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not create entity
                        Type: {typeof(ProductBatchCreatedEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity created:
                    Type: {typeof(ProductBatchCreatedEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
