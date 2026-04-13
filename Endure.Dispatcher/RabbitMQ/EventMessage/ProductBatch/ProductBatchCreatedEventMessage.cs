using Endure.Dispatcher.Attributes;
using Endure.Dispatcher.RabbitMQ.EventMessage.Product;

namespace Endure.Dispatcher.RabbitMQ.EventMessage.ProductBatch;

[EventQueue("productbatch.created")]
public class ProductBatchCreatedEventMessage : BaseEventMessage
{
    public Guid Id { get; set; }

    public long BestBefore { get; set; }

    public int Count { get; set; }

    public required Guid ProductId { get; set; }

    public required Guid StorageUnitId { get; set; }

    public required Guid WarehouseId { get; set; }

    /// <summary>
    /// The product that the batch is attached to, is included in case that the consumer does not have the product.
    /// </summary>
    public ProductCreateEventMessage Product { get; set; }
}
