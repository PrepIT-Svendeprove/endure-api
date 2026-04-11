using Endure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endure.Data.Configuration.TypeConfiguration;

internal class WarehouseTypeConfiguration : BaseTypeConfiguration<Warehouse>
{
    public override void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        base.Configure(builder);
    }
}
