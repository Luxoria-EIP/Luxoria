using Luxoria.Modules.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Luxoria.Modules.Models.Events
{
    [ExcludeFromCodeCoverage]
    public class TextInputEvent : IEvent
    {
        public string Text { get; }

        public TextInputEvent(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text), "Text cannot be null.");
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Text cannot be empty.", nameof(text));
            }

            Text = text;
        }
    }
}