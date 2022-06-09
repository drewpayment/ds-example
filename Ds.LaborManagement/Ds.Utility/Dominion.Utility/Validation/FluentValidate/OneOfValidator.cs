using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Validation
{
    using System.Linq.Expressions;

    using FluentValidation.Validators;
    public class OneOfValidator<T> : PropertyValidator
    {
        public T[] Values { get; }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue is T)
            {
                return Values.Contains((T)context.PropertyValue);
            }
            return false;
        }

        public OneOfValidator(string errorMessage, params T[] values)
            : base(errorMessage ?? $"Property {{PropertyName}} must be one of the following values: {string.Join(", ", values)}"
            )
        {
            this.Values = values;
        }
    }
}
