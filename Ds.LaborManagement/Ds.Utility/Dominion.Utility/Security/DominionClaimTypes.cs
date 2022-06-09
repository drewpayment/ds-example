using Dominion.Utility.Transform;

namespace Dominion.Utility.Security
{
    public static class DominionClaimTypes
    {
        public const string UserId = "https://live.dominionsystems.com/identity/claims/userid";
        public const string ClientId = "https://live.dominionsystems.com/identity/claims/clientid";
        public const string EmployeeId = "https://live.dominionsystems.com/identity/claims/employeeid";
        public const string UserTypeId = "https://live.dominionsystems.com/identity/claims/usertypeid";
        public const string UserName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        /// <summary>
        /// I DON'T THINK WE NEED THESE
        /// </summary>
        public const string AuthUserId = "https://live.dominionsystems.com/identity/claims/authUserid";
        public const string UserStatus = "https://live.dominionsystems.com/identity/claims/userStatus";
        public const string AuthPhone = "https://live.dominionsystems.com/identity/claims/phone";
        public const string AuthEmail = "https://live.dominionsystems.com/identity/claims/email";
        public const string SiteConfigId = "https://live.dominionsystems.com/identity/claims/siteConfigId";
        public const string MobileOverride = "https://live.dominionsystems.com/identity/claims/ismobileoverride";
        public const string HomeSite = "https://live.dominionsystems.com/identity/claims/homesite";
    }
}