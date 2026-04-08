namespace Endure.Service.Models.Dto.ProductDtos;

public class CreateProductDto
{
    /// <summary>
    /// Name of the product.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The products EAN.
    /// </summary>
    public required string EAN { get; set; }

    /// <summary>
    /// Name of the description.
    /// </summary>
    public string? Description { get; set; }
}
