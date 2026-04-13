using Endure.Data;
using Endure.Data.Models;
using Endure.Dispatcher.RabbitMQ.EventMessage.Warehouse;
using Endure.Dispatcher.RabbitMQ.Publisher;
using Endure.Service.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Endure.Service.Services.Dispatcher;

internal class DispatcherWarehouseService(
        DatabaseContext context,
        IMessagePublisher messagePublisher
    )
    : BaseDispatcherService<Warehouse>(context, messagePublisher), IDispatcherWarehouseService
{
    public async Task<bool> UpdateWarehouseAsync(Warehouse entity)
    {
        var result = await _context
                .Warehouse
                .Where(x => x.Id == entity.Id)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(y => y.Name, entity.Name)
                     .SetProperty(y => y.ShortName, entity.ShortName)
                     .SetProperty(y => y.ParentId, entity.ParentId)
                ) > 0;

        if (result)
            await SynchronizeWithParent(new WarehouseUpdatedEventMessage
            {
                Id = entity.Id,
                Name = entity.Name,
                ShortName = entity.ShortName,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                ParentId = entity.Id,
            });

        return result;
    }

    public async Task<bool> UpdateWarehouseAsync(WarehouseUpdatedEventMessage message)
    {
        var mappedEntity = message.MapToWarehouse();

        return await UpdateWarehouseAsync(mappedEntity);
    }
}

public interface IDispatcherWarehouseService
{
    Task<bool> UpdateWarehouseAsync(WarehouseUpdatedEventMessage message);
    Task<bool> UpdateWarehouseAsync(Warehouse entity);
}