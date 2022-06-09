using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Validation
{
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;

    using FluentValidation.Validators;
    public class IsAlphanumericValidator : PropertyValidator
    {
        private static readonly Regex AlphanumericRegex = new Regex(@"^[\w\d]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public IsAlphanumericValidator()
            : this((string)null)
        {
        }

        public IsAlphanumericValidator(string errorMessage)
            : base(errorMessage ?? "Property {PropertyName} must be alphanumeric.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue?.ToString();
            return value == null || AlphanumericRegex.IsMatch(value);

        }
    }
}
