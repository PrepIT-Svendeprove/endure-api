using Endure.Data.Models;
using Endure.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Endure.Data.Seeding;

public static class StorageUnitSeeder
{
    private static List<StorageUnit> _seededStorageUnits = [
        new StorageUnit() {
            Id = 1,
            WarehouseId = 64001002,
            Name = "Storage unit 1",
            ShortName = "SU1",
            StorageType = StorageType.Normal,
            IsSlot = false
        },
        new StorageUnit {
            Id = 2,
            WarehouseId = 64001002,
            Name = "Storage unit 2",
            ShortName = "SU2",
            StorageType = StorageType.Normal,
            IsSlot = true
        },
        new StorageUnit {
            Id = 3,
            WarehouseId = 64001002,
            Name = "Storage unit 3",
            ShortName = "SU3",
            StorageType = StorageType.Refrigerated,
            Description = "This is a sub storage unit",
            ParentStorageUnitId = 2,
            IsSlot = false
        }
    ];

    /// <summary>
    /// Adds seeded data, does not call context.SaveChangesAsync()
    /// </summary>
    public static async Task<DatabaseContext> SeedStorageUnitAsync(this DatabaseContext context)
    {
        if (await context.StorageUnit.AnyAsync())
            return context;

        await context.AddRangeAsync(_seededStorageUnits);
        await context.SaveChangesAsync();

        return context;
    }

    public static DatabaseContext SeedStorageUnit(this DatabaseContext context)
    {
        if (context.StorageUnit.Any())
            return context;

        context.AddRange(_seededStorageUnits);
        context.SaveChanges();

        return context;
    }
}
