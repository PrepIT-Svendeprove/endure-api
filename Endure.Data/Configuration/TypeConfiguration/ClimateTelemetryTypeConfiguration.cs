using Endure.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endure.Data.Configuration.TypeConfiguration;

internal class ClimateTelemetryTypeConfiguration : BaseTypeConfiguration<ClimateTelemetry>
{
    public override void Configure(EntityTypeBuilder<ClimateTelemetry> builder)
    {
        base.Configure(builder);

        builder.HasKey(x => new { x.WarehouseId, x.Id });

        builder.HasOne(x => x.ClimateDevice)
            .WithMany(x => x.ClimateTelemetry)
            .HasForeignKey(x => new { x.WarehouseId, x.Id });
    }   
}
