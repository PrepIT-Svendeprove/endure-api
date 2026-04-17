using Endure.Data.Models;
using Endure.Dispatcher.RabbitMQ.EventMessage.ClimateDevice;
using Endure.Service.Models.Dto.ClimateDeviceDtos;

namespace Endure.Service.Mappers;

internal static class ClimateDeviceMapper
{
    internal static ClimateDevice MapToClimateDevice(this ClimateDeviceCreatedEventMessage message)
    {
        return new ClimateDevice
        {
            Id = message.Id,
            StorageUnitId = message.StorageUnitId,
            WareHouseId = message.WarehouseId,
            Name = message.Name,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt,
            IsDeleted = false,
            IsConnected = false,
            IsDisabled = false,
            SetTemperature = message.SetTemperature,
            SetHumidity = message.SetHumidity,
        };
    }

    internal static IQueryable<SelectClimateDeviceDto> MapToSelectClimateDeviceDto(this IQueryable<ClimateDevice> query)
    {
        return query.Select(x => new SelectClimateDeviceDto
        {
            Id = x.Id,
            Name = x.Name
        });
    }

    internal static ClimateDevice MapToClimateDevice(this ClimateDeviceDeleteEventMessage message)
    {
        return new ClimateDevice
        {
            Id = message.Id,
            WareHouseId = message.WarehouseId
        };
    }

    internal static ClimateDevice MapToClimateDevice(this ClimateDeviceUpdatedEventMessage message)
    {
        return new ClimateDevice
        {
            Id = message.Id,
            WareHouseId = message.WareHouseId,
            StorageUnitId = message.StorageUnitId,
            Name = message.Name,
            LastReceived = message.LastReceived,
            IsConnected = message.IsConnected,
            IsDisabled = message.IsDisabled,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt
        };
    }

    internal static ClimateDeviceCreatedEventMessage MapToClimateDeviceCreatedEventMessage(this ClimateDevice device)
    {
        return new ClimateDeviceCreatedEventMessage
        {
            Id = device.Id,
            WarehouseId = device.WareHouseId,
            StorageUnitId = device.StorageUnitId,
            Name = device.Name,
            SetHumidity = device.SetHumidity,
            SetTemperature = device.SetTemperature,
            CreatedAt = device.CreatedAt,
            UpdatedAt = device.UpdatedAt,
        };
    }

    internal static ClimateDeviceDeleteEventMessage MapToClimateDeviceDeletedEventMessage(this ClimateDevice device)
    {
        return new ClimateDeviceDeleteEventMessage
        {
            Id = device.Id,
            WarehouseId = device.WareHouseId,
        };
    }

    internal static ClimateDeviceUpdatedEventMessage MapToClimateDeviceUpdatedEventMessage(this ClimateDevice device)
    {
        return new ClimateDeviceUpdatedEventMessage
        {
            Id = device.Id,
            WareHouseId = device.WareHouseId,
            StorageUnitId = device.StorageUnitId,
            Name = device.Name,
            LastReceived = device.LastReceived,
            IsConnected = device.IsConnected,
            IsDisabled = device.IsDisabled,
            SetTemperature = device.SetTemperature,
            SetHumidity = device.SetHumidity,
            CreatedAt = device.CreatedAt,
            UpdatedAt = device.UpdatedAt
        };
    }

    internal static ClimateDevice MapToClimateDevice(this CreateClimateDeviceDto device)
    {
        return new ClimateDevice
        {
            Name = device.Name,
            StorageUnitId = device.StorageUnitId
        };
    }

    internal static ClimateDevice MapToClimateDevice(this UpdateClimateDeviceDto device)
    {
        return new ClimateDevice
        {
            Id = device.Id,
            Name = device.Name,
            StorageUnitId = device.StorageUnitId,
        };
    }

    internal static IQueryable<ClimateDeviceDto> MapToClimateDeviceDto(this IQueryable<ClimateDevice> query)
    {
        return query.Select(x => new ClimateDeviceDto
        {
            Id = x.Id,
            Name = x.Name,
            LastReceived = x.LastReceived == null || x.LastReceived == 0 ? null : DateTimeOffset.FromUnixTimeSeconds((long)x.LastReceived),
            IsConnected = x.IsConnected,
            IsDisabled = x.IsDisabled,
            StorageUnitId = x.StorageUnitId,
            SetHumidity = x.SetHumidity,
            SetTemperature = x.SetTemperature,
            LatestClimate = x.ClimateTelemetry
                .OrderByDescending(x => x.CreatedAt)
                .MapToClimateTelemetryDto()
                .FirstOrDefault()
        });
    }
}
