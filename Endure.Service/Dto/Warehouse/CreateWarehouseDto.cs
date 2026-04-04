namespace Endure.Service.Dto.Warehouse;

public sealed class CreateWarehouseDto
{
    /// <summary>
    /// Name of the warehouse.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Abbreviated name of the warehouse.
    /// </summary>
    public string? ShortName { get; set; }

    /// <summary>
    /// If IsRoot is set, it will set it as the current root warehouse.
    /// </summary>
    public bool IsRoot { get; set; } = false;

    /// <summary>
    /// The identifier of the warehouse of who it is being connected to.
    /// </summary>
    public Guid? ParentWarehouseId { get; set; }
}
