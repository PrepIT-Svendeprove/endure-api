namespace Endure.Service.Models.Dto.ProductDtos;

public class Top10ProductDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Ean { get; set; }

    /// <summary>
    /// Number of product batches, that has not gone over bestBefore date.
    /// </summary>
    public int BatchCount { get; set; }
}
