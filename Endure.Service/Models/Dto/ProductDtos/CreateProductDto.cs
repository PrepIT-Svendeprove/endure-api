namespace Endure.Service.Models.Dto.ProductDtos;

public class CreateProductDto
{
    public required string Name { get; set; }

    public required string EAN { get; set; }

    public string? Description { get; set; }
}
