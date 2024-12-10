using Luxoria.Modules.Interfaces;

namespace Luxoria.Core.Interfaces
{
    public interface IModuleService
    {
        void AddModule(IModule module);

        void RemoveModule(IModule module);

        List<IModule> GetModules();

        void InitializeModules(IModuleContext context);
    }
}
