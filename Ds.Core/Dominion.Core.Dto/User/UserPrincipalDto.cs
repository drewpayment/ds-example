using System.Collections.Generic;

namespace Dominion.Core.Dto.User
{
    /// <summary>
    /// User info used to create a new principal object.
    /// </summary>
    public class UserPrincipalDto
    {
        public int?     AuthUserId               { get; set; }
        public int      UserId                   { get; set; }
        public string   Username                 { get; set; }
        public int?     LastClientId             { get; set; }
        public int?     LastEmployeeId           { get; set; }
        public int?     EmployeeId               { get; set; }
        public int?     EmployeeClientId         { get; set; }
        public bool     IsApplicantTrackingAdmin { get; set; }
        public UserType UserType                 { get; set; }
        public bool     IsReportingOnly          { get; set; }
        public bool     IsAnonymous              { get; set; }
        public bool     IsBlockHr                { get; set; }
        public bool     IsEssViewOnly            { get; set; }
        public bool     CertifyI9                { get; set; }
        public bool     IsPayrollAccessBlocked   { get; set; }
        public bool     AddEmployee              { get; set; }
        public bool     IsEditGlEnabled          { get; set; }
        public bool     IsBillingAdmin           { get; set; }
        public bool CanAddSystemAdmins { get; set; }

        //POSSIBLE MERGE ISSUE: NEXT 2 LINES: MAY2
        public UserViewEmployeePayType ViewEmployeeRateTypes { get; set; }
        public UserViewEmployeePayType ViewEmployeePayTypes { get; set; }
        public IEnumerable<int> AccessibleClientIds { get; set; }
        public UserPermissionsDto Permissions { get; set; }
    }
}
