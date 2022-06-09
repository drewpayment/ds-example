using System;
using Dominion.Utility.Dto;
using System.Collections.Generic;

namespace Dominion.Core.Dto.User
{
    /// <summary>
    /// This is a container class for generic user info data.
    /// </summary>
    public class UserInfoDto : DtoObject
    {
        public int UserId { get; set; }
        public int? AuthUserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public UserType UserTypeId { get; set; }
        public int? LastEmployeeId { get; set; }
        public string LastEmployeeFirstName { get; set; }
        public string LastEmployeeMiddleInitial { get; set; }
        public string LastEmployeeLastName { get; set; }

        public UserSessionDto Session { get; set; }

        public int? LastClientId { get; set; }
        public string LastClientName { get; set; }
        public string LastClientCode { get; set; }

        public string EmailAddress { get; set; }

        public int? TimeoutMinutes { get; set; }

        /**
         * This is set from the User's related employee record and is used to determine 
         * if the user is an active onboarding user and should be send to ESS Onboarding.
         */
        public bool IsApplicantAdmin { get; set; }
        public bool IsInOnboarding { get; set; }
        public bool? CertifyI9 { get; set; }
        public bool? AddEmployee { get; set; }
        public bool IsBillingAdmin { get; set; }
        public bool IsHrBlocked { get; set; }

        public bool IsEmployeeSelfServiceViewOnly { get; set; }
        public bool IsArAdmin { get; set; }
        public bool IsAllowedToAddSystemAdmin { get; set; }
        public IEnumerable<UserBetaFeatureDto> BetaFeatures { get; set; }
    }

    public class UserSessionDto
    {
        public int UserId { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? LastClientId { get; set; }
        public int? LastEmployeeId { get; set; }
        public string IpAddress { get; set; }
        public int? InvalidLoginAttempts { get; set; }
    }

    public class UserProfileBaseDto
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public bool IsUserDisabled { get; set; }
    }

    public class UserProfileDto : UserProfileBaseDto
    {
        public int? AuthUserId { get; set; }

        // GENERAL
        public UserType UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? EmployeeId { get; set; }
        public Employee.EmployeeStatusType? EmployeeStatusType { get; set; }
        public Employee.EmployeeBasicDto Employee { get; set; }

        // ACCOUNT SETTINGS
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public string Email { get; set; }
        public bool? ForceUserPasswordReset { get; set; }
        public bool IsAccountEnabled { get; set; }

        // APPLICATION SETTINGS
        public int? SessionTimeout { get; set; } 
        public string UserPin { get; set; }
        public UserViewEmployeePayType ViewEmployeesType { get; set; }
        public UserViewEmployeePayType ViewRatesType { get; set; }
        public bool IsEssViewOnly { get; set; }
        public bool BlockHr { get; set; }
        public bool HasEssSelfService { get; set; }
        public bool HasEmployeeAccess { get; set; }
        public bool IsReportingAccessOnly { get; set; }
        public bool HasGLAccess { get; set; }
        public bool BlockPayrollAccess { get; set; }
        public bool HasTaxPacketsAccess { get; set; }
        public bool isApplicantTrackingAdmin { get; set; }
        public bool HasTimeAndAttAccess { get; set; }
        public bool IsEmployeeNavigatorAdmin { get; set; }
        public bool IsTimeclockAppOnly { get; set; }
        public bool? HasTempAccess { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class UpdateUserProfileAccountDisableRequest
    {
        public int UserId { get; set; }
        public bool? IsAccountEnabled { get; set; }
        public bool? IsDisabled { get; set; }
    }
}
