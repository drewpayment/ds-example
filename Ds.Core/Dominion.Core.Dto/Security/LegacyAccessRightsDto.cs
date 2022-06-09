using Dominion.Core.Dto.User;
using Dominion.Utility.Constants;

namespace Dominion.Core.Dto.Security
{
    /// <summary>
    /// DTO containing indication of which client account features are enabled.
    /// </summary>
    public class LegacyAccessRightsDto
    {
        #region PROPERTIES & VARIABLES

        public int UserId { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }

        public UserType UserType { get; set; }

        public bool IsApplicantTrackingEnabled { get; set; }
        public bool IsEmployeeChangeRequestEnabled { get; set; }
        public bool IsEmployeeTaxChangeRequestEnabled { get; set; }
        public bool IsApplicantTrackingHiringWorkflowEnabled { get; set; }
        public bool IsPayrollAccessEnabled { get; set; }
        public bool IsTimeClockEnabled { get; set; }
        public byte? IdleTimeoutValue { get; set; }
        public bool IsBenefitPortalEnabled { get; set; }

        public bool HasEmployeeTimeClockConfigured { get; set; }

        public bool IsUserEmployeeSelfServiceOnly { get; set; }
        public bool IsUserEmployeeOnly { get; set; }
        public bool IsUserReportingOnly { get; set; }
        public bool IsUserPayrollAccessEnabled { get; set; }
        public bool IsUserTimeClockEnabled { get; set; }
        public bool IsRequestNobscotExitInterviewsEnabled { get; set; }
        public bool IsOnboardingEnabled { get; set; }
        public bool IsApplicantAdmin { get; set; }
        public bool IsGoalTrackingEnabled { get; set; }
        public bool IsPerformanceReviewsEnabled { get; set; }

        public bool IsEmployeeAssignedWithTimePolicy { get; set; }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Instantiates a new LegacyAccessRightsDto with default access rights set to NoAccess.
        /// </summary>
        public LegacyAccessRightsDto()
        {
            Init(false);
        }

        /// <summary>
        /// Instantiates a new LegacyAccessRightsDto with the specified default access.
        /// </summary>
        /// <param name="defaultAccess">Default access level to set all permissions to.</param>
        public LegacyAccessRightsDto(bool defaultAccess)
        {
            Init(defaultAccess);
        }

        /// <summary>
        /// Initializes the permissions properties with the specified access level.
        /// </summary>
        /// <param name="defaultAccess">Access level to set all permissions to.</param>
        private void Init(bool defaultAccess)
        {
            this.HasEmployeeTimeClockConfigured = defaultAccess;
            this.IsApplicantTrackingEnabled = defaultAccess;
            this.IsEmployeeChangeRequestEnabled = defaultAccess;
            this.IsEmployeeTaxChangeRequestEnabled = defaultAccess;
            this.IsApplicantTrackingHiringWorkflowEnabled = defaultAccess;
            this.IsPayrollAccessEnabled = defaultAccess;
            this.IsTimeClockEnabled = defaultAccess;
            this.IsBenefitPortalEnabled = defaultAccess;
            this.IdleTimeoutValue = CommonConstants.DEFAULT_SYSTEM_WIDE_TIMEOUT_MINUTES;
            this.IsRequestNobscotExitInterviewsEnabled = defaultAccess;
            this.IsOnboardingEnabled = defaultAccess;
            this.IsApplicantAdmin = false;
            this.IsGoalTrackingEnabled = defaultAccess;
            this.IsPerformanceReviewsEnabled = defaultAccess;
            this.IsEmployeeAssignedWithTimePolicy = defaultAccess;			
        }

        #endregion
    }
}