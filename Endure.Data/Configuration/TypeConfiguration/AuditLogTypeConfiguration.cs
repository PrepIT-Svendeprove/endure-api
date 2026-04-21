using Endure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endure.Data.Configuration.TypeConfiguration;

internal class AuditLogTypeConfiguration : BaseTypeConfiguration<AuditLog>
{
    public override void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.LogLevel)
            .IsRequired();

        builder.Property(x => x.Log)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.HasKey(x => new { x.WarehouseId, x.Id });
    }
}
