using System.Security.Claims;

namespace Endure;

public sealed class RequestContext : IRequestContext
{
    /// <summary>
    /// Represents the traceid of the current request.
    /// <para>
    ///     NOTE; The value should not change throughout the request.
    /// </para>
    /// </summary>
    public required string TraceId { get; set; }

    public required ClaimsPrincipal Principal { get; set; }
}

public interface IRequestContext
{
    /// <summary>
    /// Represents the traceid of the current request.
    /// <para>
    ///     NOTE; The value should not change throughout the request.
    /// </para>
    /// </summary>
    string TraceId { get; set; }

    ClaimsPrincipal Principal { get; set; }
}
