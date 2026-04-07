using System.Text.Json.Serialization;

namespace Endure.Service.Models.Dto.WarehouseDtos;

public class UpdateWarehouseDto
{
    [JsonPropertyName(WarehouseConstants.ID_NAME)]
    public required Guid Id { get; set; }

    /// <summary>
    /// The name of the warehouse.
    /// </summary>
    [JsonPropertyName(WarehouseConstants.NAME_NAME)]
    public required string Name { get; set; }

    [JsonPropertyName(WarehouseConstants.SHORTNAME_NAME)]
    public string? ShortName { get; set; }

    [JsonPropertyName(WarehouseConstants.ISROOT_NAME)]
    public bool IsRoot { get; set; }

    [JsonPropertyName(WarehouseConstants.PARENTWAREHOUSEID)]
    public Guid? ParentWarehouseId { get; set; }
}
