using System.Text.Json.Serialization;

namespace Endure.Service.Models.Dto.DietaryRestrictionTypeDtos;

public sealed class CreateDietaryRestrictionTypeDto
{
    /// <summary>
    /// Name of the new dietaryrestriction type.
    /// </summary>
    [JsonPropertyName(DietaryRestrictionTypeConstants.NAME_NAME)]
    public required string Name { get; set; }
}
