using Endure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Endure.Data.Configuration.TypeConfiguration;

internal class WarehouseTypeConfiguration : BaseTypeConfiguration<Warehouse, int>
{
    public override void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.ParentWarehouse)
            .WithMany(x => x.ChildWarehouses)
            .HasForeignKey(x => x.ParentWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
