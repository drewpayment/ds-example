using System;
using System.Collections.Generic;
using Dominion.Utility.Dto;

namespace Dominion.Core.Dto.User
{
    /// <summary>
    /// This is a container class for generic user data that is intended to service user creation
    /// and update actions.
    /// </summary>
    public class UserDto : DtoObject
    {
        public int UserId { get; set; }
        public int AuthUserId { get; set; }
        public UserType UserTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public int SecretQuestionId { get; set; }
        public string SecretQuestionAnswer { get; set; }

        public int EmployeeId { get; set; }
        public int LastEmployeeId { get; set; }
        public int LastClientId { get; set; }
        public int LastModifiedByUserId { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string IpAddress { get; set; }
        public UserViewEmployeePayType ViewEmployeePayTypes { get; set; }
        public UserViewEmployeePayType ViewEmployeeRateTypes { get; set; }
        public bool CanViewTaxPackets { get; set; }
        public bool MustChangePassword { get; set; }
        public bool TimeClockAppOnly { get; set; }

        public bool IsSecurityEnabled { get; set; }
        public bool IsPasswordEnabled { get; set; }
        public bool IsTimeclockEnabled { get; set; }
        public bool IsWageTaxHistoryEditable { get; set; }
        public bool IsEmployeeSelfServiceViewOnly { get; set; }
        public bool IsEmployeeSelfServiceOnly { get; set; }
        public bool IsReportingOnly { get; set; }
        public bool IsPayrollAccessBlocked { get; set; }
        public bool IsHrBlocked { get; set; }
        public bool IsEmployeeAccessOnly { get; set; }
        public bool IsApplicantTrackingAdmin { get; set; }
        public bool IsEditGlEnabled { get; set; }
        public bool IsAllowedToAddSystemAdmin { get; set; }


        public DateTime? TempEnableFromDate { get; set; }
        public DateTime? TempEnableToDate { get; set; }
        public int TimeoutMinutes { get; set; }

        public bool IsUserDisabled { get; set; }

        public UserPermissionsDto Permissions { get; set; }
        public ICollection<UserClientDto> UserClientSettings { get; set; }
    } // class UserDto

    public class UserProfileNameDto
    {
        public int UserId { get; set; }
        public string FriendlyName { get; set; }
    }
}