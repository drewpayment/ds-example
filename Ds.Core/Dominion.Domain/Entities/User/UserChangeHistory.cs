using System;
using System.Linq.Expressions;

using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.User
{
    public class UserChangeHistory : Entity<UserChangeHistory>
    {
        public virtual int ChangeHistoryId { get; set; }
        public virtual DateTime ChangeDate { get; set; }
        public virtual string ChangeMode { get; set; }

        public virtual int UserId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Username { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string EmailAddress { get; set; }

        public virtual int? SecretQuestionId { get; set; }
        public virtual string SecretQuestionAnswer { get; set; }

        public virtual UserType UserTypeId { get; set; }

        public virtual int? EmployeeId { get; set; }
        public virtual int? LastEmployeeId { get; set; }
        public virtual int? LastClientId { get; set; }
        public virtual UserViewEmployeePayType ViewEmployeePayTypes { get; set; }
        public virtual UserViewEmployeePayType ViewEmployeeRateTypes { get; set; }

        public virtual DateTime? LastLoginDate { get; set; }
        public virtual DateTime? TemporaryEnableFromDate { get; set; }
        public virtual DateTime? TemporaryEnableToDate { get; set; }

        public virtual bool? IsSecurityEnabled { get; set; }
        public virtual bool IsPasswordEnabled { get; set; }
        public virtual bool IsUserDisabled { get; set; }
        public virtual bool? IsTimeclockEnabled { get; set; }
        public virtual bool? IsEmployeeSelfServiceViewOnly { get; set; }
        public virtual bool? IsEmployeeSelfServiceOnly { get; set; }
        public virtual bool? IsReportingOnly { get; set; }
        public virtual bool? IsPayrollAccessBlocked { get; set; }
        public virtual bool? IsHrBlocked { get; set; }
        public virtual bool? IsEmployeeAccessOnly { get; set; }
        public virtual bool IsApplicantTrackingAdmin { get; set; }
        public virtual bool? IsEditGlEnabled { get; set; }
        public virtual bool IsAllowedToAddSystemAdmin { get; set; }

        public virtual int? ModifiedBy { get; set; }
        public virtual string IpAddress { get; set; }

        // Relational Properties
        public virtual User User { get; set; }

        public UserChangeHistory()
        {
        }

        #region Filters

        /// <summary>
        /// Returns a specification expression which checks if a particular change is for the specified user.
        /// </summary>
        /// <param name="userId">ID of the user to check.</param>
        /// <returns>Specification expression which can be used in data queries.</returns>
        public static Expression<Func<UserChangeHistory, bool>> ForUser(int userId)
        {
            return change => change.UserId == userId;
        }

        #endregion
    }
}