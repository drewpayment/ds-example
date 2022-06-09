namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Regex password rule preventing spaces from being included in a password.
    /// </summary>
    public class NoSpaceValidationRule : RegularExpressionValidationRule
    {
        /// <summary>
        /// User-friendly description of the rule.
        /// </summary>
        public override string Description
        {
            get { return "Not contain spaces"; }
        }

        /// <summary>
        /// Regular expression representation of the rule: ^(?!.*\s).*$
        /// </summary>
        public override string RegularExpression
        {
            get { return @"^(?!.*\s).*$"; }
        }

        /// <summary>
        /// Error message to display if the rule fails.
        /// </summary>
        public override string ErrorMessage
        {
            get { return "No spaces allowed"; }
        }
    }
}