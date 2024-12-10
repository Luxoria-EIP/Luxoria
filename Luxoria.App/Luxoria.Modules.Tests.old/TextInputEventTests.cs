using Luxoria.Modules.Models.Events;

namespace Luxoria.App.Tests
{
    public class TextInputEventTests
    {
        [Fact]
        public void Constructor_ShouldSetText()
        {
            // Arrange
            var text = "Sample text";

            // Act
            var textInputEvent = new TextInputEvent(text);

            // Assert
            Assert.Equal(text, textInputEvent.Text);
        }

        [Fact]
        public void Constructor_WithNullText_ShouldThrowArgumentNullException()
        {
            // Arrange
            string text = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TextInputEvent(text));
        }

        [Fact]
        public void Constructor_WithEmptyText_ShouldThrowArgumentException()
        {
            // Arrange
            string text = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new TextInputEvent(text));
        }
    }
}