using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.RabbitMQ.EventMessage.Product;

[EventQueue("product.update")]
public class ProductUpdateEventMessage : BaseEventMessage
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required string Ean { get; set; }

    public required Guid WarehouseId { get; set; }
}
