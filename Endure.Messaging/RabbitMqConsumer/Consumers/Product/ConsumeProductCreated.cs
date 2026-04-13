using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.RabbitMQ.EventMessage.Product;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.RabbitMqConsumer.Consumers.Product;

internal sealed class ConsumeProductCreated(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ConsumeProductCreated> loggerService
    )
    : BaseRabbitMqConsumer<ProductCreateEventMessage>(
        rabbitMqConnection,
        serviceScopeFactory,
        loggerService
    )
{
    private readonly ILogger<ConsumeProductCreated> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(ProductCreateEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IDispatcherProductService>();

        if (!await productService.CreateProductAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not delete entity
                        Type: {typeof(ProductCreateEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity deleted
                    Type: {typeof(ProductCreateEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
