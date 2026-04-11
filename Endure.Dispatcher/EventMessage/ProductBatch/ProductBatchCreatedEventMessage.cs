using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.EventMessage.ProductBatch;

[EventQueue("productbatch.created")]
public class ProductBatchCreatedEventMessage : BaseEventMessage
{
    public Guid Id { get; set; }

    public long BestBefore { get; set; }

    public int Count { get; set; }

    public required Guid ProductId { get; set; }

    public required Guid StorageUnitId { get; set; }

    public required Guid WarehouseId { get; set; }
}
