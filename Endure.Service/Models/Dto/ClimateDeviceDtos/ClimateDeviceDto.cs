using Endure.Service.Models.Dto.ClimateTelemetry;
using System.Text.Json.Serialization;

namespace Endure.Service.Models.Dto.ClimateDeviceDtos;

public sealed class ClimateDeviceDto
{
    [JsonPropertyName(ClimateDeviceConstants.ID_NAME)]
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the device.
    /// </summary>
    [JsonPropertyName(ClimateDeviceConstants.NAME_NAME)]
    public required string Name { get; set; }

    /// <summary>
    /// DateTime in UTC for when the climatedevice last communicated with the worker.
    /// </summary>
    [JsonPropertyName(ClimateDeviceConstants.LAST_RECEIVED_NAME)]
    public DateTimeOffset? LastReceived { get; set; }

    /// <summary>
    /// Is false, if the climatedevice has not sent any data or a lastwill were requested with the clientid.
    /// </summary>
    [JsonPropertyName(ClimateDeviceConstants.IS_CONNECTED_NAME)]
    public bool IsConnected { get; set; }

    /// <summary>
    /// Defines if the climatedevice is active and can be used.
    /// </summary>
    [JsonPropertyName(ClimateDeviceConstants.IS_DISABLED_NAME)]
    public bool IsDisabled { get; set; }

    [JsonPropertyName(ClimateDeviceConstants.SET_HUMIDITY_NAME)]
    public double? SetHumidity { get; set; }

    [JsonPropertyName(ClimateDeviceConstants.SET_TEMPERATURE_NAME)]
    public double? SetTemperature { get; set; }

    [JsonPropertyName(ClimateDeviceConstants.CLIMATEDEVICECODE_NAME)]
    public string ClimateDeviceCode { get; set; }

    /// <summary>
    /// Defines the storageunit that the climatedevice is used in.
    /// </summary>
    [JsonPropertyName(ClimateDeviceConstants.STORAGEUNITID_NAME)]
    public Guid? StorageUnitId { get; set; }

    /// <summary>
    /// The latest registered climate that were received from the climatedevice.
    /// </summary>
    [JsonPropertyName(ClimateDeviceConstants.LATESTCLIMATE_NAME)]
    public ClimateTelemetryDto? LatestClimate { get; set; }
}
