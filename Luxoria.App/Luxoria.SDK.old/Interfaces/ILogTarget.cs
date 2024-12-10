namespace Luxoria.SDK.Interfaces;

/// <summary>
/// Interface for log targets (e.g., Debug console, file, etc.).
/// </summary>
public interface ILogTarget
{
    /// <summary>
    /// Writes a log message to the target.
    /// </summary>
    void WriteLog(string message);
}
