using System.Text.Json.Serialization;

namespace Endure.Service.Dto.WarehouseDtos;

public sealed class CreateWarehouseDto
{
    /// <summary>
    /// Name of the warehouse.
    /// </summary>
    [JsonPropertyName(WarehouseConstants.NAME_NAME)]
    public required string Name { get; set; }

    /// <summary>
    /// Abbreviated name of the warehouse.
    /// </summary>
    [JsonPropertyName(WarehouseConstants.SHORTNAME_NAME)]
    public string? ShortName { get; set; }

    /// <summary>
    /// If IsRoot is set, it will set it as the current root warehouse.
    /// </summary>
    [JsonPropertyName(WarehouseConstants.ISROOT_NAME)]
    public bool IsRoot { get; set; } = false;

    /// <summary>
    /// The identifier of the warehouse of who it is being connected to.
    /// </summary>
    [JsonPropertyName(WarehouseConstants.PARENTWAREHOUSEID)]
    public Guid? ParentWarehouseId { get; set; }
}
