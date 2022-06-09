using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Validation
{
    using System.Linq.Expressions;

    using FluentValidation.Validators;
    public class IsIntValidator : PropertyValidator
    {
        private readonly bool _allowNull;

        public IsIntValidator()
            : this(false, (string)null)
        {
        }

        public IsIntValidator(bool allowNull, string errorMessage)
            : base(errorMessage ?? "Property {PropertyName} must be a valid integer number.")
        {
            this._allowNull = allowNull;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue;

            if (value != null)
            {
                int v;
                return int.TryParse(value.ToString(), out v);
            }
            else
            {
                return this._allowNull;
            }
        }
    }
}
