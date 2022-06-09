using Dominion.Utility.OpResult;

namespace Dominion.Utility.Msg.Identifiers
{
    /// <summary>
    /// All message codes should be defined in this class.
    /// lowfix: jay: should we continue to use this? Can we use enums and ToString() them instead rather than using int?
    /// </summary>
    public class MsgCodes
    {
        /// <summary>
        /// Validation errors/info from the domain model entities.
        /// Currently we use Fluent validation, but this error code wouldn't change if we used something else.
        /// </summary>
        public static readonly int BasicException = -1;

        /// <summary>
        /// This message is information only.
        /// </summary>
        public static readonly int Information = 0;

        /// <summary>
        /// Validation errors/info from the domain model entities.
        /// Currently we use Fluent validation, but this error code wouldn't change if we used something else.
        /// </summary>
        public static readonly int EntityValidation = 1;

        /// <summary>
        /// Validation errors/info from entity framework's configuration validation.
        /// These are errors specific to entity framework's built-in entity validation defined by the entity configuration code.
        /// </summary>
        public static readonly int EntityFrameworkValidation = 2;

        /// <summary>
        /// Errors from produced during a unit of work data store failure.
        /// ie. An error thrown due to constaints at the sql level when trying to persist data via the entity framework.
        /// </summary>
        public static readonly int UnitOfWorkDataStoreFailure = 3;

        /// <summary>
        /// Error produced if the user isn't allowed to carry out an action due to permissions.
        /// </summary>
        public static readonly int ActionNotAllowed = 4;

        /// <summary>
        /// Error produced when expected data was not found.  Typically used in conjunction with an 
        /// <see cref="IOpResult{TDataType}"/>.
        /// </summary>
        public static readonly int DataNotFound = 5;

        /// <summary>
        /// Error occured when updating data.
        /// </summary>
        public static readonly int DataUpdateError = 6;

        /// <summary>
        /// Error occured when during schema validation.
        /// </summary>
        public static readonly int SchemaValidationErr = 7;

        /// <summary>
        /// Error occured during data validation.
        /// </summary>
        public static readonly int DataValidationError = 8;
    }
}