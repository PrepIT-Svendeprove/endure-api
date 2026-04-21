using Endure.Data.Models.Enums;
using System.Text.Json.Serialization;

namespace Endure.Service.Models.Dto.StorageUnitDtos;

public class CreateStorageUnitDto
{
    [JsonPropertyName(StorageUnitConstants.NAME_NAME)]
    public required string Name { get; set; }

    [JsonPropertyName(StorageUnitConstants.SHORTNAME_NAME)]
    public string? ShortName { get; set; }

    [JsonPropertyName(StorageUnitConstants.DESCRIPTION_NAME)]
    public string? Description { get; set; }

    [JsonPropertyName(StorageUnitConstants.PARENTSTORAGEUNITID_NAME)]
    public Guid? ParentId { get; set; }

    [JsonPropertyName(StorageUnitConstants.STORAGETYPE_NAME)]
    public StorageType StorageType { get; set; }

    [JsonPropertyName(StorageUnitConstants.ISSLOT_NAME)]
    public bool IsSlot { get; set; }
}
