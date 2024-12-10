using Luxoria.Modules.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Luxoria.Modules.Models.Events;

/// <summary>
/// Represents an event triggered when a collection is opened.
/// </summary>
[ExcludeFromCodeCoverage]
public class OpenCollectionEvent : IEvent
{
    // *** Properties ***

    /// <summary>
    /// Gets the name of the collection.
    /// </summary>
    public string CollectionName { get; }

    /// <summary>
    /// Gets the path to the collection.
    /// </summary>
    public string CollectionPath { get; }

    // *** Events ***

    /// <summary>
    /// Event triggered to report progress messages.
    /// </summary>
    public event OnProgressMessage? ProgressMessage;

    /// <summary>
    /// Delegate for the progress message event.
    /// </summary>
    public delegate void OnProgressMessage(string message, int? progress);

    /// <summary>
    /// Event triggered when the event is completed.
    /// </summary>
    public event EventHandler? OnEventCompleted;

    /// <summary>
    /// Event triggered when the collection fails to import.
    /// </summary>
    public event EventHandler? OnImportFailed;

    // *** Constructor ***

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenCollectionEvent"/> class.
    /// </summary>
    /// <param name="name">The name of the collection.</param>
    /// <param name="path">The path to the collection.</param>
    public OpenCollectionEvent(string name, string path)
    {
        CollectionName = name ?? throw new ArgumentNullException(nameof(name), "Collection name cannot be null.");
        CollectionPath = path ?? throw new ArgumentNullException(nameof(path), "Collection path cannot be null.");
    }

    // *** Methods ***

    /// <summary>
    /// Sends a progress message with an optional progress value.
    /// </summary>
    /// <param name="message">The message to be sent through the event channel.</param>
    /// <param name="progressValue">The progress value (0-100). Can be null if not applicable.</param>
    public void SendProgressMessage(string message, int? progressValue = null)
    {
        // Invoke the ProgressMessage event if there are subscribers
        ProgressMessage?.Invoke(message, progressValue);
    }

    /// <summary>
    /// Marks the event as successfully completed, signaling no more messages will be sent.
    /// </summary>
    public void CompleteSuccessfully()
    {
        // Logic to complete the event, indicating successful completion
        // Trigger EventCompleted event when the operation finishes successfully
        OnEventCompleted?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Marks the event as having encountered an error.
    /// </summary>
    public void MarkAsFailed()
    {
        // Logic to complete the event, indicating an error occurred
        // Trigger ImportFailed event when an error occurs
        OnImportFailed?.Invoke(this, EventArgs.Empty);
    }
}
