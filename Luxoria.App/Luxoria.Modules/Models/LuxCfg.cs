using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Luxoria.Modules.Models;

/// <summary>
/// Represents a picture with its properties and associated actions and versioning.
/// </summary>
[ExcludeFromCodeCoverage]
public class LuxCfg
{
    /// <summary>
    /// Gets the configuration version of the Luxoria application.
    /// </summary>
    public string Version { get; private set; }

    /// <summary>
    /// Gets the unique identifier for the picture.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the name of the picture.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the file name of the picture.
    /// </summary>
    public string FileName { get; private set; }

    /// <summary>
    /// Gets the description of the picture.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Gets the file extension of the picture.
    /// </summary>
    public FileExtension Extension { get; private set; }

    /// <summary>ff
    /// Gets the full name of the picture, combining the name and extension.
    /// </summary>
    public string FullName => FileName;

    /// <summary>
    /// Gets the list of actions associated with the picture.
    /// </summary>
    public List<LuxAction> Actions { get; private set; }

    /// <summary>
    /// Gets the list of versions associated with the picture.
    /// </summary>
    public List<LuxVersion> Versionning { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Picture"/> class.
    /// </summary>
    /// <param name="version">The configuration version of Luxoria.</param>
    /// <param name="name">The name of the picture.</param>
    /// <param name="fileName">The file name of the picture.</param>
    /// <param name="description">The description of the picture.</param>
    /// <param name="extension">The file extension of the picture.</param>
    /// <param name="actions">The list of actions associated with the picture.</param>
    /// <param name="versionning">The list of versions associated with the picture.</param>
    public LuxCfg(string version, Guid fileUuid, string name, string fileName, string description, FileExtension extension)
    {
        Version = version;

        Id = fileUuid;
        Name = name;
        FileName = fileName;
        Description = description;
        Extension = extension;
        Actions = new List<LuxAction>();
        Versionning = new List<LuxVersion>();
    }

    /// <summary>
    /// Public model 'Asset interface'
    /// </summary>
    public class AssetInterface
    {
        // File name
        public required string FileName { get; set; }
        // Relative file path
        public required string RelativeFilePath { get; set; }
        // Related Luxoria Config Id
        public required Guid LuxCfgId { get; set; }
        // File hash (SHA-256)
        public required string Hash { get; set; }
    }
}
