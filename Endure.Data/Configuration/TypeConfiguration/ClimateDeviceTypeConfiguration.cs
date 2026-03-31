using Endure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endure.Data.Configuration.TypeConfiguration;

internal class ClimateDeviceTypeConfiguration : BaseTypeConfiguration<ClimateDevice>
{
    public void Configure(EntityTypeBuilder<ClimateDevice> builder)
    {
        base.Configure(builder);

        builder.HasKey(x => new { x.WarehouseId, x.Id });
        builder.HasIndex(x => new { x.WarehouseId, x.StorageUnitId });

        builder.HasOne(x => x.StorageUnit)
            .WithMany(x => x.ClimateDevices)
            .HasForeignKey(x => new { x.StorageUnitId, x.WarehouseId })
            .OnDelete(DeleteBehavior.SetNull);
    }
}
