using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Validation
{
    using System.Linq.Expressions;

    using FluentValidation.Validators;
    public class IsDecimalValidator : PropertyValidator
    {
        private readonly bool _allowNull;

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue;
            if (value != null)
            {
                decimal v;
                return decimal.TryParse(value.ToString(), out v);
            }
            return this._allowNull;
        }

        public IsDecimalValidator(bool allowNull, string errorMessage)
            : base(errorMessage ?? "Property {PropertyName} must be a number.")
        {
            this._allowNull = allowNull;
        }
        public IsDecimalValidator()
            : this(false, null)
        {
        }
    }
}
