using Endure.Data.Models;
using Endure.Dispatcher.Mqtt.Topic.ClimateTelemetry;
using Endure.Dispatcher.RabbitMQ.EventMessage.ClimateTelemetry;
using Endure.Service.Models.Dto.ClimateTelemetry;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Endure.Service.Mappers;

internal static class ClimateTelemetryMapper
{
    internal static ClimateTelemetry MapToClimateTelemetry(this ClimateTelemetryCreatedEventMessage message)
    {
        return new ClimateTelemetry
        {
            Id = message.Id,
            Humidity = message.Humidity,
            Temperature = message.Temperature,
            ClimateDeviceId = message.ClimateDeviceId,
            WarehouseId = message.WarehouseId,
            UpdatedAt = message.UpdatedAt,
            CreatedAt = message.CreatedAt,
        };
    }

    internal static ClimateTelemetryCreatedEventMessage MapToClimateTelemetryCreatedEventMessage(this ClimateTelemetry telemetry)
    {
        return new ClimateTelemetryCreatedEventMessage
        {
            Id = telemetry.Id,
            ClimateDeviceId = telemetry.ClimateDeviceId,
            WarehouseId = telemetry.WarehouseId,
            Humidity = telemetry.Humidity,
            Temperature = telemetry.Temperature,
            CreatedAt = telemetry.CreatedAt,
            UpdatedAt = telemetry.UpdatedAt
        };
    }

    internal static ClimateTelemetry MapToClimateClimateTelemetry(this ClimateTelemetryTopic telemetry, Guid climateDeviceId, Guid wareHouseId)
    {
        return new ClimateTelemetry
        {
            ClimateDeviceId = climateDeviceId,
            WarehouseId = wareHouseId,
            Humidity = telemetry.Humidity,
            Temperature = telemetry.Temperature
        };
    }

    internal static IEnumerable<ClimateTelemetryDto> MapToClimateTelemetryDto(this IEnumerable<ClimateTelemetry> list)
    {
        return list.Select(x => new ClimateTelemetryDto
        {
            Id = x.Id,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(x.CreatedAt),
            Humidity = x.Humidity,
            Temperature = x.Temperature
        });
    }

    internal static IQueryable<ClimateTelemetryDto> MapToClimateTelemetryDto(this IQueryable<ClimateTelemetry> query)
    {
        return query.Select(x => new ClimateTelemetryDto
        {
            Id = x.Id,
            Temperature = x.Temperature,
            Humidity = x.Humidity,
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(x.CreatedAt)
        });
    }
}
