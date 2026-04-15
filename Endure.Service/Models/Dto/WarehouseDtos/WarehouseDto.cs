using System.Text.Json.Serialization;

namespace Endure.Service.Models.Dto.WarehouseDtos;

/// <summary>
/// Represents a warehouse and related storage units.
/// </summary>
public sealed class WarehouseDto
{
    /// <summary>
    /// Identifier of the warehouse.
    /// </summary>
    [JsonPropertyName(WarehouseConstants.ID_NAME)]
    public Guid Id { get; set; }

    [JsonPropertyName(WarehouseConstants.NAME_NAME)]
    public string Name { get; set; }

    [JsonPropertyName(WarehouseConstants.SHORTNAME_NAME)]
    public string? ShortName { get; set; }

    [JsonPropertyName(WarehouseConstants.CREATEDAT_NAME)]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName(WarehouseConstants.UPDATEDAT_NAME)]
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Defines if the warehouse is the root warehouse.
    /// </summary>
    [JsonPropertyName(WarehouseConstants.ISROOT_NAME)]
    public bool IsRoot { get; set; }

    /// <summary>
    /// The parentwarehouse, it is only defined if the warehouse is not a sub-warehouse.
    /// </summary>
    [JsonPropertyName(WarehouseConstants.PARENTWAREHOUSEID)]
    public Guid? ParentId { get; set; }
}
