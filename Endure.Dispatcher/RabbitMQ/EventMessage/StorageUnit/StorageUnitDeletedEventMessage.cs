using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.RabbitMQ.EventMessage.StorageUnit;

[EventQueue("storageunit.deleted")]
public class StorageUnitDeletedEventMessage : BaseEventMessage
{
    public Guid Id { get; set; }

    public Guid WarehouseId { get; set; }
}
