using Luxoria.SDK.Interfaces;
using System.Diagnostics;

namespace Luxoria.SDK.Services.Targets;

/// <summary>
/// A log target that writes logs to the Debug console.
/// </summary>
public class DebugLogTarget : ILogTarget
{
    public void WriteLog(string message)
    {
        Trace.WriteLine(message);
    }
}