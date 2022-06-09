using System;
using System.Linq.Expressions;

using Dominion.Utility.Containers;
using Dominion.Utility.Messaging;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query.LinqKit;
using Dominion.Utility.Validation;
using Dominion.Utility.Validation.FluentValidate;
using FluentValidation;

namespace Dominion.Domain.Validation
{
    /// <summary>
    /// Base class for all entity validators.
    /// </summary>
    [Obsolete("We no longer add validation on the entity. We will no longer use EntityValidator<T>. See: Dominion.Utility.Validation.FluentValidate.FluentValidator<T>. ")]
    public abstract class EntityValidator<TEntity> : AbstractValidator<TEntity>, IVerify<TEntity> 
        where TEntity : class
    {
        public const string RULE_SET_REQUIRED_PROPERTIES = "RequiredProperties";
        public const int MINIMUM_NAVIGATIONAL_ID = 1;

        public static readonly DateTime MAX_REALISTIC_DATE = new DateTime(2999, 12, 31);

        // this date was selected somewhat arbitrarily
        public static readonly DateTime MIN_REALISTIC_DATE = new DateTime(1885, 9, 2);

        PropertyList<TEntity> IVerify<TEntity>.PropertiesToValidate { get; set; }

        // this date was selected somewhat arbitrarily

        /// <summary>
        /// Used to set the 'state' of a property validator rule.
        /// This concept was copied over from robs design just done differently.
        /// </summary>
        protected readonly Func<TEntity, object> _requiredContext;


        /// <summary>
        /// Get the reason that validation is being performed.
        /// </summary>
        protected ValidationReason Reason { get; private set; }

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="reason">The reason that validation is being performed.</param>
        [Obsolete("We no longer add validation on the entity. We will no longer use EntityValidator<T>. See: Dominion.Utility.Validation.FluentValidate.FluentValidator<T>. ")]
        public EntityValidator(ValidationReason reason = ValidationReason.Default)
        {
            Reason = reason;
            this.CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;
            _requiredContext = x => ValidationStatusMessageType.Required;

            Rules();
        }

        /// <summary>
        /// Verifies the object against pre-defined validation rules.
        /// </summary>
        /// <param name="obj">Object to verify.</param>
        /// <returns></returns>
        IOpResult IVerify<TEntity>.Verify(TEntity obj)
        {
            return FluentValidator<TEntity>.BuildFluentValidationIOpResult(obj, this);
        }

        /// <summary>
        /// Verifies only the specified properties for the object against pre-defined validation rules.
        /// </summary>
        /// <param name="obj">Object to verify.</param>
        /// <param name="properties">Properties to verify.</param>
        /// <returns></returns>
        IOpResult IVerify<TEntity>.Verify(TEntity obj, params Expression<Func<TEntity, object>>[] properties)
        {
            return FluentValidator<TEntity>.BuildFluentValidationIOpResult(obj, this.SelectPropertiesToValidate(properties));
        }

        /// <summary>
        /// Validation rule definitions for the entity.
        /// </summary>
        protected abstract void Rules();

    }
}