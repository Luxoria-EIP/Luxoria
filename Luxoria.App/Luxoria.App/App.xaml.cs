using Luxoria.Core.Interfaces;
using Luxoria.Core.Logics;
using Luxoria.Modules;
using Luxoria.Modules.Interfaces;
using Luxoria.SDK.Interfaces;
using Luxoria.SDK.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;
using System.IO;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Luxoria.App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public Window Window => m_window;
        private NewMainWindow m_window;

        private readonly Startup _startup;
        private readonly IHost _host;
        // Logger section part
        private const string LOG_SECTION = "General";
        private readonly ILoggerService _logger;

        private readonly IModuleService _moduleService;
        public IModuleService ModuleService => _moduleService;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            _startup = new Startup();
            _host = CreateHostBuilder(_startup).Build();
            _moduleService = _host.Services.GetRequiredService<IModuleService>();
            _logger = _host.Services.GetRequiredService<ILoggerService>();
        }

        public static IHostBuilder CreateHostBuilder(Startup startup)
        {
            return Host.CreateDefaultBuilder().ConfigureServices((context, services) => startup.ConfigureServices(context, services));
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            await _logger.LogAsync("Application is starting...");

            // Show splash screen
            var splashScreen = new SplashScreen();
            splashScreen.Activate();

            await _logger.LogAsync("Modules loaded. Closing slasph screen...");
            await Task.Delay(500);

            string modulesPath = GetOrCreateModulesDirectory();
            _moduleService.LoadModules(modulesPath);
            _moduleService.InitializeModules(new ModuleContext());

            // Load modules asynchronously and update the splash screen with the module names
            await LoadModulesAsync(splashScreen);

            // Close the splash screen after loading modules
            splashScreen.DispatcherQueue.TryEnqueue(() =>
            {
                splashScreen.Close();
            });

            var eventBus = _host.Services.GetRequiredService<IEventBus>();
            var loggerService = _host.Services.GetRequiredService<ILoggerService>();

            m_window = new NewMainWindow();
            m_window.Activate();
        }

        /// <summary>
        /// Loads all modules asynchronously and updates the splash screen with progress.
        /// </summary>
        private async Task LoadModulesAsync(SplashScreen splashScreen)
        {
            // Creates a scoped service provider and ensures cleanup
            using var scope = _host.Services.CreateScope();

            // Ensures the modules directory exists and retrieves its path
            string modulesPath = GetOrCreateModulesDirectory();

            var loader = new ModuleLoader();

            // Retrieves all subdirectories under the modules directory
            string[] moduleFolders = Directory.GetDirectories(modulesPath);

            // Load all modules from each folder
            foreach (string moduleFolder in moduleFolders)
            {
                await LoadModulesFromFolderAsync(moduleFolder, loader, splashScreen);
            }

            // Finalize module initialization and update the splash screen
            await UpdateSplashScreenAsync(splashScreen, "Initializing modules...");
            _moduleService.InitializeModules(new ModuleContext());
        }

        /// <summary>
        /// Ensures the "modules" directory exists and returns its path.
        /// If the directory does not exist, it is created.
        /// </summary>
        private string GetOrCreateModulesDirectory()
        {
            string modulesPath = Path.Combine(AppContext.BaseDirectory, "modules");

            if (!Directory.Exists(modulesPath))
            {
                _logger.Log($"Modules directory not found: {modulesPath}", LOG_SECTION, LogLevel.Warning);
                Directory.CreateDirectory(modulesPath);
                _logger.Log($"Modules directory created: {modulesPath}");
            }

            return modulesPath;
        }

        /// <summary>
        /// Loads all module DLL files from a specific folder.
        /// </summary>
        /// <param name="moduleFolder">The folder containing the module files.</param>
        /// <param name="loader">The module loader responsible for loading modules.</param>
        /// <param name="splashScreen">The splash screen to update with progress.</param>
        private async Task LoadModulesFromFolderAsync(string moduleFolder, ModuleLoader loader, SplashScreen splashScreen)
        {
            // Find all ".Lux.dll" files in the folder
            string[] moduleFiles = Directory.GetFiles(moduleFolder, "*.Lux.dll");

            if (moduleFiles.Length == 0)
            {
                await _logger.LogAsync($"No module DLL files found in: {moduleFolder}", LOG_SECTION, LogLevel.Warning);
                return;
            }

            // Load each module file
            foreach (string moduleFile in moduleFiles)
            {
                string moduleName = Path.GetFileNameWithoutExtension(moduleFile);
                await LoadModuleAsync(moduleFile, moduleName, loader, splashScreen);
            }
        }

        /// <summary>
        /// Attempts to load a single module DLL file asynchronously.
        /// </summary>
        /// <param name="moduleFile">The path to the module file.</param>
        /// <param name="moduleName">The name of the module (derived from the file name).</param>
        /// <param name="loader">The module loader responsible for loading the module.</param>
        /// <param name="splashScreen">The splash screen to update with progress.</param>
        private async Task LoadModuleAsync(string moduleFile, string moduleName, ModuleLoader loader, SplashScreen splashScreen)
        {
            // Update the splash screen to indicate the module being loaded
            await UpdateSplashScreenAsync(splashScreen, $"Loading {moduleName}...");
            await _logger.LogAsync($"Trying to load: {moduleName}");

            try
            {
                // Load the module in a background task to prevent blocking
                await Task.Run(() =>
                {
                    IModule module = loader.LoadModule(moduleFile);
                    if (module != null)
                    {
                        // Log detailed information about the loaded module
                        LogModuleInfo(module, moduleName);

                        // Add the loaded module to the module service
                        _moduleService.AddModule(module);
                    }
                    else
                    {
                        _logger.Log($"No valid module found in: {moduleFile}", LOG_SECTION, LogLevel.Warning);
                    }
                });
            }
            catch (FileNotFoundException ex)
            {
                // Handle cases where the file is missing
                LogError($"File not found for module [{moduleFile}]: {ex.Message}");
            }
            catch (BadImageFormatException ex)
            {
                // Handle cases where the file format is invalid
                LogError($"Invalid module file format for [{moduleFile}]: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                LogError($"Failed to load module [{moduleFile}]: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the splash screen's module text and applies a brief delay for smooth updates.
        /// </summary>
        /// <param name="splashScreen">The splash screen to update.</param>
        /// <param name="message">The message to display on the splash screen.</param>
        private static async Task UpdateSplashScreenAsync(SplashScreen splashScreen, string message)
        {
            splashScreen.DispatcherQueue.TryEnqueue(() =>
            {
                splashScreen.CurrentModuleTextBlock.Text = message;
            });

            // Small delay to ensure the UI has time to update
            await Task.Delay(300);
        }

        /// <summary>
        /// Logs information about a successfully loaded module.
        /// </summary>
        /// <param name="module">The loaded module instance.</param>
        /// <param name="moduleName">The name of the module.</param>
        private void LogModuleInfo(IModule module, string moduleName)
        {
            _logger.Log($"Module loaded: {moduleName}");
            _logger.Log($"Module name: {module.Name}");
            _logger.Log($"Module version: {module.Version}");
            _logger.Log($"Module description: {module.Description}");
        }

        /// <summary>
        /// Logs an error message with the appropriate log level and section.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        private void LogError(string message)
        {
            _logger.Log(message, LOG_SECTION, LogLevel.Error);
        }
    }
}
