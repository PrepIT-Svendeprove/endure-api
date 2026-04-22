using Endure.Data;
using Endure.Data.Models;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.AuditLogDtos;
using Endure.Service.Models.Dto.ProductDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Models.StatusCodes;
using Endure.Service.Services.Dispatcher;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Linq.Expressions;

namespace Endure.Service.Services;

internal class ProductService(
        IDispatcherProductService dispatcherProductService,
        IWarehouseService warehouseService,
        IAuditLogService auditLogService,
        DatabaseContext context
    )
    : BaseService<Product>(context), IProductService
{
    private readonly IWarehouseService _warehouseService = warehouseService;
    private readonly IDispatcherProductService _dispatcherProductService = dispatcherProductService;
    private readonly IAuditLogService _auditLogService = auditLogService;

    protected override async Task<ServiceResult> SoftDeleteEntity(Guid id, Expression<Func<Product, bool>>? predicate = null)
    {
        // If the product id is being relied on, we should not be able to remove that product.
        if (await _context.ProductBatch.AnyAsync(x => x.ProductId == id && !x.IsDeleted))
            return ServiceResult.Failed;

        var result = await _dispatcherProductService.DeleteProductAsync(id);

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

    public async Task<Result> CreateProductAsync(CreateProductDto entity)
    {
        // Check if there already exists a product with the same EAN.
        if (await _context.Product.AnyAsync(x => x.EAN == entity.EAN && !x.IsDeleted))
            return Result.Failed([ProductStatusCodes.EAN_EXISTS]);

        var warehouseRootId = await _warehouseService.GetRootWarehouseIdAsync();

        var mappedEntity = entity.MapToProduct();
        mappedEntity.WarehouseId = warehouseRootId;

        var result = await _dispatcherProductService.CreateProductAsync(mappedEntity) ? Result.Success() : Result.Failed([]);

        if (result.ServiceResult is ServiceResult.Success)
        {
            await _auditLogService.CreateAuditLogAsync(new CreateAuditlogDto
            {
                Log = new Log { EntityId = mappedEntity.Id, Entity = entity },
                LogLevel = LogLevel.Info,
                LogType = LogType.Created,
                WarehouseId = warehouseRootId
            }, warehouseRootId);
        }

        return result;
    }

    public async Task<Result> UpdateProductAsync(UpdateProductDto entity)
    {
        var dbEntity = await _context.Product.FirstOrDefaultAsync(x => x.Id == entity.Id && !x.IsDeleted);

        if (dbEntity is null)
            return Result.Failed([ProductStatusCodes.ENTITY_MISSING]);

        var mappedEntity = entity.MapToProduct();
        mappedEntity.WarehouseId = dbEntity.WarehouseId;

        var result = await _dispatcherProductService.UpdateProductAsync(mappedEntity) ? Result.Success() : Result.Failed([]);

        if (result.ServiceResult is ServiceResult.Success)
            await _auditLogService.CreateAuditLogAsync(new CreateAuditlogDto
            {
                Log = new Log { EntityId = mappedEntity.Id, Entity = entity },
                LogLevel = LogLevel.Info,
                LogType = LogType.Updated,
                WarehouseId = mappedEntity.WarehouseId
            }, mappedEntity.WarehouseId);

        return result; 
    }

    public async Task<PaginatedResult<ProductDto>> GetPaginatedProducts(ProductPaginatedFilter filter, Guid warehouseId)
    {
        var context = MakePaginatedQuery(filter)
            .OrderByDescending(x => x.Name)
            .ThenBy(x => x.EAN)
            .Where(x => x.WarehouseId == warehouseId);

        var maxPages = (await context.CountAsync() / filter.Take) + 1;

        return new PaginatedResult<ProductDto>(await context.MapToProductDto().ToListAsync(), maxPages);
    }

    public async Task<ProductDto?> GetProductById(Guid productId, Guid warehouseId)
    {
        return await _context
                .Product
                .Where(x => x.Id == productId && x.WarehouseId == warehouseId && !x.IsDeleted)
                .MapToProductDto()
                .FirstOrDefaultAsync();
    }

    public async Task<List<Top10ProductDto>> GetTop10ProductsInWarehouseId(Guid warehouseId)
    {
        return await _context
                .Product
                .OrderByDescending(x => x.ProductBatches.Count)
                .Where(x => x.WarehouseId == warehouseId)
                .Take(10)
                .MapToTop10ProductDto()
                .ToListAsync();
    }

    public async Task<List<SelectProductDto>> GetAvailableProductsAsync()
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        return await _context
                .Product
                .OrderByDescending(x => x.Name)
                .Where(x => !x.IsDeleted)
                .MapToSelectProductDto()
                .ToListAsync();
    }

    public async Task<int> GetProductCountAsync(Guid warehouseId)
    {
        return await _context
                .Product
                .Where(x => x.WarehouseId == warehouseId && !x.IsDeleted)
                .CountAsync();
    }
}

public interface IProductService : IBaseService
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    Task<Result> CreateProductAsync(CreateProductDto entity);

    /// <summary>
    /// Updates an exisiting product.
    /// </summary>
    Task<Result> UpdateProductAsync(UpdateProductDto entity);

    /// <summary>
    /// Retrieves a paginated list of products.
    /// </summary>
    Task<PaginatedResult<ProductDto>> GetPaginatedProducts(ProductPaginatedFilter filter, Guid warehouseId);

    /// <summary>
    /// Retrives a single product, by ID.
    /// </summary>
    Task<ProductDto?> GetProductById(Guid productId, Guid warehouseId);

    /// <summary>
    /// Retrieves the 10 products with the most product batches.
    /// </summary>
    /// <returns></returns>
    Task<List<Top10ProductDto>> GetTop10ProductsInWarehouseId(Guid warehouseId);
    Task<List<SelectProductDto>> GetAvailableProductsAsync();
    Task<int> GetProductCountAsync(Guid warehouseId);
}
