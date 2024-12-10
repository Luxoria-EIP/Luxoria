using Luxoria.Modules.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Luxoria.Modules.Models.Events
{
    [ExcludeFromCodeCoverage]
    public class CollectionUpdatedEvent : IEvent
    {
        /// <summary>
        /// Collection name
        /// </summary>
        public string CollectionName { get; }

        /// <summary>
        /// Path to the collection
        /// </summary>
        public string CollectionPath { get; }

        /// <summary>
        /// Collection of assets
        /// </summary>
        public ICollection<LuxAsset> Assets { get; }

        /// <summary>
        /// Constructor for the CollectionUpdatedEvent class.
        /// </summary>
        /// <param name="collectionName">Collection's name</param>
        /// <param name="collectionPath">Path related to the collection</param>
        /// <param name="assets">Contains all assets</param>
        public CollectionUpdatedEvent(string collectionName, string collectionPath, ICollection<LuxAsset> assets)
        {
            CollectionName = collectionName;
            CollectionPath = collectionPath;
            Assets = assets;
        }
    }
}
