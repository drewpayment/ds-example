using System.Text.RegularExpressions;
using Dominion.Utility.Constants;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// A validator used to validate a Social Security Number string.
    /// </summary>
    public class SocialSecurityNumberValidator : RegularExpressionValidator
    {
        public static Regex SSN_REGEX = new Regex(ValidationConstants.SSN_REG_MATCH_ALW_WSP);

        /// <summary>
        /// Instantiates a new SocialSecurityNumberValidator.
        /// </summary>
        public SocialSecurityNumberValidator()
            : base(SSN_REGEX)
        {
        }

        /// <summary>
        /// Instantiates a new SocialSecurityNumberValidator.
        /// </summary>
        public SocialSecurityNumberValidator(string errorMessage)
            : base(SSN_REGEX)
        {
            this.ErrorMessageSource = new StaticStringSource(errorMessage);
        }

    }
}