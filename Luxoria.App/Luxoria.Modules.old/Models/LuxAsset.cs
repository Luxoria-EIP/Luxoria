using System.Diagnostics.CodeAnalysis;

namespace Luxoria.Modules.Models;

/// <summary>
/// Contains the properties of an asset in the Luxoria application.
/// Data -> ImageData
/// ConfigFile -> LuxCfg
/// </summary>
[ExcludeFromCodeCoverage]
public class LuxAsset
{
    /// <summary>
    /// Gets the unique identifier for the asset.
    /// </summary>
    public Guid Id => MetaData.Id;

    /// <summary>
    /// Contains the properties of the asset.
    /// </summary>
    public required LuxCfg MetaData { get; init; }

    /// <summary>
    /// Contains the data of the asset.
    /// </summary>
    public required ImageData Data { get; init; }
}
