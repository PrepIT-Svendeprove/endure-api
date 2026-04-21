using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.RabbitMQ.EventMessage.Warehouse;

[EventQueue("warehouse.connect")]
public sealed class WarehouseConnectEventMessage : BaseEventMessage
{
    public Guid WarehouseId { get; set; }

    public required string Name { get; set; }

    public string? ShortName { get; set; }

    public Guid? ParentId { get; set; }
}
