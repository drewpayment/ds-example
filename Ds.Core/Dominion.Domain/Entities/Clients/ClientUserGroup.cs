using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Utility.Containers;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientUserGroup : Entity<ClientUserGroup>, IModifiableEntity<ClientUserGroup>
    {
        public virtual int ClientUserGroupId { get; set; }

        /// <summary>
        /// ID of the client with which the user group is associated.
        /// </summary>
        /// <remarks>
        /// Null indicates a static system group that is available to all clients.
        /// </remarks>
        public virtual int? ClientId { get; set; }

        public virtual Client Client { get; set; }

        public virtual string Name { get; set; }

        // IModifiableEntity Properties
        public virtual int LastModifiedByUserId { get; set; }
        public virtual User.User LastModifiedByUser { get; set; }
        public virtual DateTime LastModifiedDate { get; set; }

        public virtual string LastModifiedByDescription
        {
            get { return LastModifiedByUserId.ToString(); }
        }

        public void SetLastModifiedValues(
            int lastModifiedByUserId, 
            string lastModifiedByUserName, 
            DateTime lastModifiedDate)
        {
            LastModifiedByUserId = lastModifiedByUserId;
            LastModifiedDate = lastModifiedDate;
        }

        public virtual ICollection<ClientUserGroupActionType> GroupActions { get; set; }

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

        #endregion

        #region PROPERTY LIST STATIC METHODS

        /// <summary>
        /// List of the entity's key properties.  
        /// </summary>
        public new static PropertyList<ClientUserGroup> EntityKeyProperties
        {
            get
            {
                return new PropertyList<ClientUserGroup>
                {
                    x => x.ClientUserGroupId
                };
            }
        }

        /// <summary>
        /// List of the entity's relationship Ids (foreign keys) used to 
        /// access Navigation Properties.
        /// </summary>
        public new static PropertyList<ClientUserGroup> RequiredEntityRelationshipIdProperties
        {
            get { return new PropertyList<ClientUserGroup>(); }
        }

        /// <summary>
        /// List of the entity's required properties that could be set to 
        /// null due to the property's type (eg. strings).
        /// </summary>
        public new static PropertyList<ClientUserGroup> RequiredNullableProperties
        {
            get
            {
                return new PropertyList<ClientUserGroup>
                {
                    x => x.Name
                };
            }
        }

        #endregion

        #region Filters

        /// <summary>
        /// Determine whether a user group is a system user group.
        /// </summary>
        /// <returns>True if the user group is a system user group</returns>
        public static Expression<Func<ClientUserGroup, bool>> IsSystemUserGroup()
        {
            return userGroup => userGroup.ClientId == null;
        }

        /// <summary>
        /// Determine whether a user group is a system user group with the given name.
        /// </summary>
        /// <param name="userGroupName">The target system user group name.</param>
        /// <returns>True if the user group is a system user group and hase the given name.</returns>
        public static Expression<Func<ClientUserGroup, bool>> IsSystemUserGroup(string userGroupName)
        {
            return userGroup => userGroup.ClientId == null && userGroup.Name == userGroupName;
        }

        #endregion
    }
}