using Luxoria.Modules;
using Luxoria.Modules.Interfaces;
using Luxoria.SDK.Interfaces;
using Moq;
using System.Reflection;

namespace Luxoria.App.Tests
{
    public class ModuleLoaderTests
    {
        private ModuleLoader _moduleLoader;
        private readonly Mock<Func<string, bool>> _fileExistsMock;
        private readonly Mock<Func<string, Assembly>> _assemblyLoadFromMock;
        private readonly string _modulePath;

        public ModuleLoaderTests()
        {
            _fileExistsMock = new Mock<Func<string, bool>>();
            _assemblyLoadFromMock = new Mock<Func<string, Assembly>>();
            _modulePath = "fake/path/to/module.dll";
        }

        [Fact]
        public void Constructor_ShouldUseDefaults_WhenNoParametersProvided()
        {
            // Act
            _moduleLoader = new ModuleLoader();

            // Assert
            Assert.NotNull(_moduleLoader);
        }

        [Fact]
        public void LoadModule_ShouldThrowArgumentNullException_WhenPathIsNull()
        {
            // Arrange
            _moduleLoader = new ModuleLoader(_fileExistsMock.Object, _assemblyLoadFromMock.Object);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _moduleLoader.LoadModule(null));
            Assert.Contains("path", exception.Message);
        }

        [Fact]
        public void LoadModule_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
        {
            // Arrange
            _fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(false);
            _moduleLoader = new ModuleLoader(_fileExistsMock.Object, _assemblyLoadFromMock.Object);

            // Act & Assert
            var exception = Assert.Throws<FileNotFoundException>(() => _moduleLoader.LoadModule(_modulePath));
            Assert.Equal($"Module not found: [{_modulePath}]", exception.Message);
        }

        [Fact]
        public void LoadModule_ShouldThrowInvalidOperationException_WhenNoValidModuleInAssembly()
        {
            // Arrange
            _fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(true);
            var mockAssembly = new Mock<Assembly>();
            mockAssembly.Setup(a => a.GetTypes()).Returns(Array.Empty<Type>());
            _assemblyLoadFromMock.Setup(a => a(It.IsAny<string>())).Returns(mockAssembly.Object);
            _moduleLoader = new ModuleLoader(_fileExistsMock.Object, _assemblyLoadFromMock.Object);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _moduleLoader.LoadModule(_modulePath));
            Assert.Equal("No valid module found in assembly.", exception.Message);
        }

        [Fact]
        public void LoadModule_ShouldReturnFirstValidModule_WhenAssemblyHasMixedTypes()
        {
            // Arrange
            _fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(true);

            var mockAssembly = new Mock<Assembly>();
            mockAssembly.Setup(a => a.GetTypes()).Returns(new Type[]
                { typeof(string), typeof(AbstractModule), typeof(TestModule) });
            _assemblyLoadFromMock.Setup(a => a(It.IsAny<string>())).Returns(mockAssembly.Object);

            _moduleLoader = new ModuleLoader(_fileExistsMock.Object, _assemblyLoadFromMock.Object);

            // Act
            var result = _moduleLoader.LoadModule(_modulePath);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TestModule>(result);
            Assert.Equal("Test Module", result.Name);
            Assert.Equal("A test module.", result.Description);
            Assert.Equal("1.0.0", result.Version);
        }

        [Fact]
        public void LoadModule_ShouldThrowFileLoadException_WhenAssemblyCannotBeLoaded()
        {
            // Arrange
            _fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(true);
            _assemblyLoadFromMock.Setup(a => a(It.IsAny<string>())).Throws<FileLoadException>();
            _moduleLoader = new ModuleLoader(_fileExistsMock.Object, _assemblyLoadFromMock.Object);

            // Act & Assert
            var exception = Assert.Throws<FileLoadException>(() => _moduleLoader.LoadModule(_modulePath));
            Assert.Equal($"Could not load assembly: {_modulePath}", exception.Message);
        }

        [Fact]
        public void LoadModule_ShouldReturnValidModule_WhenValidModuleTypeIsFound()
        {
            // Arrange
            _fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(true);
            var mockAssembly = new Mock<Assembly>();
            mockAssembly.Setup(a => a.GetTypes()).Returns(new Type[] { typeof(TestModule) });
            _assemblyLoadFromMock.Setup(a => a(It.IsAny<string>())).Returns(mockAssembly.Object);
            _moduleLoader = new ModuleLoader(_fileExistsMock.Object, _assemblyLoadFromMock.Object);

            // Act
            var result = _moduleLoader.LoadModule(_modulePath);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TestModule>(result);
        }

        [Fact]
        public void LoadModule_ShouldThrowInvalidOperationException_WhenNoTypesImplementIModule()
        {
            // Arrange
            _fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(true);
            var mockAssembly = new Mock<Assembly>();
            mockAssembly.Setup(a => a.GetTypes()).Returns(new Type[] { typeof(string), typeof(int) });
            _assemblyLoadFromMock.Setup(a => a(It.IsAny<string>())).Returns(mockAssembly.Object);
            _moduleLoader = new ModuleLoader(_fileExistsMock.Object, _assemblyLoadFromMock.Object);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _moduleLoader.LoadModule(_modulePath));
            Assert.Equal("No valid module found in assembly.", exception.Message);
        }

        [Fact]
        public void LoadModule_ShouldReturnFirstValidModule_WhenMultipleModulesExistInAssembly()
        {
            // Arrange
            _fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(true);
            var mockAssembly = new Mock<Assembly>();
            mockAssembly.Setup(a => a.GetTypes()).Returns(new Type[]
            {
                typeof(string), // Non valide
                typeof(AbstractModule), // Non valide (abstrait)
                typeof(TestModule), // Valide
                typeof(AnotherModule) // Valide
            });
            _assemblyLoadFromMock.Setup(a => a(It.IsAny<string>())).Returns(mockAssembly.Object);
            _moduleLoader = new ModuleLoader(_fileExistsMock.Object, _assemblyLoadFromMock.Object);

            // Act
            var result = _moduleLoader.LoadModule(_modulePath);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<TestModule>(result); // Vérifie que le premier module valide est retourné
        }

        [Fact]
        public void LoadModule_ShouldThrowInvalidOperationException_WhenAllTypesAreAbstractOrInvalid()
        {
            // Arrange
            _fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(true);
            var mockAssembly = new Mock<Assembly>();
            mockAssembly.Setup(a => a.GetTypes()).Returns(new Type[] { typeof(AbstractModule), typeof(AnotherAbstractModule) });
            _assemblyLoadFromMock.Setup(a => a(It.IsAny<string>())).Returns(mockAssembly.Object);
            _moduleLoader = new ModuleLoader(_fileExistsMock.Object, _assemblyLoadFromMock.Object);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _moduleLoader.LoadModule(_modulePath));
            Assert.Equal("No valid module found in assembly.", exception.Message);
        }

        [Fact]
        public void LoadModule_ShouldSkipTypes_NotImplementingIModule()
        {
            // Arrange
            _fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(true);

            var mockAssembly = new Mock<Assembly>();
            mockAssembly.Setup(a => a.GetTypes()).Returns(new[] { typeof(NonModuleType) });
            _assemblyLoadFromMock.Setup(a => a(It.IsAny<string>())).Returns(mockAssembly.Object);

            _moduleLoader = new ModuleLoader(_fileExistsMock.Object, _assemblyLoadFromMock.Object);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _moduleLoader.LoadModule(_modulePath));
            Assert.Equal("No valid module found in assembly.", exception.Message);
        }

        [Fact]
        public void LoadModule_ShouldNotInstantiateAbstractTypes()
        {
            // Arrange
            _fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(true);

            var mockAssembly = new Mock<Assembly>();
            mockAssembly.Setup(a => a.GetTypes()).Returns(new[] { typeof(AbstractModule) });
            _assemblyLoadFromMock.Setup(a => a(It.IsAny<string>())).Returns(mockAssembly.Object);

            _moduleLoader = new ModuleLoader(_fileExistsMock.Object, _assemblyLoadFromMock.Object);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _moduleLoader.LoadModule(_modulePath));
            Assert.Equal("No valid module found in assembly.", exception.Message);
        }

        [Fact]
        public void LoadModule_ShouldCallInjectedDependencies()
        {
            // Arrange
            _fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(true);
            _assemblyLoadFromMock.Setup(a => a(It.IsAny<string>())).Returns(Assembly.GetExecutingAssembly());

            var createInstanceMock = new Mock<Func<Type, object?>>();
            createInstanceMock.Setup(ci => ci(It.IsAny<Type>())).Returns(new TestModule());

            _moduleLoader = new ModuleLoader(
                _fileExistsMock.Object,
                _assemblyLoadFromMock.Object,
                createInstanceMock.Object);

            // Act
            var result = _moduleLoader.LoadModule(_modulePath);

            // Assert
            Assert.NotNull(result);
            _fileExistsMock.Verify(f => f(_modulePath), Times.Once);
            _assemblyLoadFromMock.Verify(a => a(_modulePath), Times.Once);
            createInstanceMock.Verify(ci => ci(It.IsAny<Type>()), Times.AtLeastOnce);
        }

        // Helper classes
        public class NonModuleType
        {
            public static string Name => "Not a Module";
        }


        public abstract class AbstractModule : IModule
        {
            public void Initialize()
            {
                // Do nothing
            }
            public string Name { get; } = "Abstract Module";
            public string Description { get; } = "An abstract module.";
            public string Version { get; } = "1.0.0";
            public void Initialize(IEventBus eventBus, IModuleContext context, ILoggerService logger) { }
            public void Execute() { }
            public void Shutdown() { }
        }

        public abstract class AnotherAbstractModule : IModule
        {
            public void Initialize()
            {
                // Do nothing
            }
            public string Name { get; } = "Another Abstract Module";
            public string Description { get; } = "An abstract module but not the same.";
            public string Version { get; } = "1.0.0";
            public void Initialize(IEventBus eventBus, IModuleContext context, ILoggerService logger) { }
            public void Execute() { }
            public void Shutdown() { }
        }

        public class TestModule : IModule
        {
            public void Initialize()
            {
                // Do nothing
            }
            public string Name { get; } = "Test Module";
            public string Description { get; } = "A test module.";
            public string Version { get; } = "1.0.0";
            public void Initialize(IEventBus eventBus, IModuleContext context, ILoggerService logger) { }
            public void Execute() { }
            public void Shutdown() { }
        }

        public class AnotherModule : IModule
        {
            public void Initialize()
            {
                // Do nothing
            }
            public string Name { get; } = "Another Module";
            public string Description { get; } = "Another test module.";
            public string Version { get; } = "1.0.0";
            public void Initialize(IEventBus eventBus, IModuleContext context, ILoggerService logger) { }
            public void Execute() { }
            public void Shutdown() { }
        }
    }
}
