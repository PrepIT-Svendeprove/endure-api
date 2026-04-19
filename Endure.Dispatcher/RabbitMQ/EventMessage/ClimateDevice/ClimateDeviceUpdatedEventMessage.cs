using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.RabbitMQ.EventMessage.ClimateDevice;

[EventQueue("climatedevice.updated")]
public class ClimateDeviceUpdatedEventMessage : BaseEventMessage
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public long? LastReceived { get; set; }

    public bool IsConnected { get; set; }

    public bool IsDisabled { get; set; }

    public required double? SetTemperature { get; set; }

    public required double? SetHumidity { get; set; }

    public required string ClimateDeviceCode { get; set; }

    public Guid? StorageUnitId { get; set; }

    public required Guid WareHouseId { get; set; }
}
