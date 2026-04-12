using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.EventMessage.ProductBatch;

[EventQueue("productbatch.deleted")]
public class ProductBatchDeletedEventMessage : BaseEventMessage
{
    public Guid Id { get; set; }

    public Guid WarehouseId { get; set; }
}
