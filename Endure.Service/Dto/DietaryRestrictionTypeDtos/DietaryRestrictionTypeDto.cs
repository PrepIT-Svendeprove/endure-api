using System.Text.Json.Serialization;

namespace Endure.Service.Dto.DietaryRestrictionTypeDtos;

public sealed class DietaryRestrictionTypeDto
{
    [JsonPropertyName(DietaryRestrictionTypeConstants.ID_NAME)]
    public required Guid Id { get; set; }

    [JsonPropertyName(DietaryRestrictionTypeConstants.NAME_NAME)]
    public required string Name { get; set; }
}
