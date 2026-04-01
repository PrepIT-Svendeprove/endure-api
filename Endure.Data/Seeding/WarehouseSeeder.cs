using Endure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Endure.Data.Seeding;

public static class WarehouseSeeder
{
    private static List<Warehouse> _seededWarehouses = [
            new Warehouse() {
                Id = 64001000,
                Name = "Central Lager",
                IsRoot = true,
                ParentWarehouseId = null
            },
            new Warehouse() {
                Id = 64001002,
                Name = "Distribution Center 1",
                IsRoot = false,
                ParentWarehouseId = 64001000
            },
            new Warehouse() {
                Id = 64001003,
                Name = "Distribution Center 2",
                IsRoot = false,
                ParentWarehouseId = 64001000
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
