namespace Endure.Service.Models.Results;

public class BaseDateRangeResult<T>(List<T>? entities)
{
    public DateTimeOffset To { get; set; }

    public DateTimeOffset From { get; set; }

    public List<T> Entities { get; set; }
}
