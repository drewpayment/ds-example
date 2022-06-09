using System.Collections.Generic;

namespace Dominion.Utility.Security
{
    /// <summary>
    /// Represents a secured action that can be performed.
    /// </summary>
    public interface IActionType
    {
        /// <summary>
        /// Get the unique identifier for this action.
        /// </summary>
        string Designation { get; }

        /// <summary>
        /// Get the label for this action.
        /// </summary>
        string Label { get; }

    }
}