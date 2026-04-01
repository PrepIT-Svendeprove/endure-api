using Endure.Data.Models;
using Endure.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Endure.Data.Seeding;

public static class StorageUnitSeeder
{
    private static List<StorageUnit> _seededStorageUnits = [
        new StorageUnit() {
            Id = Guid.Parse("0e13045f-4db4-4a34-b949-fff6b2656748"),
            WarehouseId = 64001002,
            Name = "Storage unit 1",
            ShortName = "SU1",
            StorageType = StorageType.Normal,
            IsSlot = false
        },
        new StorageUnit {
            Id = Guid.Parse("2c2d414c-92dd-49b1-bacb-edbdd5ed362b"),
            WarehouseId = 64001002,
            Name = "Storage unit 2",
            ShortName = "SU2",
            StorageType = StorageType.Normal,
            IsSlot = true
        },
        new StorageUnit {
            Id = Guid.Parse("dc976961-eb66-45cb-b987-acd5b2e2c70f"),
            WarehouseId = 64001002,
            Name = "Storage unit 3",
            ShortName = "SU3",
            StorageType = StorageType.Refrigerated,
            Description = "This is a sub storage unit",
            ParentStorageUnitId = Guid.Parse("2c2d414c-92dd-49b1-bacb-edbdd5ed362b"),
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
