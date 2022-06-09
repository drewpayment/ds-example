using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.User
{
    /// <summary>
    /// This class is used to define associations between action and legacy user types.
    /// </summary>
    public class UserActionTypeLegacyUserType : Entity<UserActionTypeLegacyUserType>
    {
        public virtual int UserActionTypeLegacyUserTypeId { get; set; }

        public virtual int UserActionTypeId { get; set; }
        public virtual UserActionType UserActionType { get; set; }

        public virtual int LegacyUserTypeId { get; set; }

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