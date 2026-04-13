using Endure.Data;
using Endure.Data.Models;
using Endure.Dispatcher.Mqtt.Topic.ClimateDevice;
using Endure.Dispatcher.RabbitMQ.EventMessage.ClimateDevice;
using Endure.Dispatcher.RabbitMQ.Publisher;
using Endure.Service.Mappers;
using Endure.Service.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Endure.Service.Services.Dispatcher;

internal sealed class DispatcherClimateDeviceService(
        DatabaseContext context,
        IMessagePublisher messagePublisher,
        IWarehouseService warehouseService,
        ILogger<IDispatcherClimateDeviceService> loggerService
    )
    : BaseDispatcherService<ClimateDevice>(context, messagePublisher), IDispatcherClimateDeviceService
{
    private readonly IWarehouseService _warehouseService = warehouseService;
    private readonly ILogger<IDispatcherClimateDeviceService> _loggerService = loggerService;

    public async Task<bool> CreateClimateDeviceAsync(ClimateDevice device)
    {
        // If the entity already exists, then there is no reason to try and add it again.
        if (await _context.ClimateDevice.AnyAsync(x => x.Id == device.Id && x.WareHouseId == device.WareHouseId))
        {
            await SynchronizeWithParent(device.MapToClimateDeviceCreatedEventMessage());
            return true;
        }

        await _context.AddAsync(device);

        var result = await _context.SaveChangesAsync() > 0;

        if (result)
            await SynchronizeWithParent(device.MapToClimateDeviceCreatedEventMessage());

        return result;
    }

    public async Task<bool> CreateClimateDeviceAsync(ClimateDeviceCreatedEventMessage message)
    {
        var mappedMessage = message.MapToClimateDevice();

        return await CreateClimateDeviceAsync(mappedMessage);
    }

    public async Task<bool> UpdateClimateDeviceAsync(ClimateDevice device)
    {
        var result = await _context
                .ClimateDevice
                .Where(x => x.Id == device.Id && x.WareHouseId == device.WareHouseId)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(y => y.StorageUnitId, device.StorageUnitId)
                     .SetProperty(y => y.Name, device.Name)
                     .SetProperty(y => y.LastReceived, device.LastReceived)
                     .SetProperty(y => y.IsConnected, device.IsConnected)
                     .SetProperty(y => y.IsDisabled, device.IsDisabled)
                     .SetProperty(y => y.SetTemperature, device.SetTemperature)
                     .SetProperty(y => y.SetHumidity, device.SetHumidity)
                ) > 0;

        await SynchronizeWithParent(device.MapToClimateDeviceUpdatedEventMessage());

        return result;
    }

    public async Task<bool> UpdateClimateDeviceAsync(ClimateDeviceUpdatedEventMessage message)
    {
        var mappedMessage = message.MapToClimateDevice();

        return await UpdateClimateDeviceAsync(mappedMessage);
    }



    public async Task<bool> DisconnectClimateDeviceAsync(Guid climateDeviceId)
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        var entity = await _context.ClimateDevice.FirstOrDefaultAsync(x => x.Id == climateDeviceId && x.WareHouseId == rootWarehouseId);

        if (entity is null)
        {
            _loggerService.LogWarning($"""
                    Could not mark ClimateDevice as Disconnected, because it does not exist.
                        RequestedEntityId: {climateDeviceId}
                """);
            return true;
        }

        entity.IsConnected = false;

        return await UpdateClimateDeviceAsync(entity);
    }

    public async Task<ServiceResult> DeleteClimateDeviceAsync(Guid id)
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        var entity = await _context.ClimateDevice.FirstOrDefaultAsync(x => x.Id == id && x.WareHouseId == rootWarehouseId);

        if (entity is null)
            return ServiceResult.RelationNotFound;

        var result = await SoftDeleteEntity(id, x => x.WareHouseId == rootWarehouseId);

        await SynchronizeWithParent(entity.MapToClimateDeviceDeletedEventMessage());

        return result;
    }

    public async Task<bool> DeleteClimateDeviceAsync(ClimateDeviceDeleteEventMessage message)
    {
        var result = await SoftDeleteEntity(message.Id, x => x.WareHouseId == message.WarehouseId) is ServiceResult.Success or ServiceResult.RelationNotFound or ServiceResult.NoChanges;

        await SynchronizeWithParent(message);

        return result;
    }
}

public interface IDispatcherClimateDeviceService
{
    /// <summary>
    /// Creates a new climate device for the root warehouse.
    /// </summary>
    Task<bool> CreateClimateDeviceAsync(ClimateDevice device);

    /// <summary>
    /// Soft deletes a climatedevice on the root warehouse.
    /// </summary>
    Task<ServiceResult> DeleteClimateDeviceAsync(Guid id);

    /// <summary>
    /// Updates a climatedevice on the root warehouse.
    /// </summary>
    Task<bool> UpdateClimateDeviceAsync(ClimateDevice device);

    Task<bool> CreateClimateDeviceAsync(ClimateDeviceCreatedEventMessage message);
    Task<bool> DeleteClimateDeviceAsync(ClimateDeviceDeleteEventMessage message);
    Task<bool> UpdateClimateDeviceAsync(ClimateDeviceUpdatedEventMessage message);

    /// <summary>
    /// Marks the ClimateDevice as disconnected.
    /// </summary>
    Task<bool> DisconnectClimateDeviceAsync(Guid climateDevice);
}
