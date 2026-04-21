using Endure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Endure.Data.Seeding;

public static class WarehouseSeeder
{
    public static Guid rootWarehouseId = Guid.NewGuid();

    private static readonly List<Warehouse> _seededWarehouses = [
            new Warehouse() {
                Id = rootWarehouseId,
                Name = "Central Lager",
                IsRoot = true,
                ParentId = null,
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
