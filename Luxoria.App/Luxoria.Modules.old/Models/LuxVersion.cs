using System.Diagnostics.CodeAnalysis;

namespace Luxoria.Modules.Models;

[ExcludeFromCodeCoverage]
public class LuxVersion
{
    /// <summary>
    /// Gets or sets the unique identifier for the version.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the version.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the version number (e.g., "1.0.0").
    /// </summary>
    public required string VersionNumber { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the related action.
    /// </summary>
    public Guid RelatedToActionId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LuxVersion"/> class.
    /// </summary>
    /// <param name="name">The name of the version.</param>
    /// <param name="versionNumber">The version number.</param>
    public LuxVersion(string name, string versionNumber)
    {
        Name = name;
        VersionNumber = versionNumber;
    }
}
