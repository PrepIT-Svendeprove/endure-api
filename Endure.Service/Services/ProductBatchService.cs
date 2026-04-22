using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.AuditLogDtos;
using Endure.Service.Models.Dto.ProductBatchDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Models.StatusCodes;
using Endure.Service.Services.Dispatcher;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Principal;

namespace Endure.Service.Services;

internal class ProductBatchService(
        IDispatcherProductBatchService dispatcherProductBatchService,
        IWarehouseService warehouseService,
        IAuditLogService auditLogService,
        DatabaseContext context
    )
    : BaseService<ProductBatch>(context), IProductBatchService
{
    private readonly IDispatcherProductBatchService _dispatcherProductBatchService = dispatcherProductBatchService;
    private readonly IWarehouseService _warehouseService = warehouseService;
    private readonly IAuditLogService _auditLogService = auditLogService;

    protected override IQueryable<ProductBatch> MakePaginatedQuery(BasePaginatedFilter filter)
        => base.MakePaginatedQuery(filter).OrderByDescending(x => x.BestBefore);

    protected override async Task<ServiceResult> SoftDeleteEntity(Guid id, Expression<Func<ProductBatch, bool>>? prediate = null)
    {
        var result = await _dispatcherProductBatchService.DeleteProductBatchAsync(id);

        if (result is ServiceResult.Success)
        {
            var warehouseId = await _warehouseService.GetRootWarehouseIdAsync();
            await _auditLogService.CreateAuditLogAsync(new CreateAuditlogDto
            {
                Log = new Log { EntityId = id },
                LogLevel = LogLevel.Info,
                LogType = LogType.Deleted,
                WarehouseId = warehouseId
            }, warehouseId);
        }

        return result;
    }

    public async Task<Result> CreateProductBatch(CreateProductBatchDto productBatch)
    {
        if (await IsDeleted<StorageUnit>(productBatch.StorageUnitId) || await IsDeleted<Product>(productBatch.ProductId))
            return Result.Failed([ProductBatchStatusCodes.RELATION_DOES_NOT_EXIST]);

        var mapppedEntity = productBatch.MapToProductBatch(await _warehouseService.GetRootWarehouseIdAsync());

        var result = await _dispatcherProductBatchService.CreateProductBatchAsync(mapppedEntity) ? Result.Success() : Result.Failed([]);

        if (result.ServiceResult is ServiceResult.Success)
        {
            await _auditLogService.CreateAuditLogAsync(new CreateAuditlogDto
            {
                Log = new Log { EntityId = mapppedEntity.Id, Entity = productBatch },
                LogLevel = LogLevel.Info,
                LogType = LogType.Created,
                WarehouseId = mapppedEntity.WarehouseId
            }, mapppedEntity.WarehouseId);
        }

        return result;
    }

    public async Task<PaginatedResult<ProductBatchDto>> GetPaginatedProductsByProductId(ProductBatchFilter filter)
    {
        var context = MakePaginatedQuery(filter)
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.ProductId == filter.Id);

        var maxPages = (await context.CountAsync() / filter.Take) + 1;

        return new PaginatedResult<ProductBatchDto>(await context.MapToProductBatchDto().ToListAsync(), maxPages);
    }

    public async Task<PaginatedResult<ProductBatchDto>> GetPaginatedProductsByWarehouseId(ProductBatchFilter filter)
    {
        var context = MakePaginatedQuery(filter)
                .OrderByDescending(x => x.BestBefore)
                .Where(x => x.WarehouseId == filter.Id);

        var maxPages = (await context.CountAsync() / filter.Take) + 1;

        return new PaginatedResult<ProductBatchDto>(await context.MapToProductBatchDto().ToListAsync(), maxPages);
    }

    public async Task<PaginatedResult<ProductBatchDto>> GetPaginatedProductsByStorageUnitId(ProductBatchFilter filter)
    {
        var context = MakePaginatedQuery(filter)
                .Where(x => x.StorageUnitId == filter.Id && x.WarehouseId == filter.WarehouseId);

        var maxPages = (await context.CountAsync() / filter.Take) + 1;

        return new PaginatedResult<ProductBatchDto>(await context.MapToProductBatchDto().ToListAsync(), maxPages);
    }

    public async Task<int> GetProductBatchCountAsync(Guid warehouseId)
    {
        return await _context
                .ProductBatch
                .Where(x => x.WarehouseId == warehouseId && !x.IsDeleted)
                .CountAsync();
    }

    private async Task<bool> IsDeleted<TModel>(Guid id)
            where TModel : BaseModel
        => await _context.Set<TModel>().AnyAsync(x => x.Id == id && x.IsDeleted);
}

public interface IProductBatchService : IBaseService
{
    /// <summary>
    /// Creates a new product batch.
    /// </summary>
    Task<Result> CreateProductBatch(CreateProductBatchDto productBatch);

    /// <summary>
    /// Retrieves a paginated list of products from a product.
    /// </summary>
    Task<PaginatedResult<ProductBatchDto>> GetPaginatedProductsByProductId(ProductBatchFilter filter);

    /// <summary>
    /// Retrieves a paginated list of products from a warehouse.
    /// </summary>
    Task<PaginatedResult<ProductBatchDto>> GetPaginatedProductsByWarehouseId(ProductBatchFilter filter);

    /// <summary>
    /// Retrieves a paginated list of products from a storage unit.
    /// </summary>
    Task<PaginatedResult<ProductBatchDto>> GetPaginatedProductsByStorageUnitId(ProductBatchFilter filter);

    Task<int> GetProductBatchCountAsync(Guid warehouseId);
}
