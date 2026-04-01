using Endure.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endure.Data.Configuration.TypeConfiguration;

internal class DietaryRestrictionTypeTypeConfiguration : BaseTypeConfiguration<DietaryRestrictionType>
{
    public override void Configure(EntityTypeBuilder<DietaryRestrictionType> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.DietaryRestrictions)
            .WithOne(x => x.DietaryRestrictionType)
            .HasForeignKey(x => x.DietaryRestrictionTypeId);

        builder.Ignore(x => x.UpdatedAt);
    }
}
