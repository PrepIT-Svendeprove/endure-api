using Endure.Data;
using Endure.Data.Models;
using Endure.Dispatcher.Mqtt.Topic.ClimateTelemetry;
using Endure.Dispatcher.RabbitMQ.EventMessage.ClimateTelemetry;
using Endure.Dispatcher.RabbitMQ.Publisher;
using Endure.Service.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Endure.Service.Services.Dispatcher;

internal sealed class DispatcherClimateTelemetryService(
        DatabaseContext context,
        IMessagePublisher messagePublisher,
        IWarehouseService warehouseService
    )
    : BaseDispatcherService<ClimateTelemetry>(context, messagePublisher), IDispatcherClimateTelemetryService
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    private async Task<bool> CreateClimateTelemetryAsync(ClimateTelemetry telemetry, Guid climateDeviceId)
    {
        if (await _context.ClimateTelemetry.AnyAsync(x => x.Id == telemetry.Id && x.WarehouseId == telemetry.WarehouseId && x.ClimateDeviceId == climateDeviceId))
        {
            await SynchronizeWithParent(telemetry.MapToClimateTelemetryCreatedEventMessage());
            return true;
        }

        await _context.AddAsync(telemetry);

        Console.WriteLine("Creating telemetry \t" + JsonSerializer.Serialize(telemetry));

        var result = await _context.SaveChangesAsync() > 0;

        Console.WriteLine("Result \t" + JsonSerializer.Serialize(result));

        if (result)
        {
            await _context
                .ClimateDevice
                .Where(x => x.WareHouseId == telemetry.WarehouseId && x.Id == climateDeviceId)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(y => y.LastReceived, DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                );

            await SynchronizeWithParent(telemetry.MapToClimateTelemetryCreatedEventMessage());
        }

        return result;
    }

    public async Task<bool> CreateClimateTelemetryAsync(ClimateTelemetryCreatedEventMessage message)
    {
        var mappedEntity = message.MapToClimateTelemetry();

        return await CreateClimateTelemetryAsync(mappedEntity, message.ClimateDeviceId);
    }

    public async Task<bool> CreateClimateTelemetryAsync(ClimateTelemetryTopic topic)
    {
        Console.WriteLine("Topic \t " + JsonSerializer.Serialize(topic));
        var rootwarehouseId = await _warehouseService.GetRootWarehouseIdAsync();
        var climateDevice = await _context.ClimateDevice.Select(x => new
        {
            x.Id,
            x.ClimateDeviceCode,
            x.WareHouseId,
            x.IsDisabled,
            x.IsDeleted
        }).FirstOrDefaultAsync(x => x.ClimateDeviceCode == topic.ClimateDeviceCode && x.WareHouseId == rootwarehouseId && !x.IsDeleted && !x.IsDisabled);

        Console.WriteLine("ClimateDevice \t" + JsonSerializer.Serialize(climateDevice));

        if (climateDevice is null)
            return false;

        var mappedEntity = topic.MapToClimateClimateTelemetry(climateDevice.Id, rootwarehouseId);

        return await CreateClimateTelemetryAsync(mappedEntity, climateDevice.Id);
    }
}

public interface IDispatcherClimateTelemetryService
{
    /// <summary>
    /// Creates a new ClimateTelemetry entity, and publishes it with AMQP.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task<bool> CreateClimateTelemetryAsync(ClimateTelemetryCreatedEventMessage message);

    /// <summary>
    /// Creates a new ClimateTelemetry entity, from a topic and publishes it with AMQP. The WarehouseId will always be the root's warehouseId!
    /// </summary>
    Task<bool> CreateClimateTelemetryAsync(ClimateTelemetryTopic topic);
}