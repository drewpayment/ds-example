using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dominion.Domain.Entities.Base;
using Dominion.Utility.Containers;

namespace Dominion.Domain.Entities.User
{
    /// <summary>
    /// Container for client user group names
    /// </summary>
    public class UserActionType : Entity<UserActionType>
    {
        public virtual int UserActionTypeId { get; set; }
        public virtual string Designation { get; set; }
        public virtual string Label { get; set; }
        public virtual ICollection<UserActionTypeLegacyUserType> LegacyUserTypes { get; set; }

        #region IValidatableObject MEMBERS

        /// <summary>
        /// Validate all properties.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>Set of validation errors, if any.</returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationErrors = base.Validate(validationContext).ToList();

            // check the required properties. 
            validationErrors.AddRange(this.ValidateRequiredProperties(RequiredNullableProperties));
            validationErrors.AddRange(this.ValidateRequiredIdProperties(RequiredEntityRelationshipIdProperties));

            return validationErrors;
        }

        #endregion // IValidatableObject MEMBERS

        #region PROPERTY LIST STATIC METHODS

        /// <summary>
        /// List of the entity's key properties.  
        /// </summary>
        public new static PropertyList<UserActionType> EntityKeyProperties
        {
            get
            {
                return new PropertyList<UserActionType>
                {
                    x => x.UserActionTypeId
                };
            }
        }

        /// <summary>
        /// List of the entity's relationship Ids (foreign keys) used to 
        /// access Navigation Properties.
        /// </summary>
        public new static PropertyList<UserActionType> RequiredEntityRelationshipIdProperties
        {
            get { return new PropertyList<UserActionType>(); }
        }

        /// <summary>
        /// List of the entity's required properties that could be set to 
        /// null due to the property's type (eg. strings).
        /// </summary>
        public new static PropertyList<UserActionType> RequiredNullableProperties
        {
            get
            {
                return new PropertyList<UserActionType>
                {
                    x => x.Designation, 
                    x => x.Label
                };
            }
        }

        #endregion //  PROPERTY LIST STATIC METHODS
    }
}