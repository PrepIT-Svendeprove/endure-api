namespace Endure.Service.Models.Dto.ProductDtos;

public class UpdateProductDto
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
}
