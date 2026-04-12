using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.ProductBatchDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Models.StatusCodes;
using Endure.Service.Services.Dispatcher;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Endure.Service.Services;

internal class ProductBatchService(
        IDispatcherProductBatchService dispatcherProductBatchService,
        DatabaseContext context
    )
    : BaseService<ProductBatch>(context), IProductBatchService
{
    private readonly IDispatcherProductBatchService _dispatcherProductBatchService = dispatcherProductBatchService;

    protected override IQueryable<ProductBatch> MakePaginatedQuery(BasePaginatedFilter filter)
        => base.MakePaginatedQuery(filter).OrderByDescending(x => x.BestBefore);

    protected override async Task<ServiceResult> SoftDeleteEntity(Guid id, Expression<Func<ProductBatch, bool>>? prediate = null)
    {
        return await _dispatcherProductBatchService.DeleteProductBatchAsync(id);
    }

    public async Task<Result> CreateProductBatch(CreateProductBatchDto productBatch)
    {
        if (await IsDeleted<Warehouse>(productBatch.WarehouseId) || await IsDeleted<StorageUnit>(productBatch.StorageUnitId) || await IsDeleted<Product>(productBatch.ProductId))
            return Result.Failed([ProductBatchStatusCodes.RELATION_DOES_NOT_EXIST]);

        var mapppedEntity = productBatch.MapToProductBatch();

        return await _dispatcherProductBatchService.CreateProductBatchAsync(mapppedEntity) ? Result.Success() : Result.Failed([]);
    }

    public async Task<ProductBatchDto?> GetProductBatchById(Guid id)
    {
        return await _context
                .ProductBatch
                .Where(x => x.Id == id)
                .MapToProductBatchDto()
                .FirstOrDefaultAsync();
    }

    public async Task<List<ProductBatchDto>> GetPaginatedProductsByProductId(ProductBatchFilter filter)
    {
        return await MakePaginatedQuery(filter)
                .Where(x => x.ProductId == filter.Id)
                .MapToProductBatchDto()
                .ToListAsync();
    }

    public async Task<List<ProductBatchDto>> GetPaginatedProductsByWarehouseId(ProductBatchFilter filter)
    {
        return await MakePaginatedQuery(filter)
                .Where(x => x.WarehouseId == filter.Id)
                .MapToProductBatchDto()
                .ToListAsync();
    }

    public async Task<List<ProductBatchDto>> GetPaginatedProductsByStorageUnitId(ProductBatchFilter filter)
    {
        return await MakePaginatedQuery(filter)
                .Where(x => x.StorageUnitId == filter.Id && x.WarehouseId == filter.WarehouseId)
                .MapToProductBatchDto()
                .ToListAsync();
    }
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
    Task<List<ProductBatchDto>> GetPaginatedProductsByProductId(ProductBatchFilter filter);

    /// <summary>
    /// Retrieves a paginated list of products from a warehouse.
    /// </summary>
    Task<List<ProductBatchDto>> GetPaginatedProductsByWarehouseId(ProductBatchFilter filter);

    /// <summary>
    /// Retrieves a paginated list of products from a storage unit.
    /// </summary>
    Task<List<ProductBatchDto>> GetPaginatedProductsByStorageUnitId(ProductBatchFilter filter);

    /// <summary>
    /// Retrieves a specific productbatch by its id.
    /// </summary>
    Task<ProductBatchDto?> GetProductBatchById(Guid id);
}
