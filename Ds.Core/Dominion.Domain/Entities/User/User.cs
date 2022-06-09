using System;
using System.Collections.Generic;

using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Api;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Entities.Security;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.PerformanceReviews;

namespace Dominion.Domain.Entities.User
{
    /// <summary>
    /// Container class for a user entity.
    /// </summary>
    public class User : Entity<User>
    {
        // this date was selected somewhat arbitrarily
        public static readonly DateTime MAX_TEMP_ENABLE_DATE = new DateTime(2112, 1, 1);

        public virtual int UserId { get; set; }

        /// <summary>
        /// ID for the user's auth record.
        /// </summary>
        public virtual int? AuthUserId { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string UserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string EmailAddress { get; set; }

        public virtual int? SecretQuestionId { get; set; }
        public virtual SecretQuestion SecretQuestion { get; set; }
        public virtual string SecretQuestionAnswer { get; set; }

        public virtual UserType UserTypeId { get; set; }
        public virtual UserTypeInfo UserTypeInfo { get; set; }

        public virtual int? EmployeeId { get; set; }
        public virtual Employee.Employee Employee { get; set; }

        public virtual UserViewEmployeePayType ViewEmployeePayTypes { get; set; }
        public virtual UserViewEmployeePayType ViewEmployeeRateTypes { get; set; }

        public virtual bool IsSecurityEnabled { get; set; }
        public virtual bool IsPasswordEnabled { get; set; }
        public virtual bool IsUserDisabled { get; set; }
        public virtual bool IsTimeclockEnabled { get; set; }
        public virtual bool IsWageTaxHistoryEditable { get; set; }
        public virtual bool IsEmployeeSelfServiceViewOnly { get; set; }
        public virtual bool IsEmployeeSelfServiceOnly { get; set; }
        public virtual bool IsReportingOnly { get; set; }
        public virtual bool IsPayrollAccessBlocked { get; set; }
        public virtual bool IsHrBlocked { get; set; }
        public virtual bool? IsEmployeeAccessOnly { get; set; }
        public virtual bool IsApplicantTrackingAdmin { get; set; }
        public virtual bool IsEditGlEnabled { get; set; }
        public virtual bool IsAllowedToAddSystemAdmin { get; set; }

        public virtual bool MustChangePassword { get; set; }
        
        public virtual DateTime? TempEnableFromDate { get; set; }
        public virtual DateTime? TempEnableToDate { get; set; }

        public virtual int? LastModifiedByUserId { get; set; }
        public virtual User LastModifiedByUser { get; set; }
        public virtual int? TimeoutMinutes { get; set; }
        public virtual bool CanViewTaxPackets { get; set; }
        public virtual bool TimeclockAppOnly { get; set; }
        public virtual bool IsBillingAdmin { get; set; }
        public virtual bool IsArAdmin { get; set; }

        // Relational Properties
        public virtual ICollection<ApiAccountMapping> ApiAccountMapping { get; set; } // many-to-one;
        public virtual ICollection<UserChangeHistory> ChangeHistory { get; set; }
        public virtual ICollection<UserSupervisorSecuritySetting> UserSupervisorSecuritySettings { get; set; }

        public virtual ICollection<UserClient> UserClientSettings { get; set; }
        public virtual ICollection<UserSupervisorSecurityGroupAccess> UserSupervisorSecurityGroupAccess { get; set; }
        //public virtual ICollection<ApplicantPosting> ApplicantPostings { get; set; }

        /// <summary>
        /// The applicant record(s) associated with this user account.
        /// </summary>
        public virtual ICollection<Applicant> Applicants { get; set; }
        public virtual ICollection<MeetingAttendee> MeetingsAttended { get; set; }
        public virtual ICollection<Evaluation> PerformanceEvaluations { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }

        public virtual ICollection<Employee.Employee> Employees { get; set; }
        public virtual ICollection<EvaluationTemplate> EvaluationTemplates { get; set; }
        public virtual ICollection<ReviewOwner> ReviewOwners { get; set; }
        public virtual ICollection<UserBetaFeature> BetaFeatures { get; set; }
        public virtual UserPermissions Permissions { get; set; }
        public virtual ICollection<Notification.NotificationContact> NotificationContacts { get; set; }
        public virtual UserPin UserPin { get; set; }
        public virtual UserSession Session { get; set; }
    }
}