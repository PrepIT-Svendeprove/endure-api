using Endure.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endure.Data.Configuration.TypeConfiguration;

internal class ProductTypeConfiguration : BaseTypeConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Id)
            .HasMaxLength(20);

        builder.HasKey(x => new { x.WarehouseId, x.Id });
    }
}
