using Luxoria.Modules.Models.Events;

namespace Luxoria.Modules.Tests.Models.Events
{
    public class OpenCollectionEventTests
    {
        [Fact]
        public void Constructor_ShouldSetPathProperty()
        {
            // Arrange
            var expectedPath = "test/path/to/collection";

            // Act
            var openCollectionEvent = new OpenCollectionEvent("Col#1", expectedPath);

            // Assert
            Assert.Equal(expectedPath, openCollectionEvent.CollectionPath);
        }

        [Fact]
        public void SendProgressMessage_ShouldInvokeProgressMessageEvent_WithMessageAndProgress()
        {
            // Arrange
            var openCollectionEvent = new OpenCollectionEvent("Col#2", "test/path");
            string receivedMessage = String.Empty;
            int? receivedProgress = null;

            openCollectionEvent.ProgressMessage += (message, progress) =>
            {
                receivedMessage = message;
                receivedProgress = progress;
            };

            var expectedMessage = "Processing...";
            var expectedProgress = 50;

            // Act
            openCollectionEvent.SendProgressMessage(expectedMessage, expectedProgress);

            // Assert
            Assert.Equal(expectedMessage, receivedMessage);
            Assert.Equal(expectedProgress, receivedProgress);
        }

        [Fact]
        public void SendProgressMessage_ShouldInvokeProgressMessageEvent_WithMessageOnly()
        {
            // Arrange
            var openCollectionEvent = new OpenCollectionEvent("Col#3", "test/path");
            string receivedMessage = String.Empty;
            int? receivedProgress = null;

            openCollectionEvent.ProgressMessage += (message, progress) =>
            {
                receivedMessage = message;
                receivedProgress = progress;
            };

            var expectedMessage = "Processing without progress";

            // Act
            openCollectionEvent.SendProgressMessage(expectedMessage);

            // Assert
            Assert.Equal(expectedMessage, receivedMessage);
            Assert.Null(receivedProgress);
        }

        [Fact]
        public void SendProgressMessage_ShouldNotThrow_WhenNoSubscribers()
        {
            // Arrange
            var openCollectionEvent = new OpenCollectionEvent("Col#4", "test/path");

            // Act & Assert
            var exception = Record.Exception(() =>
                openCollectionEvent.SendProgressMessage("No subscribers yet", 20));

            Assert.Null(exception);
        }

        [Fact]
        public void Complete_ShouldNotThrow_WhenCalled()
        {
            // Arrange
            var openCollectionEvent = new OpenCollectionEvent("Col#6", "test/path");

            // Act & Assert
            var exception = Record.Exception(() => openCollectionEvent.CompleteSuccessfully());

            Assert.Null(exception);
        }
    }
}
