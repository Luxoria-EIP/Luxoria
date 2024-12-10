using Luxoria.Modules.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Luxoria.Modules.Models.Events
{
    [ExcludeFromCodeCoverage]
    public class ImageUpdatedEvent : IEvent
    {
        public string ImagePath { get; }

        public ImageUpdatedEvent(string imagePath)
        {
            if (imagePath == null)
            {
                throw new ArgumentNullException(nameof(imagePath), "ImagePath cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(imagePath))
            {
                throw new ArgumentException("ImagePath cannot be empty or whitespace.", nameof(imagePath));
            }

            ImagePath = imagePath;
        }
    }
}