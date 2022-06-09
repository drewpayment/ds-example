using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Dominion.Utility.Containers;
using Dominion.Utility.Msg.Identifiers;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query.LinqKit;

using FluentValidation;
using FluentValidation.Results;

namespace Dominion.Utility.Validation.FluentValidate
{
    /// <summary>
    /// Base class for all entity validators (at this time).
    /// At this time FluentValidation is our validation framework of choice.
    /// For examples see find examples of validators deriving from this abstract class.
    /// EXAMPLE: Entity Validators (base class)
    /// </summary>
    public abstract class FluentValidator<T> : AbstractValidator<T>, IVerify<T>
        where T : class
    {
        #region Variables and Properties

        /// <summary>
        /// A list of properties you want to validate.
        /// </summary>
        public PropertyList<T> PropertiesToValidate { get; set; }

        /// <summary>
        /// Get the reason that validation is being performed.
        /// </summary>
        protected ValidationReason Reason { get; private set; }
        
        #endregion

        #region Constructor

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed.</param>
        protected FluentValidator(ValidationReason reason = ValidationReason.Default)
        {
            Reason = reason;
            this.CascadeMode = CascadeMode.StopOnFirstFailure;
        }

        #endregion

        #region IVerify<T> Methods

        /// <summary>
        /// Verifies the object against pre-defined validation rules.
        /// </summary>
        /// <param name="obj">Object to verify.</param>
        /// <returns></returns>
        public IOpResult Verify(T obj)
        {
            return BuildFluentValidationIOpResult(obj, this);
        }

        /// <summary>
        /// Verifies only the specified properties for the object against pre-defined validation rules.
        /// </summary>
        /// <param name="obj">Object to verify.</param>
        /// <param name="properties">Properties to verify.</param>
        /// <returns></returns>
        public IOpResult Verify(T obj, params Expression<Func<T, object>>[] properties)
        {
            return BuildFluentValidationIOpResult(obj, this.SelectPropertiesToValidate(properties));
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Converts the results from the fluent validator to IOpResult messages.
        /// </summary>
        /// <param name="obj">The object you're validating.</param>
        /// <param name="validator">The validator object.</param>
        /// <returns></returns>
        public static IOpResult BuildFluentValidationIOpResult(T obj, IVerify<T> validator)
        {
            var result = new OpResult.OpResult();
            var fluentValidator = validator as AbstractValidator<T>;

            if((validator.PropertiesToValidate == null) || (validator.PropertiesToValidate.Count == 0))
            {
                var results = fluentValidator.Validate(obj);
                var errorMessages = ConvertResults(results);
                result.AddMessages(errorMessages);
            }
            else
            {
                var results = fluentValidator.Validate(
                    obj, 
                    validator.PropertiesToValidate.GetPropertyNames().ToArray());
                
                var errorMessages = ConvertResults(results);
                result.AddMessages(errorMessages);
            }

            return result.SetSuccessBasedOnMessageCount();
        }


        /// <summary>
        /// Convert the fluent results to our generic property validator result.
        /// </summary>
        public static IEnumerable<PropertyValidationMsg> ConvertResults(ValidationResult fluentResult)
        {
            var errorList = new List<PropertyValidationMsg>();

            foreach (var failure in fluentResult.Errors)
            {
                var obj = new PropertyValidationMsg()
                {
                    AttemptedValue = failure.AttemptedValue, 
                    Message = failure.ErrorMessage, 
                    PropertyName = failure.PropertyName, 
                    CustomState =
                        (failure.CustomState != null && failure.CustomState is EntityValidationTypes)
                            ? ((EntityValidationTypes) failure.CustomState)
                            : EntityValidationTypes.General
                };

                errorList.Add(obj); 
            }

            return errorList;
        }

        #endregion

    }
}
