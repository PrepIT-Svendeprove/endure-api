namespace Endure.Service.Models.Dto.ProductDtos;

public class ProductDto
{
    public required Guid Id { get; set; }

    /// <summary>
    /// Name of the product.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The EAN of the product.
    /// </summary>
    public required string EAN { get; set; }

    /// <summary>
    /// Description of the product.
    /// </summary>
    public string? Description { get; set; }

    public Guid WarehouseId { get; set; }
}
