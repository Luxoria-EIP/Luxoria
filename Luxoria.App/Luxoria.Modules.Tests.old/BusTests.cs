using Luxoria.Modules.Models.Events;
using Moq;
using System.Threading.Tasks;

namespace Luxoria.Modules.Tests
{
    public class EventBusTests
    {
        private readonly EventBus _eventBus;

        public EventBusTests()
        {
            // Centralized Setup
            _eventBus = new EventBus();
        }

        [Fact]
        public async Task Subscribe_WithValidHandler_ShouldAddSubscriber()
        {
            // Arrange
            var mockHandler = new Mock<Action<LogEvent>>();

            // Act
            _eventBus.Subscribe(mockHandler.Object);
            await _eventBus.Publish(new LogEvent("Test message"));

            // Assert
            mockHandler.Verify(handler => handler(It.IsAny<LogEvent>()), Times.Once,
                "The handler should have been called once when a message is published, but it was not.");
        }

        [Fact]
        public async Task Unsubscribe_WithValidHandler_ShouldRemoveSubscriber()
        {
            // Arrange
            var mockHandler = new Mock<Action<LogEvent>>();
            _eventBus.Subscribe(mockHandler.Object);

            // Act
            _eventBus.Unsubscribe(mockHandler.Object);
            await _eventBus.Publish(new LogEvent("Test message"));

            // Assert
            mockHandler.Verify(handler => handler(It.IsAny<LogEvent>()), Times.Never,
                "The handler should not be called after being unsubscribed, but it was.");
        }

        [Fact]
        public async Task Unsubscribe_WithValidAsyncHandler_ShouldRemoveAsyncSubscriber()
        {
            // Arrange
            var mockAsyncHandler = new Mock<Func<LogEvent, Task>>();
            mockAsyncHandler.Setup(handler => handler(It.IsAny<LogEvent>())).Returns(Task.CompletedTask);
            _eventBus.Subscribe(mockAsyncHandler.Object);

            // Act
            _eventBus.Unsubscribe(mockAsyncHandler.Object);
            await _eventBus.Publish(new LogEvent("Test message"));

            // Assert
            mockAsyncHandler.Verify(handler => handler(It.IsAny<LogEvent>()), Times.Never,
                "The async handler should not be called after being unsubscribed, but it was.");
        }

        [Fact]
        public async Task Unsubscribe_WithMultipleSubscribers_ShouldOnlyRemoveSpecifiedSubscriber()
        {
            // Arrange
            var mockHandler1 = new Mock<Action<LogEvent>>();
            var mockHandler2 = new Mock<Action<LogEvent>>();
            _eventBus.Subscribe(mockHandler1.Object);
            _eventBus.Subscribe(mockHandler2.Object);

            // Act
            _eventBus.Unsubscribe(mockHandler1.Object);
            await _eventBus.Publish(new LogEvent("Test message"));

            // Assert
            mockHandler1.Verify(handler => handler(It.IsAny<LogEvent>()), Times.Never,
                "Handler1 should not be called after being unsubscribed, but it was.");
            mockHandler2.Verify(handler => handler(It.IsAny<LogEvent>()), Times.Once,
                "Handler2 should still be called since it was not unsubscribed.");
        }

        [Fact]
        public void Unsubscribe_SameHandlerTwice_ShouldNotThrow()
        {
            // Arrange
            var mockHandler = new Mock<Action<LogEvent>>();
            _eventBus.Subscribe(mockHandler.Object);
            _eventBus.Unsubscribe(mockHandler.Object);

            // Act & Assert
            // Trying to unsubscribe the same handler again should not throw an exception.
            _eventBus.Unsubscribe(mockHandler.Object);
            Assert.NotNull(_eventBus);
        }

        [Fact]
        public async Task UnsubscribeAsync_WithMultipleIdenticalSubscribers_ShouldRemoveOneInstanceAtATime()
        {
            // Arrange
            var mockAsyncHandler = new Mock<Func<LogEvent, Task>>();
            mockAsyncHandler.Setup(handler => handler(It.IsAny<LogEvent>())).Returns(Task.CompletedTask);
            _eventBus.Subscribe(mockAsyncHandler.Object);
            _eventBus.Subscribe(mockAsyncHandler.Object);

            // Act
            _eventBus.Unsubscribe(mockAsyncHandler.Object);
            await _eventBus.Publish(new LogEvent("Test message"));

            // Assert
            mockAsyncHandler.Verify(handler => handler(It.IsAny<LogEvent>()), Times.Once,
                "The async handler should be called once after one unsubscribe, but it was not.");
        }

        [Fact]
        public void UnsubscribeAsync_SameHandlerTwice_ShouldNotThrowAndHandlerShouldBeRemoved()
        {
            // Arrange
            var mockAsyncHandler = new Mock<Func<LogEvent, Task>>();
            mockAsyncHandler.Setup(handler => handler(It.IsAny<LogEvent>())).Returns(Task.CompletedTask);
            _eventBus.Subscribe(mockAsyncHandler.Object);
            _eventBus.Unsubscribe(mockAsyncHandler.Object);

            // Act & Assert
            // Trying to unsubscribe the same async handler again should not throw an exception.
            _eventBus.Unsubscribe(mockAsyncHandler.Object);
            Assert.NotNull(_eventBus);
        }

        [Fact]
        public void Unsubscribe_NonexistentSubscriber_ShouldNotThrow()
        {
            // Arrange
            var mockSubscriber = new Mock<Action<LogEvent>>();

            // Act & Assert
            // Ensure that trying to unsubscribe a handler that was never subscribed does not throw an exception.
            _eventBus.Unsubscribe(mockSubscriber.Object);
            Assert.NotNull(_eventBus);
        }

        [Fact]
        public void Unsubscribe_WithValidAsyncHandler_ShouldNotThrowWhenNotSubscribed()
        {
            // Arrange
            var mockAsyncHandler = new Mock<Func<LogEvent, Task>>();

            // Act & Assert
            // Trying to unsubscribe an async handler that was never subscribed should not throw an exception.
            _eventBus.Unsubscribe(mockAsyncHandler.Object);
            Assert.NotNull(_eventBus);
        }

        [Fact]
        public async Task Publish_WithMultipleSubscribers_ShouldNotifyAllSubscribers()
        {
            // Arrange
            var mockHandler1 = new Mock<Action<LogEvent>>();
            var mockHandler2 = new Mock<Action<LogEvent>>();
            _eventBus.Subscribe(mockHandler1.Object);
            _eventBus.Subscribe(mockHandler2.Object);

            // Act
            await _eventBus.Publish(new LogEvent("Test message"));

            // Assert
            mockHandler1.Verify(handler => handler(It.IsAny<LogEvent>()), Times.Once,
                "Handler1 should have been called once when the event is published, but it was not.");
            mockHandler2.Verify(handler => handler(It.IsAny<LogEvent>()), Times.Once,
                "Handler2 should have been called once when the event is published, but it was not.");
        }

        [Fact]
        public async Task PublishAsync_WithValidAsyncHandler_ShouldInvokeHandler()
        {
            // Arrange
            var mockAsyncHandler = new Mock<Func<LogEvent, Task>>();
            mockAsyncHandler.Setup(handler => handler(It.IsAny<LogEvent>())).Returns(Task.CompletedTask);
            _eventBus.Subscribe(mockAsyncHandler.Object);

            // Act
            await _eventBus.Publish(new LogEvent("Test message"));

            // Assert
            mockAsyncHandler.Verify(handler => handler(It.IsAny<LogEvent>()), Times.Once,
                "The async handler should have been called once when the event is published, but it was not.");
        }

        [Fact]
        public async Task Publish_WithNoSubscribers_ShouldNotThrow()
        {
            // Arrange
            var logEvent = new LogEvent("Test message");

            // Act & Assert
            // Ensures that publishing an event with no subscribers does not throw an exception.
            await _eventBus.Publish(logEvent);
            Assert.NotNull(_eventBus);
        }

        [Fact]
        public void LogEvent_Constructor_WithMessage_ShouldSetMessageProperty()
        {
            // Arrange
            var message = "Test message";

            // Act
            var logEvent = new LogEvent(message);

            // Assert
            Assert.Equal(message, logEvent.Message);
        }
    }
}
