using Endure.Data;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services.Internal;

internal class InternalProductBatchService(DatabaseContext context) : IInternalProductBatchService
{
    private readonly DatabaseContext _context = context;

    public async Task<bool> RemoveProductBatchesFromStorageUnitAsync(Guid storageUnitId)
    {
        return await _context
                .ProductBatch
                .Where(x => x.StorageUnitId == storageUnitId)
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsDeleted, true)) > 0;
    }

    public async Task<bool> IsRelyingOnProductId(Guid id)
    {
        return await _context
                .ProductBatch
                .AnyAsync(x => x.ProductId == id && !x.IsDeleted);
    }
}

internal interface IInternalProductBatchService
{
    /// <summary>
    /// Removes all of the productbatches that is in a specific storageunit.
    /// </summary>
    Task<bool> RemoveProductBatchesFromStorageUnitAsync(Guid storageUnitId);

    /// <summary>
    /// Checks if any productbatch is relying on a specific product across warehouses.
    /// </summary>
    Task<bool> IsRelyingOnProductId(Guid id);
}
