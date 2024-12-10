

using Luxoria.SDK.Interfaces;

namespace Luxoria.Modules.Interfaces;

public interface IModule
{
    string Name { get; }
    string Description { get; }
    string Version { get; }
    void Initialize(IEventBus eventBus, IModuleContext context, ILoggerService logger);
    void Execute();
    void Shutdown();
}
