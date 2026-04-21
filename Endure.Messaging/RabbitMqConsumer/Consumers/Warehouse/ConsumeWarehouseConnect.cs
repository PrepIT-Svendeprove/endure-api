using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.RabbitMQ.EventMessage.Warehouse;

namespace Endure.Messaging.RabbitMqConsumer.Consumers.Warehouse;

internal sealed class ConsumeWarehouseConnect(
        IRabbitMqConsumerConnection rabbitMqConsumerConnection,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ConsumeWarehouseConnect> loggerService
    )
    : BaseRabbitMqConsumer<WarehouseConnectEventMessage>(rabbitMqConsumerConnection, serviceScopeFactory, loggerService)
{
    protected override Task HandleMessageAsync(WarehouseConnectEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
