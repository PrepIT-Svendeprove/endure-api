namespace Endure.Service.Enums;

/// <summary>
/// Defines the severity level of a log entry.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Indicates an informational event, such as a user updating an entity.
    /// </summary>
    Info = 0,

    /// <summary>
    /// Indicates a non-critical event that may require attention, but does not affect core system functionality.
    /// </summary>
    Warning = 1,

    /// <summary>
    /// Indicates a critical event where an error has caused the system to stop functioning as intended.
    /// </summary>
    Error = 2
}