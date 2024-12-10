using Luxoria.Modules.Interfaces;
using System;
using System.IO;
using System.Reflection;

namespace Luxoria.Modules
{
    public class ModuleLoader
    {
        private readonly Func<string, bool> _fileExists;
        private readonly Func<string, Assembly> _loadAssembly;
        private readonly Func<Type, object?> _createInstance;

        public ModuleLoader(
            Func<string, bool>? fileExists = null,
            Func<string, Assembly>? loadAssembly = null,
            Func<Type, object?>? createInstance = null)
        {
            _fileExists = fileExists ?? File.Exists;
            _loadAssembly = loadAssembly ?? Assembly.LoadFrom;
            _createInstance = createInstance ?? Activator.CreateInstance;
        }

        public IModule LoadModule(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path), "Path cannot be null");
            }

            if (!_fileExists(path))
            {
                throw new FileNotFoundException($"Module not found: [{path}]");
            }

            try
            {
                Assembly assembly = _loadAssembly(path);

                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IModule).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        try
                        {
                            var instance = _createInstance(type);
                            if (instance is IModule module)
                            {
                                return module;
                            }
                        }
                        catch (Exception ex) when (ex is MissingMethodException || ex is TargetInvocationException)
                        {
                            throw new InvalidOperationException(
                                $"Failed to create instance of module type: {type.FullName}. {ex.Message}", ex);
                        }
                    }
                }

                throw new InvalidOperationException("No valid module found in assembly.");
            }
            catch (FileLoadException ex)
            {
                throw new FileLoadException($"Could not load assembly: {path}", ex);
            }
        }
    }
}
