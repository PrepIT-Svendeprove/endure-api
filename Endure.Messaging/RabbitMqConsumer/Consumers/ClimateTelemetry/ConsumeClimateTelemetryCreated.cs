using Endure.Dispatcher.RabbitMQ.Connection;
using Endure.Dispatcher.RabbitMQ.EventMessage.ClimateTelemetry;
using Endure.Service.Services.Dispatcher;

namespace Endure.Messaging.RabbitMqConsumer.Consumers.ClimateTelemetry;

internal sealed class ConsumeClimateTelemetryCreated(
        IRabbitMqConsumerConnection connection,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ConsumeClimateTelemetryCreated> loggerService
    ) : BaseRabbitMqConsumer<ClimateTelemetryCreatedEventMessage>(connection, serviceScopeFactory, loggerService)
{
    private readonly ILogger<ConsumeClimateTelemetryCreated> _loggerService = loggerService;

    protected override async Task HandleMessageAsync(ClimateTelemetryCreatedEventMessage message, Guid requestId, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var climateTelemetry = scope.ServiceProvider.GetRequiredService<IDispatcherClimateTelemetryService>();

        if (!await climateTelemetry.CreateClimateTelemetryAsync(message))
        {
            _loggerService.LogWarning($"""
                    Could not create entity
                        Type: {typeof(ClimateTelemetryCreatedEventMessage).Name}
                        RequestId: {requestId}
                """);
            return;
        }

        _loggerService.LogInformation($"""
                Entity created:
                    Type: {typeof(ClimateTelemetryCreatedEventMessage).Name}
                    RequestId: {requestId}
            """);
    }
}
