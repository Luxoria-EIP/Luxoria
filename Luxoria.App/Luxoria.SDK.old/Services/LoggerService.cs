using Luxoria.SDK.Interfaces;
using Luxoria.SDK.Models;

namespace Luxoria.SDK.Services
{
    /// <summary>
    /// Provides logging functionality with support for multiple log targets and caller information.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly List<ILogTarget> _logTargets = new();
        private readonly LogLevel _minLogLevel;

        /// <summary>
        /// Initializes the logger with a minimum log level and log targets.
        /// </summary>
        /// <param name="minLogLevel">The minimum log level for filtering logs.</param>
        /// <param name="targets">The log targets to write logs to.</param>
        public LoggerService(LogLevel minLogLevel, params ILogTarget[] targets)
        {
            _minLogLevel = minLogLevel;
            _logTargets.AddRange(targets);
        }

        public void Log(
            string message,
            string category = "General",
            LogLevel level = LogLevel.Info,
            string callerName = "",
            string callerFile = "",
            int callerLine = 0)
        {
            if (level < _minLogLevel) return;

            string formattedMessage = FormatLog(message, category, level, callerName, callerFile, callerLine);
            foreach (var target in _logTargets)
            {
                target.WriteLog(formattedMessage);
            }
        }

        public async Task LogAsync(
            string message,
            string category = "General",
            LogLevel level = LogLevel.Info,
            string callerName = "",
            string callerFile = "",
            int callerLine = 0)
        {
            if (level < _minLogLevel) return;

            string formattedMessage = FormatLog(message, category, level, callerName, callerFile, callerLine);
            foreach (var target in _logTargets)
            {
                await Task.Run(() => target.WriteLog(formattedMessage));
            }
        }

        /// <summary>
        /// Formats the log message with caller information.
        /// </summary>
        private static string FormatLog(
            string message,
            string category,
            LogLevel level,
            string callerName,
            string callerFile,
            int callerLine)
        {
            string fileName = Path.GetFileName(callerFile);
            return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {category} " +
                   $"[{fileName}:{callerLine} {callerName}]: {message}";
        }
    }
}
