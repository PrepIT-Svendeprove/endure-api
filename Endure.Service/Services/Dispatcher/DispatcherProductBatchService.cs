using Endure.Data;
using Endure.Data.Models;
using Endure.Dispatcher.EventMessage;
using Endure.Dispatcher.EventMessage.Product;
using Endure.Dispatcher.EventMessage.ProductBatch;
using Endure.Dispatcher.Publisher;
using Endure.Service.Mappers;
using Endure.Service.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services.Dispatcher;

internal class DispatcherProductBatchService(
        DatabaseContext context,
        IMessagePublisher messagePublisher,
        IWarehouseService warehouseService
    )
    : BaseDispatcherService<ProductBatch>(context, messagePublisher), IDispatcherProductBatchService
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    public async Task<bool> CreateProductBatchAsync(ProductBatch batch)
    {
        var product = await _context.Product.FirstOrDefaultAsync(x => x.Id == batch.ProductId && x.WarehouseId == batch.WarehouseId);

        if (product is null)
            return false;

        await _context.AddAsync(batch);

        var result = await _context.SaveChangesAsync() > 0;

        if (result)
            await SynchronizeWithParent(
                new ProductBatchCreatedEventMessage
                {
                    Id = batch.Id,
                    Count = batch.Count,
                    BestBefore = batch.BestBefore,
                    ProductId = batch.ProductId,
                    StorageUnitId = batch.StorageUnitId,
                    WarehouseId = batch.WarehouseId,
                    CreatedAt = batch.CreatedAt,
                    UpdatedAt = batch.UpdatedAt,
                    Product = new ProductCreateEventMessage
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Name,
                        Ean = product.EAN,
                        WarehouseId = product.WarehouseId,
                        CreatedAt = product.CreatedAt,
                        UpdatedAt = product.CreatedAt
                    }
                }
            );


        return result;
    }

    public async Task<bool> CreateProductBatchAsync(ProductBatchCreatedEventMessage message)
    {
        if (await _context.ProductBatch.AnyAsync(x => x.Id == message.Id && x.WarehouseId == message.WarehouseId))
        {
            await SynchronizeWithParent(
                message
            );

            return true;
        }

        // Check if the product already exists, if it does not and it has been included in the event message create it.
        if (message.Product is not null && !await _context.Product.AnyAsync(x => x.Id == message.ProductId && x.WarehouseId == message.WarehouseId))
        {
            var mappedProductentity = message.Product.MapToProduct();

            await _context.AddAsync(mappedProductentity);

            await _context.SaveChangesAsync();
        }

        var mapppedEntity = message.MapToProductBatch();

        return await CreateProductBatchAsync(mapppedEntity);
    }

    public async Task<ServiceResult> DeleteProductBatchAsync(Guid id)
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        // Check if the given entity is a part of the root warehouse.
        var exists = await _context.ProductBatch.FirstOrDefaultAsync(x => x.Id == id && x.WarehouseId == rootWarehouseId && x.Warehouse.IsRoot);

        // If it is not a part of the root warehouse we should not be able to remove it.
        if (exists is null)
            return ServiceResult.RelationNotFound;

        var result = await SoftDeleteEntity(id, x => x.WarehouseId == rootWarehouseId);

        if (result is ServiceResult.Success)
            await SynchronizeWithParent(
                new ProductBatchDeletedEventMessage
                {
                    Id = id,
                    WarehouseId = rootWarehouseId,
                    CreatedAt = exists.CreatedAt,
                    UpdatedAt = exists.UpdatedAt
                }
            );

        return result;
    }

    public async Task<bool> DeleteProductBatchAsync(ProductBatchDeletedEventMessage message)
    {
        var result = await SoftDeleteEntity(message.Id, x => x.WarehouseId == message.WarehouseId) is ServiceResult.Success or ServiceResult.RelationNotFound or ServiceResult.NoChanges;

        if (result)
            await SynchronizeWithParent(
                message
            );

        return result;
    }
}

public interface IDispatcherProductBatchService
{
    /// <summary>
    /// Creates a new product batch, and synchronizes with the root parent if it is set.
    /// </summary>
    Task<bool> CreateProductBatchAsync(ProductBatch batch);

    /// <summary>
    /// Creates a new product batch, and synchronizes with the root parent if it is set.
    /// </summary>
    Task<bool> CreateProductBatchAsync(ProductBatchCreatedEventMessage message);

    /// <summary>
    /// Soft deletes a product batch from the root warehouse, and synchronizes the data with the root warehouses parent if it is set.
    /// </summary>
    Task<ServiceResult> DeleteProductBatchAsync(Guid id);

    /// <summary>
    /// Soft deletes a product batch, and synchronizes the data with the root warehouses parent if it is set.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task<bool> DeleteProductBatchAsync(ProductBatchDeletedEventMessage message);
}