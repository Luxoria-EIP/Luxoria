using Xunit;
using Moq;
using Luxoria.SDK.Interfaces;
using Luxoria.SDK.Models;
using Luxoria.SDK.Services;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Luxoria.SDK.Services.Targets;

namespace Luxoria.SDK.Tests
{
    public class LoggerServiceTests
    {
        private readonly LoggerService _loggerService;

        public LoggerServiceTests()
        {
            // Centralized Setup
            _loggerService = new LoggerService(LogLevel.Debug, new DebugLogTarget());
        }

        [Fact]
        public void Log_WithDefaultParameters_ShouldLogInfoLevelAndCategory()
        {
            // Arrange
            string message = "Test message";
            string expectedCategory = "General";
            LogLevel expectedLevel = LogLevel.Info;

            using (var listener = new TestDebugListener())
            {
                Trace.Listeners.Clear();
                Trace.Listeners.Add(listener);

                // Act
                _loggerService.Log(message);

                // Assert
                Assert.Single(listener.LoggedMessages);
                var logEntry = listener.LoggedMessages[0];

                // Check if the log contains the correct level, category, and message
                Assert.Contains($"[{expectedLevel}]", logEntry);
                Assert.Contains(expectedCategory, logEntry);
                Assert.Contains(message, logEntry);

                // Check timestamp presence
                Assert.Contains(DateTime.Now.ToString("yyyy-MM-dd"), logEntry);
            }
        }

        [Fact]
        public void Log_WithCustomCategory_ShouldLogWithCustomCategory()
        {
            // Arrange
            string message = "Test message";
            string customCategory = "CustomCategory";
            LogLevel expectedLevel = LogLevel.Info;

            using (var listener = new TestDebugListener())
            {
                Trace.Listeners.Clear();
                Trace.Listeners.Add(listener);

                // Act
                _loggerService.Log(message, customCategory);

                // Assert
                Assert.Single(listener.LoggedMessages);
                var logEntry = listener.LoggedMessages[0];

                // Check if the log contains the custom category
                Assert.Contains($"[{expectedLevel}]", logEntry);
                Assert.Contains(customCategory, logEntry);
                Assert.Contains(message, logEntry);

                // Check timestamp presence
                Assert.Contains(DateTime.Now.ToString("yyyy-MM-dd"), logEntry);
            }
        }

        [Fact]
        public void Log_WithCustomLogLevel_ShouldLogWithCustomLogLevel()
        {
            // Arrange
            string message = "An error occurred";
            string category = "General";
            LogLevel customLevel = LogLevel.Error;

            using (var listener = new TestDebugListener())
            {
                Trace.Listeners.Clear();
                Trace.Listeners.Add(listener);

                // Act
                _loggerService.Log(message, category, customLevel);

                // Assert
                Assert.Single(listener.LoggedMessages);
                var logEntry = listener.LoggedMessages[0];

                // Check if the log contains the custom log level
                Assert.Contains($"[{customLevel}]", logEntry);
                Assert.Contains(category, logEntry);
                Assert.Contains(message, logEntry);

                // Check timestamp presence
                Assert.Contains(DateTime.Now.ToString("yyyy-MM-dd"), logEntry);
            }
        }

        [Fact]
        public void Log_FormattedMessage_ShouldContainCorrectFormat()
        {
            // Arrange
            string message = "Test message with format";
            string category = "General";
            LogLevel level = LogLevel.Debug;

            using (var listener = new TestDebugListener())
            {
                Trace.Listeners.Clear();
                Trace.Listeners.Add(listener);

                // Act
                _loggerService.Log(message, category, level);

                // Assert
                Assert.Single(listener.LoggedMessages);
                var logEntry = listener.LoggedMessages[0];

                // Check for timestamp
                Assert.Contains(DateTime.Now.ToString("yyyy-MM-dd"), logEntry);
                Assert.Contains(DateTime.Now.ToString("HH:mm:ss"), logEntry);

                // Check for log level and category
                Assert.Contains($"[{level}]", logEntry);
                Assert.Contains(category, logEntry);
                Assert.Contains(message, logEntry);

                // Check if caller info is included
                Assert.Contains("[", logEntry);
                Assert.Contains("]:", logEntry);
            }
        }

        [Fact]
        public void Log_ShouldIncludeCallerInfo()
        {
            // Arrange
            string message = "Test message with caller info";
            string category = "General";
            LogLevel level = LogLevel.Debug;

            // Act
            using (var listener = new TestDebugListener())
            {
                Trace.Listeners.Clear();
                Trace.Listeners.Add(listener);

                // Log with the correct parameters
                _loggerService.Log(message, category, level);

                // Assert that caller information is included (without specific file/line details)
                var logEntry = listener.LoggedMessages[0];
                Assert.Contains("[", logEntry); // Ensures caller info exists
                Assert.Contains("]:", logEntry); // Ensures correct format for caller info
            }
        }

        [Fact]
        public void Log_WithEmptyMessage_ShouldLogEmptyMessage()
        {
            // Arrange
            string message = string.Empty;
            string category = "General";
            LogLevel level = LogLevel.Info;

            using (var listener = new TestDebugListener())
            {
                Trace.Listeners.Clear();
                Trace.Listeners.Add(listener);

                // Act
                _loggerService.Log(message, category, level);

                // Assert
                Assert.Single(listener.LoggedMessages);
                var logEntry = listener.LoggedMessages[0];

                // Check for the correct log level, category, and empty message
                Assert.Contains($"[{level}]", logEntry);
                Assert.Contains(category, logEntry);
                Assert.Contains(message, logEntry); // Expecting an empty message
            }
        }

        [Fact]
        public async Task Log_WriteLog_Asynchronously()
        {
            // Arrange
            string message = "Test message";
            string category = "General";
            LogLevel level = LogLevel.Info;
            using (var listener = new TestDebugListener())
            {
                Trace.Listeners.Clear();
                Trace.Listeners.Add(listener);
                // Act
                await _loggerService.LogAsync(message, category, level);
                // Assert
                Assert.Single(listener.LoggedMessages);
                var logEntry = listener.LoggedMessages[0];
                // Check for the correct log level, category, and message
                Assert.Contains($"[{level}]", logEntry);
                Assert.Contains(category, logEntry);
                Assert.Contains(message, logEntry);
                // Check timestamp presence
                Assert.Contains(DateTime.Now.ToString("yyyy-MM-dd"), logEntry);
            }
        }
    }

    public class TestDebugListener : TraceListener
    {
        public List<string> LoggedMessages { get; } = new List<string>();

        // Override Write method - message cannot be nullable based on base class
        public override void Write(string? message)
        {
            if (message != null) // This check is optional but included for safety
            {
                LoggedMessages.Add(message);
            }
        }

        // Override WriteLine method - message cannot be nullable based on base class
        public override void WriteLine(string? message)
        {
            if (message != null) // This check is optional but included for safety
            {
                LoggedMessages.Add(message);
            }
        }

        // Explicitly hide Dispose method from TraceListener with the 'new' keyword
        public new void Dispose()
        {
            Trace.Listeners.Remove(this);
        }
    }
}
