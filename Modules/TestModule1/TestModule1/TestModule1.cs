using System.Reflection;
using Luxoria.Modules.Interfaces;
using Luxoria.Modules.Models.Events;
using Luxoria.SDK.Interfaces;
using Luxoria.SDK.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace TestModule1
{
    public class TestModule1 : IModule, IModuleUIIntegration
    {
        private IEventBus? _eventBus;
        private IModuleContext? _context;
        private ILoggerService? _logger;

        public string Name => "Test Module1";
        public string Description => "Basic module for testing purposes.";
        public string Version => "1.0.1";

        // IModuleUIIntegration properties
        public string Category => "Library"; // La catégorie où le module s'intègre
        public string Region => "MainContent"; // La région de l'interface où le module s'intègre

        public void Initialize(IEventBus eventBus, IModuleContext context, ILoggerService logger)
        {
            _eventBus = eventBus;
            _context = context;
            _logger = logger;

            // Subscribe to the TextInputEvent to process text input
            _eventBus.Subscribe<TextInputEvent>(OnTextInputReceived);

            // Check if EventBus & Context are not null before proceeding
            if (_eventBus == null || _context == null)
            {
                _logger?.Log("Failed to initialize TestModule1: EventBus or Context is null", "Mods/TestModule1", LogLevel.Error);
                return;
            }

            _logger?.Log($"{Name} initialized", "Mods/TestModule1", LogLevel.Info);
        }

        public void Execute()
        {
            _logger?.Log($"{Name} executed", "Mods/TestModule1", LogLevel.Info);
            // Ajoute plus de logique si nécessaire
        }

        public void Shutdown()
        {
            // Unsubscribe from events to éviter les fuites de mémoire
            _eventBus?.Unsubscribe<TextInputEvent>(OnTextInputReceived);

            _logger?.Log($"{Name} shut down", "Mods/TestModule1", LogLevel.Info);
        }

        private void OnTextInputReceived(TextInputEvent textInputEvent)
        {
            // Traiter le texte d'entrée
            _logger?.Log($"Received input text: {textInputEvent.Text}", "Mods/TestModule1", LogLevel.Info);

            // Effectuer une logique de traitement avec le texte d'entrée (par exemple, mettre à jour une image)
            string updatedImagePath = ProcessInputText(textInputEvent.Text);

            // Publier un événement pour notifier qu'une image a été mise à jour
            _eventBus?.Publish(new ImageUpdatedEvent(updatedImagePath));
        }

        private string ProcessInputText(string inputText)
        {
            // Logique fictive pour simuler le traitement d'image basé sur le texte d'entrée
            _logger?.Log($"Processing input text: {inputText}", "Mods/TestModule1", LogLevel.Info);

            // Retourner un chemin d'image fictif pour des fins de démonstration
            return "path/to/updated/image.png";
        }

        // Méthode pour obtenir l'élément UI à insérer dans l'application
        public UIElement GetView()
        {
            return new TextBlock
            {
                Text = "Hello from TestModule1!",
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
    }
}
