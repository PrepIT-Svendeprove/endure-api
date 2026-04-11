using Endure.Dispatcher.EventMessage.AuditLog;
using Endure.Dispatcher.RabbitMQ;
using Endure.Service.Services.Dispatcher;

namespace Endure.Dispatcher.Consumer.Consumers.AuditLog;

internal class ConsumeAuditLogCreated(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory scopeFactory
    )
    : BaseRabbitMqConsumer<AuditLogCreatedEventMessage>(rabbitMqConnection, scopeFactory)
{   
    protected override async Task HandleMessageAsync(AuditLogCreatedEventMessage message, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var auditLogService = scope.ServiceProvider.GetRequiredService<IDispatcherAuditLogService>();

        Console.WriteLine("Message recieved - AuditLogCreatedEventMessage");

        if (!await auditLogService.CreateAuditLogAsync(message))
        {
            Console.WriteLine("Did not create AuditLog.");
            return;
        }

        Console.WriteLine("AuditLog created");
    }
}
