using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.User;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Utility.Containers;

namespace Dominion.Domain.Entities.Clients
{
    /// <summary>
    /// Container for client user group names
    /// </summary>
    public class ClientUserGroupActionType :
        Entity<ClientUserGroupActionType>, 
        IModifiableEntity<ClientUserGroupActionType>
    {
        public virtual int ClientUserGroupActionTypeId { get; set; }

        public virtual int ClientUserGroupId { get; set; }
        public virtual ClientUserGroup ClientUserGroup { get; set; }

        public virtual int UserActionTypeId { get; set; }
        public virtual UserActionType UserActionType { get; set; }

        // IModifiableEntity Properties
        public virtual int LastModifiedByUserId { get; set; }
        public virtual User.User LastModifiedByUser { get; set; }
        public virtual DateTime LastModifiedDate { get; set; }

        public virtual string LastModifiedByDescription
        {
            get { return LastModifiedByUserId.ToString(); }
        }

        public void SetLastModifiedValues(int lastModifiedByUserId, string lastModifiedByUserName, 
            DateTime lastModifiedDate)
        {
            LastModifiedByUserId = lastModifiedByUserId;
            LastModifiedDate = lastModifiedDate;
        }

        #region IValidatableObject IMPLEMENTATION

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

            // check the modified date/modified by properties.
            validationErrors.AddRange(this.ValidateModifiableEntityByUserIdProperties());

            return validationErrors;
        }

        #endregion // IValidatableObject MEMBERS

        #region PROPERTY LIST STATIC METHODS

        /// <summary>
        /// List of the entity's key properties.  
        /// </summary>
        public new static PropertyList<ClientUserGroupActionType> EntityKeyProperties
        {
            get
            {
                return new PropertyList<ClientUserGroupActionType>
                {
                    x => x.ClientUserGroupActionTypeId
                };
            }
        }

        /// <summary>
        /// List of the entity's relationship Ids (foreign keys) used to 
        /// access Navigation Properties.
        /// </summary>
        public new static PropertyList<ClientUserGroupActionType> RequiredEntityRelationshipIdProperties
        {
            get
            {
                return new PropertyList<ClientUserGroupActionType>
                {
                    x => x.ClientUserGroupId, 
                    x => x.UserActionTypeId
                };
            }
        }

        /// <summary>
        /// List of the entity's required properties that could be set to 
        /// null due to the property's type (eg. strings).
        /// </summary>
        public new static PropertyList<ClientUserGroupActionType> RequiredNullableProperties
        {
            get { return new PropertyList<ClientUserGroupActionType>(); }
        }

        #endregion //  PROPERTY LIST STATIC METHODS
    }
}