using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.RabbitMQ.EventMessage.ClimateTelemetry;

[EventQueue("climatetelemetry.created")]
public sealed class ClimateTelemetryCreatedEventMessage : BaseEventMessage
{
    public required Guid Id { get; set; }

    public required double Humidity { get; set; }
    public required double Temperature { get; set; }

    public required Guid ClimateDeviceId { get; set; }

    public required Guid WarehouseId { get; set; }
}
