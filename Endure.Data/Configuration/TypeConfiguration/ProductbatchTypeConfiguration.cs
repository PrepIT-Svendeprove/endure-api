using Endure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endure.Data.Configuration.TypeConfiguration;

internal class ProductbatchTypeConfiguration : BaseTypeConfiguration<ProductBatch>
{
    public void Configuration(EntityTypeBuilder<ProductBatch> builder)
    {
        base.Configure(builder);

        // Create composite keys, to ensure that the identifier of the batch is unique within the warehouse, but not globally.
        builder.HasKey(x => new { x.WarehouseId, x.Id });
        builder.HasOne(x => x.Product)
            .WithMany(x => x.ProductBatches)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.StorageUnit)
            .WithMany(x => x.Products)
            .HasForeignKey(x => new { x.WarehouseId, x.Id });
    }
}
