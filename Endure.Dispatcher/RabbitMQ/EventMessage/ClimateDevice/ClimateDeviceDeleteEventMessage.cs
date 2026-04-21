using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.RabbitMQ.EventMessage.ClimateDevice;

[EventQueue("climatedevice.deleted")]
public class ClimateDeviceDeleteEventMessage : BaseEventMessage
{
    public required Guid Id { get; set; }

    public required Guid WarehouseId { get; set; }
}
