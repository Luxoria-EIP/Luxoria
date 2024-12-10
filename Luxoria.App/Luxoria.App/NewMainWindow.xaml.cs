// Luxoria.App/NewMainWindow.xaml.cs
using Luxoria.Core.Interfaces;
using Luxoria.Modules.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace Luxoria.App
{
    public sealed partial class NewMainWindow : Window
    {
        private readonly IModuleService _moduleService;

        public NewMainWindow()
        {
            this.InitializeComponent();

            // Obtenir le ModuleService via le conteneur d'injection de dépendances
            var app = (App)Application.Current;
            _moduleService = app.ModuleService;

            InitializeCategories();
        }

        /// <summary>
        /// Initialise les catégories disponibles dans la ComboBox.
        /// </summary>
        private void InitializeCategories()
        {
            var categories = new List<string> { "Library", "Development", "Organize", "Modules" };
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.SelectedIndex = 0; // Sélectionne la première catégorie par défaut
        }

        /// <summary>
        /// Gère l'événement de changement de sélection de la ComboBox.
        /// Charge les modules correspondant à la catégorie sélectionnée.
        /// </summary>
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem is string category)
            {
                LoadCategory(category);
            }
        }

        /// <summary>
        /// Charge les modules pour une catégorie spécifique et injecte leurs vues dans les panneaux appropriés.
        /// </summary>
        /// <param name="category">La catégorie sélectionnée.</param>
        private void LoadCategory(string category)
        {
            // Effacer le contenu existant dans les panneaux
            ClearPanel(LeftPanel);
            ClearPanel(MainContent);
            ClearPanel(RightPanel);

            // Obtenir les intégrations de modules pour la catégorie sélectionnée
            var integrations = _moduleService.GetIntegrationsForCategory(category);

            foreach (var integration in integrations)
            {
                var view = integration.GetView();

                // Insérer la vue dans la région spécifiée
                switch (integration.Region)
                {
                    case "LeftPanel":
                        InsertView(LeftPanel, view);
                        break;
                    case "MainContent":
                        InsertView(MainContent, view);
                        break;
                    case "RightPanel":
                        InsertView(RightPanel, view);
                        break;
                    // Si tu ajoutes un BottomPanel, ajoute un case "BottomPanel":
                    default:
                        // Optionnel : gérer les régions inconnues
                        break;
                }
            }
        }

        /// <summary>
        /// Efface le contenu d'un panneau.
        /// </summary>
        /// <param name="panel">Le panneau à effacer.</param>
        private void ClearPanel(Border panel)
        {
            panel.Child = null;
        }

        /// <summary>
        /// Insère une vue dans un panneau spécifique.
        /// </summary>
        /// <param name="panel">Le panneau où insérer la vue.</param>
        /// <param name="element">La vue à insérer.</param>
        private void InsertView(Border panel, UIElement element)
        {
            panel.Child = element;
        }
    }
}
