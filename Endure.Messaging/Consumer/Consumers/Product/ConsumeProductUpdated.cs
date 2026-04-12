using Endure.Dispatcher.EventMessage.Product;
using Endure.Dispatcher.EventMessage.Warehouse;
using Endure.Dispatcher.RabbitMQ;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.Consumer.Consumers.Product;

internal sealed class ConsumeProductUpdated(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ConsumeProductUpdated> loggerService
    )
    : BaseRabbitMqConsumer<ProductUpdateEventMessage>(
        rabbitMqConnection,
        serviceScopeFactory,
        loggerService
    )
{
    private readonly ILogger<ConsumeProductUpdated> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(ProductUpdateEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IDispatcherProductService>();

        if (!await productService.UpdateProductAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not update entity
                        Type: {typeof(ProductUpdateEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity updated
                    Type: {typeof(ProductUpdateEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
