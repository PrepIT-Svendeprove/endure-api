using Endure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endure.Data.Configuration.TypeConfiguration;

internal class StorageUnitTypeConfiguration : BaseTypeConfiguration<StorageUnit>
{
    public override void Configure(EntityTypeBuilder<StorageUnit> builder)
    {
        base.Configure(builder);

        // Configure composite keys, to ensure that the identifier is unique within the context of a warehouse.
        builder.HasKey(x => new { x.WarehouseId, x.Id });

        // Warehouse relation-ship
        builder.HasOne(x => x.Warehouse)
            .WithMany(x => x.StorageUnits)
            .HasForeignKey(x => x.WarehouseId);

        // Configure self-referencing relationship, to allow for an infinite number of storage units.
        builder.HasOne(x => x.ParentStorageUnit)
            .WithMany(x => x.ChildStorageUnits)
            .HasForeignKey(x => new { x.WarehouseId, x.ParentStorageUnitId })
            .OnDelete(DeleteBehavior.Restrict);
    }
}
