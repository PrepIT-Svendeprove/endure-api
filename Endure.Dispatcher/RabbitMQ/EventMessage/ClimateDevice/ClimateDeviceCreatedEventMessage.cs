using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.RabbitMQ.EventMessage.ClimateDevice;

[EventQueue("climatedevice.created")]
public class ClimateDeviceCreatedEventMessage : BaseEventMessage
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required double SetTemperature { get; set; }

    public required double SetHumidity { get; set; }

    public required string ClimateDeviceCode { get; set; }

    public Guid? StorageUnitId { get; set; }

    public required Guid WarehouseId { get; set; }
}
