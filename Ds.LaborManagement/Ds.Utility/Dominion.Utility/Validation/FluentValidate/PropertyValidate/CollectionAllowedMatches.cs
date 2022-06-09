using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Utility.Containers;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace Dominion.Utility.Validation.FluentValidate.PropertyValidate
{
    /// <summary>
    /// Test a collection property.
    /// Counts the number of matches based on a provided predicate.
    /// Compares the count to the allowed number specified.
    /// </summary>
    /// <typeparam name="T">The object type that contains the collection.</typeparam>
    public class CollectionAllowedMatches<T> : PropertyValidator
        where T : class
    {
        private readonly Expression<Func<T, bool>> _criteria;
        private readonly int _numberAllowedNull;
 
        public CollectionAllowedMatches(Expression<Func<T, bool>> criteria, int numberAllowedNull) 
            : base(string.Empty)
        {
            _criteria = criteria;
            _numberAllowedNull = numberAllowedNull;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var list = context.PropertyValue as IList<T>;

            if(list != null)
            {
                var matchedCount = list.Count(_criteria.Compile().Invoke);

                if(matchedCount > _numberAllowedNull)
                {
                    var propertyName = _criteria.GetPropertyInfo().Name;
                    
                    var msg = 
                        "List Property: " + 
                        context.PropertyName +
                        " contains more than " + 
                        _numberAllowedNull  + 
                        " null items for the: " + 
                        propertyName + 
                        " property!";

                    //lowFix: jay: add msg object instead of message. 
                    ErrorMessageSource = new StaticStringSource(msg);

                    return false;
                }
            }

            return true;
        }
    }

}
