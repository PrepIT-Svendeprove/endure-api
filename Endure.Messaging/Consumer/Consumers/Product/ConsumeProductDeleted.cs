using Endure.Dispatcher.EventMessage.Product;
using Endure.Dispatcher.RabbitMQ;
using Endure.Service.Services.Dispatcher;
using System;
using System.Collections.Generic;
using System.Text;

namespace Endure.Messaging.Consumer.Consumers.Product;

internal sealed class ConsumeProductDeleted(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ConsumeProductDeleted> loggerService
    )
    : BaseRabbitMqConsumer<ProductDeleteEventMessage>(
        rabbitMqConnection,
        serviceScopeFactory,
        loggerService
    )
{
    private readonly ILogger<ConsumeProductDeleted> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(ProductDeleteEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IDispatcherProductService>();

        if (!await productService.DeleteProductAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not delete entity
                        Type: {typeof(ProductDeleteEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity deleted
                    Type: {typeof(ProductDeleteEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
