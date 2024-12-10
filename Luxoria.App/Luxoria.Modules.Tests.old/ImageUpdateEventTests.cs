using Luxoria.Modules.Models.Events;

namespace Luxoria.App.Tests
{
    public class ImageUpdatedEventTests
    {
        [Fact]
        public void Constructor_WithValidImagePath_ShouldSetImagePath()
        {
            // Arrange
            var imagePath = "path/to/image.jpg";

            // Act
            var imageUpdatedEvent = new ImageUpdatedEvent(imagePath);

            // Assert
            Assert.Equal(imagePath, imageUpdatedEvent.ImagePath);
        }

        [Fact]
        public void Constructor_WithNullImagePath_ShouldThrowArgumentNullException()
        {
            // Arrange
            string imagePath = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ImageUpdatedEvent(imagePath));
        }

        [Fact]
        public void Constructor_WithEmptyImagePath_ShouldThrowArgumentException()
        {
            // Arrange
            var imagePath = string.Empty;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ImageUpdatedEvent(imagePath));
        }
    }
}