using Luxoria.Modules.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Luxoria.Modules.Models.Events
{
    [ExcludeFromCodeCoverage]
    public class LogEvent : IEvent
    {
        public string Message { get; }

        public LogEvent(string message)
        {
            Message = message;
        }
    }
}