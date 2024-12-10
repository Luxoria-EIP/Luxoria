using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Luxoria.Modules.Models;

/// <summary>
/// Represents an action associated with a tool in the Luxoria application.
/// </summary>
[ExcludeFromCodeCoverage]
public class LuxAction
{
    /// <summary>
    /// Gets the unique identifier for the action.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the unique identifier for the associated tool.
    /// </summary>
    public Guid ToolId { get; private set; }

    /// <summary>
    /// Gets the parameters associated with the action.
    /// </summary>
    public List<string> Parameters { get; private set; }

    /// <summary>
    /// Gets the arguments associated with the action.
    /// </summary>
    public List<string> Args { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LuxAction"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the action.</param>
    /// <param name="toolId">The unique identifier for the associated tool.</param>
    /// <param name="parameters">The parameters associated with the action.</param>
    /// <param name="args">The arguments associated with the action.</param>
    public LuxAction(Guid id, Guid toolId, List<string> parameters, List<string> args)
    {
        Id = id;
        ToolId = toolId;
        Parameters = parameters ?? new List<string>();
        Args = args ?? new List<string>();
    }
}
