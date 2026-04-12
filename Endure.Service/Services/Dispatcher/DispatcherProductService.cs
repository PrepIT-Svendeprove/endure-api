using Endure.Data;
using Endure.Data.Models;
using Endure.Dispatcher.EventMessage.Product;
using Endure.Dispatcher.Publisher;
using Endure.Service.Mappers;
using Endure.Service.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services.Dispatcher;

internal class DispatcherProductService(
        DatabaseContext context,
        IMessagePublisher messagePublisher,
        IWarehouseService warehouseService
    )
    : BaseDispatcherService<Product>(
        context,
        messagePublisher
    ), IDispatcherProductService
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    public async Task<bool> CreateProductAsync(Product entity)
    {
        if (await _context.Product.AnyAsync(x => x.Id == entity.Id && x.WarehouseId == entity.WarehouseId))
        {
            await SynchronizeWithParent(entity.MapToProductCreateEventMessage());
            return true;
        }

        await _context.AddAsync(entity);

        var result = await _context.SaveChangesAsync() > 0;

        if (result)
            await SynchronizeWithParent(entity.MapToProductCreateEventMessage());

        return result;
    }

    public async Task<bool> CreateProductAsync(ProductCreateEventMessage entity)
    {
        var mappedEntity = entity.MapToProduct();

        return await CreateProductAsync(mappedEntity);
    }

    public async Task<bool> UpdateProductAsync(Product entity)
    {
        var result = await _context
                .Product
                .Where(x => x.Id == entity.Id && x.WarehouseId == entity.WarehouseId)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(y => y.Name, entity.Name)
                     .SetProperty(y => y.Description, entity.Description)
                     .SetProperty(y => y.EAN, entity.EAN)
                ) > 0;

        await SynchronizeWithParent(entity.MapToProductUpdateEventMessage());

        return result;
    }

    public async Task<bool> UpdateProductAsync(ProductUpdateEventMessage message)
    {
        var mappedEntity = message.MapToProduct();

        return await UpdateProductAsync(mappedEntity);
    }

    public async Task<ServiceResult> DeleteProductAsync(Guid id)
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        var entity = await _context.Product.FirstOrDefaultAsync(x => x.Id == id && x.WarehouseId == rootWarehouseId);

        if (entity is null)
            return ServiceResult.RelationNotFound;

        var result = await SoftDeleteEntity(id, x => x.WarehouseId == rootWarehouseId);

        await SynchronizeWithParent(new ProductDeleteEventMessage
        {
            Id = id,
            WarehouseId = rootWarehouseId,
        });

        return result;
    }

    public async Task<bool> DeleteProductAsync(ProductDeleteEventMessage message)
    {
        var result = await SoftDeleteEntity(message.Id, x => x.WarehouseId == message.WarehouseId) is ServiceResult.Success or ServiceResult.RelationNotFound or ServiceResult.NoChanges;

        await SynchronizeWithParent(message);

        return result;
    }
}

public interface IDispatcherProductService
{
    Task<bool> CreateProductAsync(ProductCreateEventMessage entity);
    Task<bool> CreateProductAsync(Product entity);
    Task<bool> DeleteProductAsync(ProductDeleteEventMessage message);
    Task<ServiceResult> DeleteProductAsync(Guid id);
    Task<bool> UpdateProductAsync(Product entity);
    Task<bool> UpdateProductAsync(ProductUpdateEventMessage message);
}