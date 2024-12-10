using Microsoft.UI.Xaml;

namespace Luxoria.Modules.Interfaces
{
    public interface IModuleUIIntegration
    {
        string Category { get; }
        string Region { get; }
        UIElement GetView();
    }
}
