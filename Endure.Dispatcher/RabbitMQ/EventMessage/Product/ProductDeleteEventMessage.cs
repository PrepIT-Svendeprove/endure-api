using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.RabbitMQ.EventMessage.Product;

[EventQueue("product.deleted")]
public class ProductDeleteEventMessage : BaseEventMessage
{
    public Guid Id { get; set; }
    public Guid WarehouseId { get; set; }
}
