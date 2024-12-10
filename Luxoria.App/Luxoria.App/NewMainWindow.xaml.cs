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

            // Obtenir le ModuleService via le conteneur d'injection de d�pendances
            var app = (App)Application.Current;
            _moduleService = app.ModuleService;

            InitializeCategories();
        }

        /// <summary>
        /// Initialise les cat�gories disponibles dans la ComboBox.
        /// </summary>
        private void InitializeCategories()
        {
            var categories = new List<string> { "Library", "Development", "Organize", "Modules" };
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.SelectedIndex = 0; // S�lectionne la premi�re cat�gorie par d�faut
        }

        /// <summary>
        /// G�re l'�v�nement de changement de s�lection de la ComboBox.
        /// Charge les modules correspondant � la cat�gorie s�lectionn�e.
        /// </summary>
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedItem is string category)
            {
                LoadCategory(category);
            }
        }

        /// <summary>
        /// Charge les modules pour une cat�gorie sp�cifique et injecte leurs vues dans les panneaux appropri�s.
        /// </summary>
        /// <param name="category">La cat�gorie s�lectionn�e.</param>
        private void LoadCategory(string category)
        {
            // Effacer le contenu existant dans les panneaux
            ClearPanel(LeftPanel);
            ClearPanel(MainContent);
            ClearPanel(RightPanel);

            // Obtenir les int�grations de modules pour la cat�gorie s�lectionn�e
            var integrations = _moduleService.GetIntegrationsForCategory(category);

            foreach (var integration in integrations)
            {
                var view = integration.GetView();

                // Ins�rer la vue dans la r�gion sp�cifi�e
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
                        // Optionnel : g�rer les r�gions inconnues
                        break;
                }
            }
        }

        /// <summary>
        /// Efface le contenu d'un panneau.
        /// </summary>
        /// <param name="panel">Le panneau � effacer.</param>
        private void ClearPanel(Border panel)
        {
            panel.Child = null;
        }

        /// <summary>
        /// Ins�re une vue dans un panneau sp�cifique.
        /// </summary>
        /// <param name="panel">Le panneau o� ins�rer la vue.</param>
        /// <param name="element">La vue � ins�rer.</param>
        private void InsertView(Border panel, UIElement element)
        {
            panel.Child = element;
        }
    }
}
