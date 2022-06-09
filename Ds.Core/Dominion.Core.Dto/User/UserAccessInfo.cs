using System.Collections.Generic;

namespace Dominion.Core.Dto.User
{
    public class UserAccessInfo
    {
        public int?                    UserId                   { get; set; }
        public int?                    AuthUserId               { get; set; }
        public string                  Username                 { get; set; }
        public int?                    LastClientId             { get; set; }
        public int?                    LastEmployeeId           { get; set; }
        public int?                    EmployeeId               { get; set; }
        public int?                    EmployeeClientId         { get; set; }
        public bool                    IsApplicantTrackingAdmin { get; set; }
        public UserType                UserType                 { get; set; }
        public bool                    IsReportingOnly          { get; set; }
        public bool                    IsAnonymous              { get; set; }
        public bool                    IsBlockHr                { get; set; }
        public bool                    IsEssViewOnly            { get; set; }
        public bool                    IsEssOnly                { get; set; }
        public bool                    HasApplicantRecord       { get; set; }
        public bool                    IsPayrollAccessBlocked   { get; set; }
        public bool?                   IsEmployeeAccessOnly     { get; set; }
        public bool                    IsTimeclockEnabled       { get; set; }
        public bool                    IsEditGl                 { get; set; }
        public bool                    IsBillingAdmin           { get; set; }
        public UserViewEmployeePayType ViewEmployeePayTypes     { get; set; }
        public UserViewEmployeePayType ViewEmployeeRateTypes    { get; set; }
        public bool                    IsEmployeeNavigatorAdmin { get; set; }

        public IEnumerable<ClaimTypeDto>        Claims               { get; set; }
        public IEnumerable<UserClientAccessDto> UserClients          { get; set; }
        public UserSupervisorAccessInfo         SupervisorAccessInfo { get; set; }

        public ClientAccessInfo ClientAccessInfo { get; set; }
    }
}