using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dominion.Domain.Validation;
using Dominion.Utility.Containers;
using Dominion.Utility.OpResult;

namespace Dominion.Domain.Interfaces.Entities
{
    public interface IEntity<TEntity> : IValidatableObject where TEntity : class
    {
        /// <summary>
        /// List of properties to validate when Entity.Validate() is performed
        /// </summary>
        [NotMapped]
        PropertyList<TEntity> PropertiesToValidate { get; set; }

        /// <summary>
        /// Set the validation rules for this object.
        /// </summary>
        /// <param name="validator">The validation rules.</param>
        /// <param name="properties">The properties to validate. null or empty indicates that all properties
        /// should be validated.</param>
        void SetValidator(EntityValidator<TEntity> validator, PropertyList<TEntity> properties = null);

        /// <summary>
        /// Validate all property values.
        /// </summary>
        /// <returns>Set of validation errors, if there are any.</returns>
        IEnumerable<ValidationResult> Validate();

        /// <summary>
        /// Entity validation that is done at the domain level.
        /// Validation was re-done and until the older validation is phased out, we will be calling this function.
        /// </summary>
        /// <returns></returns>
        IOpResult ConfirmValidity();
    }
}