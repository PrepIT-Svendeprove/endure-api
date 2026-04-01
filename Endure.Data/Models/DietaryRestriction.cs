namespace Endure.Data.Models;

public class DietaryRestriction : BaseModel
{
    public required string HashedCpr { get; set; }
    public Guid DietaryRestrictionTypeId { get; set; }

    public new long? UpdatedAt { get; private set; }

    public DietaryRestrictionType DietaryRestrictionType { get; set; } = default!;
}
