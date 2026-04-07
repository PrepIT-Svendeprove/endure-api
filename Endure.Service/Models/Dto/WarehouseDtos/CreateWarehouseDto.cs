using System.Text.Json.Serialization;

namespace Endure.Service.Models.Dto.WarehouseDtos;

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
    /// The identifier of the warehouse of who it is being connected to.
    /// </summary>
    [JsonPropertyName(WarehouseConstants.PARENTWAREHOUSEID)]
    public Guid? ParentWarehouseId { get; set; }
}
