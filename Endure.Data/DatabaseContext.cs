using Endure.Data.Configuration.TypeConfiguration;
using Endure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Endure.Data;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<AuditLog> AuditLog { get; set; }

    public DbSet<Warehouse> Warehouse { get; set; }
    public DbSet<StorageUnit> StorageUnit { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<ProductBatch> ProductBatch { get; set; }
    public DbSet<ClimateDevice> ClimateDevice { get; set; }
    public DbSet<ClimateTelemetry> ClimateTelemetry { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new WarehouseTypeConfiguration())
            .ApplyConfiguration(new StorageUnitTypeConfiguration())
            .ApplyConfiguration(new ClimateDeviceTypeConfiguration())
            .ApplyConfiguration(new ClimateTelemetryTypeConfiguration())
            .ApplyConfiguration(new ProductbatchTypeConfiguration())
            .ApplyConfiguration(new ProductTypeConfiguration());
    }
}
