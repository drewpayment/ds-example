using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Validators;

namespace Dominion.Utility.Validation.FluentValidate
{
    public class ArrayLengthValidator : PropertyValidator
    {
        private int _min;
        private int _max;
        private const bool AllowNull = true;

        public ArrayLengthValidator()
            : this(int.MinValue, int.MaxValue, (string)null)
        {
        }

        public ArrayLengthValidator(int min, int max, string errorMessage)
            : base(errorMessage ?? GetDefaultErrorMessage(min, max))
        {
            _min = min;
            _max = max;
        }

        private static string GetDefaultErrorMessage(int min, int max)
        {
            if (min != int.MinValue && max == int.MaxValue)
            {
                return $"Property {{PropertyName}} must contain more than {min - 1} elements";
            }
            else if (min == int.MinValue && max != int.MaxValue)
            {
                return $"Property {{PropertyName}} must contain fewer than {max} elements";
            }
            else if (min == int.MinValue && max == int.MaxValue)
            {
                return $"Property {{PropertyName}} can contain any number of elements";
            }
            return $"Property {{PropertyName}} must contain more than {min - 1} and fewer than {max} elements";
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue as Array;

            if (value != null)
            {
                return this._min <= value.Length && this._max > value.Length;
            }
            return AllowNull;
        }
    }
}
