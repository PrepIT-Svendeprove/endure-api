using Endure.Data;
using Endure.Data.Models;
using Endure.Dispatcher.RabbitMQ.Publisher;

namespace Endure.Service.Services.Dispatcher;

internal sealed class DispatcherClimateTelemetryService(
        DatabaseContext context,
        IMessagePublisher messagePublisher
    )
    : BaseDispatcherService<ClimateTelemetry>(context, messagePublisher)
{
}
