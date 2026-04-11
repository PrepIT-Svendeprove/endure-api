using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.EventMessage.Warehouse;

[EventQueue("warehouse.updated")]
public class WarehouseUpdatedEventMessage : BaseEventMessage
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public string? ShortName { get; set; }

    public Guid? ParentWarehouseId { get; set; }
}
