using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Validation.FluentValidate
{
    using System.Linq.Expressions;

    using Dominion.Utility.Containers;
    using Dominion.Utility.OpResult;

    using FluentValidation;

    public class FluentValidatorWrapper<T> : IVerify<T>
        where T : class
    {
        private readonly AbstractValidator<T> _validator;

        public FluentValidatorWrapper(AbstractValidator<T> validator)
        {
            this._validator = validator;
        }

        public IOpResult Verify(T obj)
        {
            var results = this._validator.Validate(obj);
            var messages = FluentValidator<T>.ConvertResults(results);
            var result = new OpResult();
            result.AddMessages(messages);
            return result;
        }

        public IOpResult Verify(T obj, params Expression<Func<T, object>>[] properties)
        {
            var results = this._validator.Validate(obj, properties);
            var messages = FluentValidator<T>.ConvertResults(results);
            var result = new OpResult();
            result.AddMessages(messages);
            return result;
        }

        public PropertyList<T> PropertiesToValidate { get; set; }
    }
}
