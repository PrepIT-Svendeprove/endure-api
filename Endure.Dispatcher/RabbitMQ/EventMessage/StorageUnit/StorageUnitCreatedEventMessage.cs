using Endure.Data.Models.Enums;
using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.RabbitMQ.EventMessage.StorageUnit;

[EventQueue("storageunit.created")]
public class StorageUnitCreatedEventMessage : BaseEventMessage
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public string? ShortName { get; set; }

    public string? Description { get; set; }

    public required StorageType StorageType { get; set; }

    public bool IsSlot { get; set; }

    public Guid? ParentId { get; set; }

    public Guid WarehouseId { get; set; }
}
