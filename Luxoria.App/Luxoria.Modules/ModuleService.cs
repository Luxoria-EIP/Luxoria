using Luxoria.Core.Interfaces;
using Luxoria.Modules.Interfaces;
using Luxoria.SDK.Interfaces;
using Luxoria.SDK.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Luxoria.Core.Services
{
    public class ModuleService : IModuleService
    {
        private readonly List<IModule> _modules = new List<IModule>();
        private readonly List<IModuleUIIntegration> _uiIntegrations = new List<IModuleUIIntegration>();
        private readonly IEventBus _eventBus;
        private readonly ILoggerService _logger;

        public ModuleService(IEventBus eventBus, ILoggerService logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus), "EventBus cannot be null");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "LoggerService cannot be null");
        }

        public void AddModule(IModule module)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module), "Module cannot be null");
            }

            _modules.Add(module);

            if (module is IModuleUIIntegration uiIntegration)
            {
                _uiIntegrations.Add(uiIntegration);
            }
        }

        public void RemoveModule(IModule module)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module), "Module cannot be null");
            }

            _modules.Remove(module);

            if (module is IModuleUIIntegration uiIntegration)
            {
                _uiIntegrations.Remove(uiIntegration);
            }
        }

        public List<IModule> GetModules() => _modules;

        public void InitializeModules(IModuleContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "ModuleContext cannot be null");
            }

            foreach (IModule module in _modules)
            {
                module.Initialize(_eventBus, context, _logger);
            }
        }

        public void LoadModules(string modulesPath)
        {
            if (!Directory.Exists(modulesPath))
            {
                _logger.Log($"Modules directory does not exist: {modulesPath}", "ModuleService", LogLevel.Warning);
                return;
            }

            var moduleFiles = Directory.GetFiles(modulesPath, "*.Lux.dll", SearchOption.AllDirectories);

            foreach (var moduleFile in moduleFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(moduleFile);
                    var moduleTypes = assembly.GetTypes().Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in moduleTypes)
                    {
                        var module = (IModule)Activator.CreateInstance(type);
                        AddModule(module);
                        _logger.Log($"Loaded module: {module.Name}", "ModuleService", LogLevel.Info);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log($"Failed to load module from {moduleFile}: {ex.Message}", "ModuleService", LogLevel.Error);
                }
            }
        }

        public IEnumerable<IModuleUIIntegration> GetIntegrationsForCategory(string category)
        {
            return _uiIntegrations.Where(i => i.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }
    }
}
