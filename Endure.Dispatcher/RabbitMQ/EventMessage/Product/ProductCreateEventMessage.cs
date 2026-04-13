using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.RabbitMQ.EventMessage.Product;

[EventQueue("product.create")]
public class ProductCreateEventMessage : BaseEventMessage
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public required string Ean { get; set; }

    public required Guid WarehouseId { get; set; }
}
