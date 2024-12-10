using Luxoria.SDK.Interfaces;
using System;
using System.Diagnostics;
using System.IO;

namespace Luxoria.SDK.LogTargets;

/// <summary>
/// A log target that writes logs to a file in an accessible directory.
/// </summary>
public class FileLogTarget : ILogTarget
{
    private readonly string _filePath;

    /// <summary>
    /// Initializes the file log target with a file path.
    /// </summary>
    public FileLogTarget(string fileName)
    {
        // Write to the AppData\Local directory
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string appDirectory = Path.Combine(appDataPath, "Luxoria");

        // Ensure the directory exists
        Directory.CreateDirectory(appDirectory);

        // Set the full file path
        _filePath = Path.Combine(appDirectory, fileName);
    }

    public void WriteLog(string message)
    {
        try
        {
            File.AppendAllText(_filePath, message + Environment.NewLine);
        }
        catch (IOException ex)
        {
            Debug.WriteLine($"File logging failed: {ex.Message}");
        }
    }
}
