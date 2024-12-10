using Luxoria.Modules.Models;

namespace Luxoria.Modules.Interfaces;

public interface IModuleContext
{
    ImageData? GetCurrentImage();
    void UpdateImage(ImageData image);
    void LogMessage(string message);
}
