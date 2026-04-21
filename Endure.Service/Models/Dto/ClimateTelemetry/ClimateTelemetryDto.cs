using System.Text.Json.Serialization;

namespace Endure.Service.Models.Dto.ClimateTelemetry;

public class ClimateTelemetryDto
{
    [JsonPropertyName(ClimateTelemetryConstants.ID_NAME)]
    public required Guid Id { get; set; }

    [JsonPropertyName(ClimateTelemetryConstants.TEMPERATURE_NAME)]
    public required double Temperature { get; set; }

    [JsonPropertyName(ClimateTelemetryConstants.HUMIDITY_NAME)]
    public required double Humidity { get; set; }

    [JsonPropertyName(ClimateTelemetryConstants.CREATEDAT_NAME)]
    public required DateTimeOffset CreatedAt { get; set; }
}
