using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.RabbitMQ.EventMessage.AuditLog;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.RabbitMqConsumer.Consumers.AuditLog;

internal sealed class ConsumeAuditLogCreated(
        IRabbitMqConsumerConnection rabbitMqConnection,
        IServiceScopeFactory scopeFactory,
        ILogger<ConsumeAuditLogCreated> loggerService
    )
    : BaseRabbitMqConsumer<AuditLogCreatedEventMessage>(
            rabbitMqConnection,
            scopeFactory,
            loggerService
        )
{
    private readonly ILogger<ConsumeAuditLogCreated> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(AuditLogCreatedEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var auditLogService = scope.ServiceProvider.GetRequiredService<IDispatcherAuditLogService>();

        if (!await auditLogService.CreateAuditLogAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not create entity
                        Type: {typeof(AuditLogCreatedEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity created:
                    Type: {typeof(AuditLogCreatedEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
