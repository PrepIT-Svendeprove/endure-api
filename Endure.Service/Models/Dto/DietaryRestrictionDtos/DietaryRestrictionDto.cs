using Endure.Service.Models.Dto.DietaryRestrictionTypeDtos;
using System.Text.Json.Serialization;

namespace Endure.Service.Models.Dto.DietaryRestrictionDtos;

public sealed class DietaryRestrictionDto
{
    /// <summary>
    /// Identifier of the Dietary entity.
    /// </summary>
    [JsonPropertyName(DietaryRestrictionConstants.ID_NAME)]
    public required Guid Id { get; set; }

    /// <summary>
    /// The stored cpy number that were registered.
    /// </summary>
    [JsonPropertyName(DietaryRestrictionConstants.CPR_NAME)]
    public required string Cpr { get; set; }

    /// <summary>
    /// The dietaryrestriction type that the dietaryrestriction were created for.
    /// </summary>
    [JsonPropertyName(DietaryRestrictionConstants.DIETARYRESTRICTIONTYPE_NAME)]
    public DietaryRestrictionTypeDto? DietaryRestrictionType { get; set; }
}
