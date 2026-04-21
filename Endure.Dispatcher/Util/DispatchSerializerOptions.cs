using System.Text.Json;

namespace Endure.Dispatcher.Util;

public class DispatchSerializerOptions
{
    public static JsonSerializerOptions Options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
