using Endure.Service.Models.Enums;
using System.Text.Json.Serialization;

namespace Endure.Service.Models.Results;

public class Result<T> : Result
{
    [JsonPropertyName("entity")]
    public T? UpdatedEntity { get; set; }
}

public class Result
{
    public ServiceResult ServiceResult { get; set; }

    public List<string> StatusCodes { get; set; } = [];

    public static Result Failed(List<string> statusCodes)
    {
        return new Result
        {
            ServiceResult = ServiceResult.Failed,
            StatusCodes = statusCodes
        };
    }

    public static Result Success()
    {
        return new Result
        {
            ServiceResult = ServiceResult.Success
        };
    }
}
