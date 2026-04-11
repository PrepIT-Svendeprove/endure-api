using System.Text.Json.Serialization;

namespace Endure.Dispatcher.EventMessage;

public abstract class BaseEventMessage
{
    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    [JsonPropertyName("occouredAt")]
    public DateTimeOffset OccouredAt { get; } = DateTimeOffset.UtcNow;
}
