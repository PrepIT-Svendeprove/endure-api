using Endure.Dispatcher.Attributes;

namespace Endure.Dispatcher.EventMessage.Product;

[EventQueue("product.delete")]
public class ProductDeleteEventMessage : BaseEventMessage
{
    public Guid Id { get; set; }
    public Guid WarehouseId { get; set; }
}
