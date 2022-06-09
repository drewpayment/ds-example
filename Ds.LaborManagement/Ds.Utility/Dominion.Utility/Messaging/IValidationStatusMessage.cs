using System.Collections.Generic;

namespace Dominion.Utility.Messaging
{
    /// <summary>
    /// Available validation status message types.
    /// </summary>
    public enum ValidationStatusMessageType
    {
        General, 
        Required, 
        OutOfRange, 
        Incompatible, 
        ActionNotAllowed, 
        InvalidFormat, 
        NotFound, 
        RecordLocked
    }

    /// <summary>
    /// Interface definition for validation status messages.
    /// </summary>
    /// <remarks>
    /// Notice that this encourages the implementation of an immutable concrete class.
    /// </remarks>
    public interface IValidationStatusMessage : IStatusMessage
    {
        /// <summary>
        /// Get the list of field names to which this message applies.
        /// </summary>
        /// <remarks>This property is intended to be consistent with the 
        /// System.ComponentModel.DataAnnotations.ValidationResult class.</remarks>
        IEnumerable<string> MemberNames { get; }

        /// <summary>
        /// The type of validation error.
        /// </summary>
        ValidationStatusMessageType MessageType { get; }
    }
}