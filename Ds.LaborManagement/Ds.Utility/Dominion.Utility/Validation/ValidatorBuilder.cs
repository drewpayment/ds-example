using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Validation
{
    using Dominion.Utility.Validation.FluentValidate;

    using FluentValidation;

    public static class ValidatorBuilder
    {
        public static ValidatorBuilder<T> For<T>() 
            where T : class
        {
            return new ValidatorBuilder<T>();
        }
    }

    /// <summary>
    /// Defines a class that provides a builder pattern for validators.
    /// </summary>
    public class ValidatorBuilder<T>
        where T : class
    {
        private AbstractValidator<T> Validator { get; } = new InlineValidator<T>();
        public ValidatorBuilder<T> Rules(params Func<AbstractValidator<T>, object>[] rules)
        {
            foreach (var r in rules)
            {
                r(Validator);
            }
            return this;
        }

        public IVerify<T> Build()
        {
            return new FluentValidatorWrapper<T>(Validator);
        }
    }
}
