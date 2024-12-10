using System.Threading.Tasks;
using Luxoria.SDK.Models;

namespace Luxoria.SDK.Interfaces;

/// <summary>
/// Interface for a logging service.
/// </summary>
public interface ILoggerService
{
    /// <summary>
    /// Logs a message with a specified category, level, and optional caller information.
    /// </summary>
    void Log(
        string message,
        string category = "General",
        LogLevel level = LogLevel.Info,
        string callerName = "",
        string callerFile = "",
        int callerLine = 0);

    /// <summary>
    /// Logs a message asynchronously with a specified category, level, and optional caller information.
    /// </summary>
    Task LogAsync(
        string message,
        string category = "General",
        LogLevel level = LogLevel.Info,
        string callerName = "",
        string callerFile = "",
        int callerLine = 0);
}
