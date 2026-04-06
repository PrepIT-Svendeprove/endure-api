namespace Endure.Data.Models;

public class DietaryRestrictionType : BaseModel
{
    public required string Name { get; set; }

    public required string NormalizedName { get; set; }

    public new long? UpdatedAt { get; private set; }

    public List<DietaryRestriction> DietaryRestrictions { get; set; } = [];
}
