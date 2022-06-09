using Dominion.Utility.Constants;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Text validation rule which validates text based on its length.
    /// </summary>
    public class TextLengthValidationRule : RegularExpressionValidationRule
    {
        #region VARIABLES & PROPERTIES

        public static readonly int? DefaultMinimumLength = 8;
        public static readonly int? DefaultMaximumLength = null;

        /// <summary>
        /// Minimum length the text can be, or null if text does not have a minimum length.
        /// </summary>
        public int? MinimumLength { get; set; }

        /// <summary>
        /// Maximum length the text can be, or null if text does not have a maximum length.
        /// </summary>
        public int? MaximumLength { get; set; }

        /// <summary>
        /// Regular expression representation of the length constraint in the form of: ^(?=.{min,max}).*$ 
        /// </summary>
        public override string RegularExpression
        {
            get
            {
                // build the following pattern: ^(?=.{min,max}).*$
                // where:
                // min => minimum length (or blank if null)
                // max => maximum length (or blank if null)
                string regex = @"^(?=.{";

                if (MinimumLength.HasValue)
                    regex += MinimumLength.Value;

                regex += ",";

                if (MaximumLength.HasValue)
                    regex += MaximumLength.Value;

                regex += "}).*$";

                return regex;
            }
        }

        /// <summary>
        /// User-friendly description of the length rule.
        /// </summary>
        public override string Description
        {
            get { return BuildMessage("Contain at least", "Contain no more than", "Contain between"); }
        }

        /// <summary>
        /// Error message to be used when the text does not satisfy the given length rule.
        /// </summary>
        public override string ErrorMessage
        {
            get { return BuildMessage("Must be at least", "Cannot exceed", "Must be between"); }
        }

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Instantiates a new TextLengthValidationRule with default length constraint settings.
        /// </summary>
        public TextLengthValidationRule()
        {
            MinimumLength = DefaultMinimumLength;
            MaximumLength = DefaultMaximumLength;
        }

        #endregion

        #region PRIVATE HELPERS

        /// <summary>
        /// Length constraint options that rule can be configured for.
        /// </summary>
        private enum LengthConstraintType
        {
            None, 
            MinimumOnly, 
            MaximumOnly, 
            MinMax
        }

        /// <summary>
        /// Length constraint type the rule is configured for based on what constraints (min and/or max) have been 
        /// specified.
        /// </summary>
        private LengthConstraintType LengthConstraint
        {
            get
            {
                if (MinimumLength.HasValue && !MaximumLength.HasValue)
                    return LengthConstraintType.MinimumOnly;
                if (MaximumLength.HasValue && !MinimumLength.HasValue)
                    return LengthConstraintType.MaximumOnly;
                if (MaximumLength.HasValue && MinimumLength.HasValue)
                    return LengthConstraintType.MinMax;

                return LengthConstraintType.None;
            }
        }

        /// <summary>
        /// Builds a message based on the current length constraints and prefixes provided. 
        /// </summary>
        /// <param name="minOnlyMessagePrefix">Message to add to the beginning of the message when only a minimum
        /// length constraint exists.</param>
        /// <param name="maxOnlyMessagePrefix">Message to add to the beginning of the message when only a maximum length
        /// constraint exists.</param>
        /// <param name="minMaxMessagePrefix">Message to add to the beginning of the message when both a min and max length
        /// constraint exists.</param>
        /// <returns></returns>
        private string BuildMessage(string minOnlyMessagePrefix, string maxOnlyMessagePrefix, string minMaxMessagePrefix)
        {
            string msg = CommonConstants.EMPTY_STRING;

            switch (LengthConstraint)
            {
                case LengthConstraintType.MinimumOnly:
                    msg += minOnlyMessagePrefix + CommonConstants.SINGLE_SPACE + MinimumLength.GetValueOrDefault();
                    break;
                case LengthConstraintType.MaximumOnly:
                    msg += maxOnlyMessagePrefix + CommonConstants.SINGLE_SPACE + MaximumLength.GetValueOrDefault();
                    break;
                case LengthConstraintType.MinMax:
                    msg += minMaxMessagePrefix + CommonConstants.SINGLE_SPACE + MinimumLength.GetValueOrDefault() + "-" +
                           MaximumLength.GetValueOrDefault();
                    break;
                default:
                    msg += "Any number of ";
                    break;
            }

            msg += CommonConstants.SINGLE_SPACE + "characters";

            return msg;
        }

        #endregion
    }
}