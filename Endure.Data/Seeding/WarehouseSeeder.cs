using Endure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Endure.Data.Seeding;

public static class WarehouseSeeder
{
    public const string WAREHOUSE_ID_1 = "32be9f38-81ae-4e23-92fd-9c5456679a50";
    public const string WAREHOUSE_ID_2 = "5021e764-4fe7-4e96-84a7-46375e5c9bd3";
    public const string WAREHOUSE_ID_3 = "a04a72a5-3d62-4dae-9ce8-90cefdf04c01";

    private static List<Warehouse> _seededWarehouses = [
            new Warehouse() {
                Id = Guid.Parse(WAREHOUSE_ID_1),
                Name = "Central Lager",
                IsRoot = true,
                ParentWarehouseId = null,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            },
            new Warehouse() {
                Id = Guid.Parse(WAREHOUSE_ID_2),
                Name = "Distribution Center 1",
                IsRoot = false,
                ParentWarehouseId = Guid.Parse(WAREHOUSE_ID_1),
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            },
            new Warehouse() {
                Id = Guid.Parse(WAREHOUSE_ID_3),
                Name = "Distribution Center 2",
                IsRoot = false,
                ParentWarehouseId = Guid.Parse(WAREHOUSE_ID_1),
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }
        ];

    public static async Task<DatabaseContext> SeedWarehouseAsync(this DatabaseContext context)
    {
        if (await context.Warehouse.AnyAsync())
            return context;

        await context.Warehouse.AddRangeAsync(_seededWarehouses);
        await context.SaveChangesAsync();

        return context;
    }

    public static DatabaseContext SeedWarehouse(this DatabaseContext context)
    {
        if (context.Warehouse.Any())
            return context;

        context.Warehouse.AddRange(_seededWarehouses);
        context.SaveChanges();

        return context;
    }
}
