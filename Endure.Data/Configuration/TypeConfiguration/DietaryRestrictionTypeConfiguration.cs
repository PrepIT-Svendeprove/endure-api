using Endure.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endure.Data.Configuration.TypeConfiguration;

internal class DietaryRestrictionTypeConfiguration : BaseTypeConfiguration<DietaryRestriction>
{
    public override void Configure(EntityTypeBuilder<DietaryRestriction> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.DietaryRestrictionType)
            .WithMany()
            .HasForeignKey(x => x.DietaryRestrictionTypeId);

        builder.Ignore(x => x.UpdatedAt);
    }
}
