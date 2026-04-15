using Endure.Data.Models.Enums;
using System.Text.Json.Serialization;

namespace Endure.Service.Models.Dto.StorageUnitDtos;

public class CreateStorageUnitDto
{
    /// <summary>
    /// Name of the storageunit.
    /// </summary>
    [JsonPropertyName(StorageUnitConstants.NAME_NAME)]
    public required string Name { get; set; }

    /// <summary>
    /// Short name of the storageunit, can be used to group storage units.
    /// </summary>
    [JsonPropertyName(StorageUnitConstants.SHORTNAME_NAME)]
    public string? ShortName { get; set; }

    /// <summary>
    /// Description of the storageunit.
    /// </summary>
    [JsonPropertyName(StorageUnitConstants.DESCRIPTION_NAME)]
    public string? Description { get; set; }

    /// <summary>
    /// The parent storage unit, is only set if the unit is a sub-unit.
    /// </summary>
    [JsonPropertyName(StorageUnitConstants.PARENTSTORAGEUNITID_NAME)]
    public Guid? ParentId { get; set; }

    /// <summary>
    /// The type of products that the storage unit is supposed to be used for.
    /// </summary>
    [JsonPropertyName(StorageUnitConstants.STORAGETYPE_NAME)]
    public StorageType StorageType { get; set; }

    /// <summary>
    /// If this is set to true, the storageunit can contain product batches.
    /// </summary>
    [JsonPropertyName(StorageUnitConstants.ISSLOT_NAME)]
    public bool IsSlot { get; set; }
}
