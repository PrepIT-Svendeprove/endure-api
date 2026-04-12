using Endure.Data;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services.Internal;

internal class InternalProductBatchService(DatabaseContext context) : IInternalProductBatchService
{
    private readonly DatabaseContext _context = context;

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
    /// Checks if any productbatch is relying on a specific product across warehouses.
    /// </summary>
    Task<bool> IsRelyingOnProductId(Guid id);
}
