using System.Text.Json.Serialization;

namespace Endure.Service.Models.Dto.DietaryRestrictionDtos;

public sealed class CreateDietaryRestrictionDto
{
    /// <summary>
    /// The CPR number of the person.
    /// </summary>
    [JsonPropertyName(DietaryRestrictionConstants.CPR_NAME)]
    public required string Cpr { get; set; }

    /// <summary>
    /// The dietaryrestriction type that the new entity should be created to.
    /// </summary>
    [JsonPropertyName(DietaryRestrictionConstants.DIETARYRESTRICTIONTYPE_NAME)]
    public required Guid DietaryRestrictionTypeId { get; set; }
}
