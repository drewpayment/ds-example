using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.User
{
    /// <summary>
    /// Container for client user group names
    /// </summary>
    public class UserGroupMembership : Entity<UserGroupMembership>, IModifiableEntity<UserGroupMembership>
    {
        public virtual int UserGroupMembershipId { get; set; }

        public virtual int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual int ClientUserGroupId { get; set; }
        public virtual ClientUserGroup ClientUserGroup { get; set; }

        // IModifiableEntity Properties
        public virtual int LastModifiedByUserId { get; set; }
        public virtual User LastModifiedByUser { get; set; }
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

        #region IValidatableObject MEMBERS

        /// <summary>
        /// Validate the properties specified by PropertiesToValidate, or all if PropertiesToValidate is null or empty.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>Set of validation errors, if any.</returns>
        public override IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            var validationErrors = base.Validate(validationContext).ToList();

            return validationErrors;
        }

        // Validate()
        #endregion // IValidatableObject MEMBERS
    }
}