using System;
using System.Text.RegularExpressions;
using FluentValidation.Validators;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Validates a Routing Transit Number (RTN).
    /// Use static <see cref="IsValid"/> validation method.
    /// </summary>
    public class RoutingNumberValidator : PropertyValidator
    {
        private static readonly Regex RoutingNumberRegex = new Regex(@"^\d{9}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public RoutingNumberValidator()
            : this((string)null)
        {
        }

        public RoutingNumberValidator(string errorMessage)
            : base(errorMessage ?? "{PropertyName} is not a valid Routing Transit Number (RTN).")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue?.ToString();

            return RoutingNumberValidator.IsValid(value);
        }

        /// <summary>
        /// Validates a Routing Transit Number (RTN)
        /// https://en.wikipedia.org/wiki/ABA_routing_transit_number
        /// </summary>
        /// <remarks>Derived from: https://github.com/DrShaffopolis/bank-routing-number-validator</remarks>
        /// <param name="routing">Routing number to validate</param>
        /// <param name="isRequired">Default false. If true, will also ensure not null/empty.</param>
        /// <returns></returns>
        public static bool IsValid(string routing, bool isRequired = false)
        {
            if (string.IsNullOrWhiteSpace(routing))
                return !isRequired;


            if (!RoutingNumberRegex.IsMatch(routing))
                return false;

            //The first two digits of the nine digit RTN must be in the ranges 00 through 12, 21 through 32, 61 through 72, or 80.
            //https://en.wikipedia.org/wiki/Routing_transit_number
            var firstTwo = Convert.ToInt32(routing.Substring(0, 2));
            var firstTwoValid = (0 <= firstTwo && firstTwo <= 12)
                                || (21 <= firstTwo && firstTwo <= 32)
                                || (61 <= firstTwo && firstTwo <= 72)
                                || firstTwo == 80;
            if (!firstTwoValid)
                return false;

            //this is the checksum
            //https://en.wikipedia.org/wiki/ABA_routing_transit_number#Check_digit
            //https://stackoverflow.com/a/47850677
            int check = 0;
            for (int index = 0; index < 3; index++)
            {
                int pos = index * 3;
                check += (routing[pos]     - '0') * 3;
                check += (routing[pos + 1] - '0') * 7;
                check += (routing[pos + 2] - '0');
            }

            return check != 0 && check % 10 == 0; ;
        }
    }
}