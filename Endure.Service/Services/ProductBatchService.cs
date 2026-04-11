using Endure.Data;
using Endure.Data.Models;
using Endure.Dispatcher.EventMessage.ProductBatch;
using Endure.Dispatcher.Publisher;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.ProductBatchDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Models.StatusCodes;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Endure.Service.Services;

internal class ProductBatchService(
        IWarehouseService warehouseService,
        IMessagePublisher messagePublisher,
        DatabaseContext context
    )
    : BaseService<ProductBatch>(context), IProductBatchService
{
    private readonly IWarehouseService _warehouseService = warehouseService;
    private readonly IMessagePublisher _messagePublisher = messagePublisher;

    protected override IQueryable<ProductBatch> MakePaginatedQuery(BasePaginatedFilter filter)
        => base.MakePaginatedQuery(filter).OrderByDescending(x => x.BestBefore);

    protected override async Task<ServiceResult> SoftDeleteEntity(Guid id, Expression<Func<ProductBatch, bool>>? prediate = null)
    {
        // Ensure that we are only trying to delete the entity from the root warehouse, as it should not be possible to remove it from sub-warehouses from the root warehouse.
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        var result = await base.SoftDeleteEntity(id, x => x.WarehouseId == rootWarehouseId);

        // Synchronize with the root's parent, if it exists.
        if (result is ServiceResult.Success && await ShouldSynchronizeWithParent(rootWarehouseId))
            await _messagePublisher.PublishAsync(new ProductBatchDeletedEventMessage
            {
                Id = id,
                IsSoftDeleted = true,
                WarehouseId = rootWarehouseId
            });

        return result;
    }

    public async Task<Result> CreateProductBatch(CreateProductBatchDto productBatch)
    {
        if (await IsDeleted<Warehouse>(productBatch.WarehouseId) || await IsDeleted<StorageUnit>(productBatch.StorageUnitId) || await IsDeleted<Product>(productBatch.ProductId))
            return Result.Failed([ProductBatchStatusCodes.RELATION_DOES_NOT_EXIST]);

        var mapppedEntity = productBatch.MapToProductBatch();

        await _context.AddAsync(mapppedEntity);

        var result = await _context.SaveChangesAsync() > 0 ? Result.Success() : Result.Failed([ProductBatchStatusCodes.ENTITY_UNCHANGED]);

        // If the requested warehouse is the root warehouse, we should try and synchronize with its root warehouse if it has one.
        if (result is { ServiceResult: ServiceResult.Success } && await ShouldSynchronizeWithParent(productBatch.WarehouseId))
            await _messagePublisher.PublishAsync(new ProductBatchCreatedEventMessage
            {
                Id = mapppedEntity.Id,
                Count = productBatch.Count,
                BestBefore = productBatch.BestBefore,
                ProductId = productBatch.ProductId,
                StorageUnitId = productBatch.StorageUnitId,
                WarehouseId = productBatch.WarehouseId,
            });

        return result;
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
