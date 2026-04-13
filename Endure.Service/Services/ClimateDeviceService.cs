using Endure.Data;
using Endure.Data.Models;
using Endure.Dispatcher.Mqtt;
using Endure.Dispatcher.Mqtt.Topic.ClimateDevice;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.ClimateDeviceDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Services.Dispatcher;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace Endure.Service.Services;

internal sealed class ClimateDeviceService(
        DatabaseContext context,
        IDispatcherClimateDeviceService dispatcherClimateDeviceService,
        IMqttPublisher mqttPublisher,
        IWarehouseService warehouseService
    )
    : BaseService<ClimateDevice>(context), IClimateDeviceService
{
    private readonly IDispatcherClimateDeviceService _dispatcherClimateDeviceService = dispatcherClimateDeviceService;
    private readonly IMqttPublisher _mqttPublisher = mqttPublisher;
    private readonly IWarehouseService _warehouseService = warehouseService;

    protected override IQueryable<ClimateDevice> MakePaginatedQuery(BasePaginatedFilter filter)
        => base.MakePaginatedQuery(filter).OrderByDescending(x => x.Name);

    protected override Task<ServiceResult> SoftDeleteEntity(Guid id, Expression<Func<ClimateDevice, bool>>? predicate = null)
        => _dispatcherClimateDeviceService.DeleteClimateDeviceAsync(id);

    public async Task<Result> CreateClimateDevice(CreateClimateDeviceDto device)
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        // If the storage unit is specified and it is not a slot, we should not be able to update the climate device to use that storage unit.
        if (device.StorageUnitId.HasValue && !await _context.StorageUnit.AnyAsync(x => x.IsSlot && x.Id == device.StorageUnitId.Value && x.WarehouseId == rootWarehouseId))
            return Result.Failed([]);

        var mappedEntity = device.MapToClimateDevice();
        mappedEntity.WareHouseId = rootWarehouseId;

        var result = await _dispatcherClimateDeviceService.CreateClimateDeviceAsync(mappedEntity) ? Result.Success() : Result.Failed([]);

        if (result.ServiceResult is ServiceResult.Success)
            await _mqttPublisher.PublishAsync(new ClimateRegulateTopic
            {
                Id = mappedEntity.Id,
                Humidity = mappedEntity.SetHumidity,
                Temperature = mappedEntity.SetTemperature
            });

        return result;
    }

    public async Task<Result> UpdateClimateDevice(UpdateClimateDeviceDto device)
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        // If the storage unit is specified and it is not a slot, we should not be able to update the climate device to use that storage unit.
        if (device.StorageUnitId.HasValue && !await _context.StorageUnit.AnyAsync(x => x.IsSlot && x.Id == device.StorageUnitId.Value && x.WarehouseId == rootWarehouseId))
            return Result.Failed([]);

        var mappedEntity = device.MapToClimateDevice();
        mappedEntity.WareHouseId = rootWarehouseId;

        var result = await _dispatcherClimateDeviceService.UpdateClimateDeviceAsync(mappedEntity) ? Result.Success() : Result.Failed([]);

        if (result.ServiceResult is ServiceResult.Success)
            await _mqttPublisher.PublishAsync(new ClimateRegulateTopic
            {
                Id = device.Id,
                Humidity = device.Humidity,
                Temperature = device.Temperature
            });

        return result;
    }

    public async Task<PaginatedResult<ClimateDeviceDto>> GetPaginatedClimateDevice(ClimateDevicePaginatedFilter filter)
    {
        var context = MakePaginatedQuery(filter);

        if (filter.WarehouseId.HasValue)
            context = context.Where(x => x.WareHouseId == filter.WarehouseId);
        else
        {
            var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();
            context = context.Where(x => x.WareHouseId == rootWarehouseId);
        }

        if (filter.ExcludeDisabled)
            context = context.Where(x => !x.IsDisabled);

        var maxPages = await context.CountAsync();

        var entities = await context
                .MapToClimateDeviceDto()
                .ToListAsync();

        return new PaginatedResult<ClimateDeviceDto>(entities, maxPages);
    }
}

public interface IClimateDeviceService : IBaseService
{
    /// <summary>
    /// Creates a new climatedevice, and publishes the desired temp and humidity on the queue for that climate device.
    /// </summary>
    Task<Result> CreateClimateDevice(CreateClimateDeviceDto device);

    /// <summary>
    /// Retrieves a list of ClimateDevices, and the amount of pages that the filter could produce.
    /// </summary>
    Task<PaginatedResult<ClimateDeviceDto>> GetPaginatedClimateDevice(ClimateDevicePaginatedFilter filter);

    /// <summary>
    /// Updates a climate device, and publishes the desired temp and humidity on the queue for that climate device.
    /// </summary>
    Task<Result> UpdateClimateDevice(UpdateClimateDeviceDto device);
}

