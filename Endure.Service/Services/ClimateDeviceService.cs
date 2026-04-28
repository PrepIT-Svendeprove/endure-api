using Endure.Data;
using Endure.Data.Migrations;
using Endure.Data.Models;
using Endure.Dispatcher.Mqtt;
using Endure.Dispatcher.Mqtt.Topic.ClimateDevice;
using Endure.Service.Mappers;
using Endure.Service.Models.Dto.AuditLogDtos;
using Endure.Service.Models.Dto.ClimateDeviceDtos;
using Endure.Service.Models.Enums;
using Endure.Service.Models.Filters;
using Endure.Service.Models.Results;
using Endure.Service.Models.StatusCodes;
using Endure.Service.Services.Dispatcher;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Endure.Service.Services;

internal sealed class ClimateDeviceService(
        DatabaseContext context,
        IDispatcherClimateDeviceService dispatcherClimateDeviceService,
        IMqttPublisher mqttPublisher,
        IWarehouseService warehouseService,
        IAuditLogService auditLogService
    )
    : BaseService<ClimateDevice>(context), IClimateDeviceService
{
    private readonly IDispatcherClimateDeviceService _dispatcherClimateDeviceService = dispatcherClimateDeviceService;
    private readonly IMqttPublisher _mqttPublisher = mqttPublisher;
    private readonly IWarehouseService _warehouseService = warehouseService;
    private readonly IAuditLogService _auditLogService = auditLogService;

    protected override IQueryable<ClimateDevice> MakePaginatedQuery(BasePaginatedFilter filter)
        => base.MakePaginatedQuery(filter).OrderByDescending(x => x.Name);

    protected override async Task<ServiceResult> SoftDeleteEntity(Guid id, Expression<Func<ClimateDevice, bool>>? predicate = null)
    {
        var result = await _dispatcherClimateDeviceService.DeleteClimateDeviceAsync(id);

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

    public async Task<Result> CreateClimateDevice(CreateClimateDeviceDto device)
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        // If the storage unit is specified, but it does not exist we should not be able to add it.
        if (device.StorageUnitId.HasValue && !await _context.StorageUnit.AnyAsync(x => x.Id == device.StorageUnitId.Value && x.WarehouseId == rootWarehouseId))
            return Result.Failed([ClimateDeviceStatusCodes.RELATION_DOES_NOT_EXIST]);

        var mappedEntity = device.MapToClimateDevice();
        mappedEntity.WareHouseId = rootWarehouseId;

        using (var rng = RandomNumberGenerator.Create())
        {
            var bytes = new byte[2];
            rng.GetBytes(bytes);
            mappedEntity.ClimateDeviceCode = Convert.ToHexString(bytes);
        }

        var result = await _dispatcherClimateDeviceService.CreateClimateDeviceAsync(mappedEntity) ? Result.Success() : Result.Failed([]);

        if (result.ServiceResult is ServiceResult.Success)
        {
            var climateDevice = await _context.ClimateDevice.Select(x => new { x.WareHouseId, x.Id, x.ClimateDeviceCode }).FirstOrDefaultAsync(x => x.WareHouseId == rootWarehouseId && x.Id == mappedEntity.Id);

            await _mqttPublisher.PublishAsync(new ClimateRegulateTopic
            {
                ClimateDeviceCode = climateDevice!.ClimateDeviceCode,
                Humidity = mappedEntity.SetHumidity,
                Temperature = mappedEntity.SetTemperature
            });

            await _auditLogService.CreateAuditLogAsync(new CreateAuditlogDto
            {
                Log = new Log { 
                    EntityId = mappedEntity.Id, 
                    Entity = device 
                },
                LogLevel = LogLevel.Info,
                LogType = LogType.Created,
                WarehouseId = rootWarehouseId
            }, rootWarehouseId);
        }

        return result;
    }

    public async Task<Result> UpdateClimateDevice(UpdateClimateDeviceDto device)
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        if (device.StorageUnitId.HasValue && !await _context.StorageUnit.AnyAsync(x => x.Id == device.StorageUnitId.Value && x.WarehouseId == rootWarehouseId && !x.IsDeleted))
            return Result.Failed([]);

        var mappedEntity = device.MapToClimateDevice();
        mappedEntity.WareHouseId = rootWarehouseId;

        var result = await _dispatcherClimateDeviceService.UpdateClimateDeviceAsync(mappedEntity) ? Result.Success() : Result.Failed([]);

        if (result.ServiceResult is ServiceResult.Success && !mappedEntity.IsDisabled)
        {
            var climateDevice = await _context.ClimateDevice.Select(x => new { x.WareHouseId, x.Id, x.ClimateDeviceCode }).FirstOrDefaultAsync(x => x.WareHouseId == rootWarehouseId && x.Id == device.Id);

            await _mqttPublisher.PublishAsync(new ClimateRegulateTopic
            {
                ClimateDeviceCode = climateDevice!.ClimateDeviceCode,
                Humidity = device.SetHumidity,
                Temperature = device.SetTemperature
            });

            await _auditLogService.CreateAuditLogAsync(new CreateAuditlogDto
            {
                Log = new Log { 
                    EntityId = mappedEntity.Id, 
                    Entity = device
                },
                LogLevel = LogLevel.Info,
                LogType = LogType.Updated,
                WarehouseId = rootWarehouseId
            }, rootWarehouseId);
        }

        return result;
    }

    public async Task<Result> UpdateStorageUnitOnClimateDevice(Guid climateDeviceId, Guid storageUnitId)
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        if (!await _context.StorageUnit.AnyAsync(x => x.Id == storageUnitId && x.WarehouseId == rootWarehouseId && !x.IsDeleted))
            return Result.Failed([ClimateDeviceStatusCodes.RELATION_DOES_NOT_EXIST]);

        var dbEntity = await _context.ClimateDevice.FirstOrDefaultAsync(x => x.Id == climateDeviceId && x.WareHouseId == rootWarehouseId && !x.IsDeleted);

        if (dbEntity is null)
            return Result.Failed([ClimateDeviceStatusCodes.ENTITY_MISSING]);

        dbEntity.StorageUnitId = storageUnitId;

        await _auditLogService.CreateAuditLogAsync(new CreateAuditlogDto
        {
            Log = new Log { 
                EntityId = dbEntity.Id, 
                Entity = new
                {
                    dbEntity.StorageUnitId
                }
            },
            LogLevel = LogLevel.Info,
            LogType = LogType.Updated,
            WarehouseId = rootWarehouseId
        }, rootWarehouseId);

        return await _dispatcherClimateDeviceService.UpdateClimateDeviceAsync(dbEntity) ? Result.Success() : Result.Failed([]);
    }

    public async Task<List<SelectClimateDeviceDto>> GetSelectClimateDeviceDtoAsync()
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        return await _context
                .ClimateDevice
                .Where(x => x.WareHouseId == rootWarehouseId && x.StorageUnitId == null && !x.IsDeleted)
                .MapToSelectClimateDeviceDto()
                .ToListAsync();
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

    public async Task<List<ClimateDeviceDto>> GetClimateDevicesByStorageUnitId(Guid warehouseId, Guid storageUnitId)
    {
        return await _context
                .ClimateDevice
                .Where(x => x.WareHouseId == warehouseId && x.StorageUnitId == storageUnitId && !x.IsDeleted)
                .MapToClimateDeviceDto()
                .ToListAsync();
    }

    public async Task<List<ClimateDeviceDto>> GetAvailableClimateDevices()
    {
        var rootWarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        return await _context
                .ClimateDevice
                .Where(x => x.WareHouseId == rootWarehouseId && !x.IsDeleted && (x.StorageUnit == null || x.StorageUnit.IsDeleted))
                .MapToClimateDeviceDto()
                .ToListAsync();
    }

    public async Task<ClimateDeviceDto?> GetClimateDeviceAsync(Guid warehouseId, Guid climateDeviceId)
    {
        return await _context
                .ClimateDevice
                .Where(x => x.WareHouseId == warehouseId && x.Id == climateDeviceId && !x.IsDeleted)
                .MapToClimateDeviceDto()
                .FirstOrDefaultAsync();
    }

    public async Task<int> GetClimateDeviceCountAsync(Guid warehouseId)
    {
        return await _context
                .ClimateDevice
                .Where(x => x.WareHouseId == warehouseId && !x.IsDeleted)
                .CountAsync();
    }
}

public interface IClimateDeviceService : IBaseService
{
    /// <summary>
    /// Creates a new climatedevice, and publishes the desired temp and humidity on the queue for that climate device.
    /// </summary>
    Task<Result> CreateClimateDevice(CreateClimateDeviceDto device);

    /// <summary>
    /// Retrieves all climates that are available on the root warehouse, to be added into a storage unit.
    /// </summary>
    Task<List<ClimateDeviceDto>> GetAvailableClimateDevices();
    Task<ClimateDeviceDto?> GetClimateDeviceAsync(Guid warehouseId, Guid climateDeviceId);
    Task<int> GetClimateDeviceCountAsync(Guid warehouseId);
    Task<List<ClimateDeviceDto>> GetClimateDevicesByStorageUnitId(Guid warehouseId, Guid storageUnitId);

    /// <summary>
    /// Retrieves a list of ClimateDevices, and the amount of pages that the filter could produce.
    /// </summary>
    Task<PaginatedResult<ClimateDeviceDto>> GetPaginatedClimateDevice(ClimateDevicePaginatedFilter filter);
    Task<List<SelectClimateDeviceDto>> GetSelectClimateDeviceDtoAsync();

    /// <summary>
    /// Updates a climate device, and publishes the desired temp and humidity on the queue for that climate device.
    /// </summary>
    Task<Result> UpdateClimateDevice(UpdateClimateDeviceDto device);
    Task<Result> UpdateStorageUnitOnClimateDevice(Guid climateDeviceId, Guid storageUnitId);
}

