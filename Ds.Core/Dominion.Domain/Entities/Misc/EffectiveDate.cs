using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Misc
{
    public enum EffectiveDateTypes
    {
        EffectiveDateChangeRequest = 1, // ie: payroll pay change
        EmployeeChangeRequest = 2, // ie: employee changes emergency contact or adds new one
        PayrollRequest = 3, // ie: Performance payroll requests (merit increases)
        UserChangeRequest = 4
    }

    public class EffectiveDate : Entity<EffectiveDate>
    {
        /// <summary>
        /// Primary key for this entity.
        /// </summary>
        public virtual int EffectiveId { get; set; }

        /// <summary>
        /// Who this change is for.
        /// </summary>
        public virtual int EmployeeId { get; set; }


        /// <summary>
        /// The table in the database this change should be made to.
        /// </summary>
        public virtual string Table { get; set; }

        /// <summary>
        /// The column in the database this change should be made to.
        /// </summary>
        public virtual string Column { get; set; }

        /// <summary>
        /// The primary key of the column value that is changing.
        /// </summary>
        public virtual int TablePkId { get; set; }


        /// <summary>
        /// The value before the proposed change.
        /// </summary>
        public virtual string OldValue { get; set; }

        /// <summary>
        /// The value after the proposed change.
        /// </summary>
        public virtual string NewValue { get; set; }

        /// <summary>
        /// The data type of the value for the change.
        /// </summary>
        public virtual string Datatype { get; set; }


        /// <summary>
        /// The date this value is effective on if this is an effective date type.
        /// </summary>
        public virtual DateTime DateEffective { get; set; }

        /// <summary>
        /// The date it really became effective (requires user to make effective).
        /// Will be null if it isn't effective.
        /// </summary>
        public virtual DateTime? DateAppliedOn { get; set; }

        /// <summary>
        /// Who made this value effective?
        /// Stores the employee ID in string form.
        /// todo (jay):  find out if this is the user name or the employee name (my guess is user).
        /// </summary>
        public virtual string AppliedBy { get; set; }


        /// <summary>
        /// This is what a user will see when looking to make the change effective.
        /// </summary>
        public virtual string FriendlyView { get; set; }

        /// <summary>
        /// 1 (true) || 0 (false) || null (not processed)
        /// Was accepted and made effective.
        /// </summary>
        public virtual int? Accepted { get; set; }

        /// <summary>
        /// 1: effective date
        /// if 2: proposed property change 
        /// </summary>
        public virtual EffectiveDateTypes EffectiveDateType { get; set; }

        /// <summary>
        /// Who made this item?
        /// This is an employee ID stored as a string.
        /// </summary>
        public virtual string CreatedBy { get; set; }

        /// <summary>
        /// The date this item was made.
        /// </summary>
        public virtual DateTime DateCreatedOn { get; set; }
    }
}