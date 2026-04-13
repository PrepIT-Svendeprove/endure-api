using Endure.Data;
using Endure.Data.Models;
using Endure.Dispatcher.Mqtt.Topic.ClimateTelemetry;
using Endure.Dispatcher.RabbitMQ.EventMessage.ClimateTelemetry;
using Endure.Dispatcher.RabbitMQ.Publisher;
using Endure.Service.Mappers;
using System.Numerics;

namespace Endure.Service.Services.Dispatcher;

internal sealed class DispatcherClimateTelemetryService(
        DatabaseContext context,
        IMessagePublisher messagePublisher,
        IWarehouseService warehouseService
    )
    : BaseDispatcherService<ClimateTelemetry>(context, messagePublisher), IDispatcherClimateTelemetryService
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    private async Task<bool> CreateClimateTelemetryAsync(ClimateTelemetry telemetry)
    {
        await _context.AddAsync(telemetry);

        var result = await _context.SaveChangesAsync() > 0;

        if (result)
            await SynchronizeWithParent(telemetry.MapToClimateTelemetryCreatedEventMessage());

        return result;
    }

    public async Task<bool> CreateClimateTelemetryAsync(ClimateTelemetryCreatedEventMessage message)
    {
        var mappedEntity = message.MapToClimateTelemetry();

        return await CreateClimateTelemetryAsync(mappedEntity);
    }

    public async Task<bool> CreateClimateTelemetryAsync(ClimateTelemetryTopic topic, Guid clientDeviceId)
    {
        var rootwarehouseId = await _warehouseService.GetRootWarehouseIdAsync();

        var mappedEntity = topic.MapToClimateClimateTelemetry(clientDeviceId, rootwarehouseId);

        return await CreateClimateTelemetryAsync(mappedEntity);
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
    Task<bool> CreateClimateTelemetryAsync(ClimateTelemetryTopic topic, Guid clientDeviceId);
}