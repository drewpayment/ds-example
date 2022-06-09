using System;
using Dominion.Utility.Transform;

namespace Dominion.Utility.Constants
{
    public class CommonConstants
    {
        #region ORIGINAL

        public const string SINGLE_SPACE = " ";
        public const string COLON_SPACE = ": ";
        public const string DASH = "-";
        public const string PERIOD = ".";
        public const string COMMA_SPACE = ", ";
        public const string EMPTY_STRING = "";
        public const string LOWERCASE_X = "x";

        public const int DEFAULT_SYSTEM_ADMIN_TIMEOUT_MINUTES = 15;
        public const int DEFAULT_UNLIMITED_TIMEOUT_MINUTES = 0;
        public const int DEFAULT_SYSTEM_WIDE_TIMEOUT_MINUTES = 15;
        public const int DEFAULT_TIMEOUT_MULTIPLIER = 60000;

        public const int US_COUNTRY_ID = 1;
        public const int MI_STATE_ID = 1;
        public const int SEC_QUESTION_MAIDEN_NAME =1;
        public const int NEW_ENTITY_ID = 0;
        public const int NO_VALUE_SELECTED = int.MinValue;
        public const string IP_LOCAL_IP4 = "127.0.0.1";
        public const string IP_LOCAL_IP6 = "::1";
        public const string IP_ERROR = "ERROR";
        public const string IP_NOT_FOUND = "NOT FOUND";
        public const string IP_TESTING = "TESTING";

        public const string SVR_VARS_HTTP_X_FORWARDED_FOR = "HTTP_X_FORWARDED_FOR";

        /// <summary>
        /// ONLY USE THIS IF YOU HAVE TO.
        /// Use DateTime.Min when ever possible.
        /// </summary>
        public static readonly DateTime NO_DATE_SELECTED_DT = new DateTime(
            year: 1900,
            month: 1,
            day: 1);

        public const string NO_DATE_SELECTED = "1/1/1900";

        /// <summary>
        /// The text of the sub check field for a manual check (from paycheck calculator).
        /// </summary>
        public const string MANUAL_CHECK = "~Manual~Check~";
        #endregion

        #region FROM AUTH

        public const string NO_IP_AVAILABLE = "NO IP";
        public const string BUILD_CONDITION_DEBUG = "DEBUG";
        public const string JAYS_MACHINE_NAME_DEBUGGING = "DOM-WS078"; //Mike Guy's machine name

        ///// <summary>
        ///// ORIGINAL CLAIMS
        ///// </summary>
        //public const string CLAIM_KEY_USERID = "https://live.dominionsystems.com/identity/claims/userid";
        //public const string CLAIM_KEY_EEID = "https://live.dominionsystems.com/identity/claims/employeeid";
        //public const string CLAIM_KEY_USRTYPE = "https://live.dominionsystems.com/identity/claims/usertypeid";
        //public const string CLAIM_KEY_HOME = "https://live.dominionsystems.com/identity/claims/homesite";

        ////NEW FOR AUTH
        //public const string CLAIM_KEY_AUTH_USERID = "https://live.dominionsystems.com/identity/claims/authUserid";
        //public const string CLAIM_KEY_AUTH_PHONE = "https://live.dominionsystems.com/identity/claims/phone";
        //public const string CLAIM_KEY_AUTH_EMAIL = "https://live.dominionsystems.com/identity/claims/email";
        //public const string CLAIM_KEY_AUTH_SITE_CONFIG_ID = "https://live.dominionsystems.com/identity/claims/siteConfigId";

        ///// <summary>
        ///// I DON'T THINK WE NEED THESE
        ///// </summary>
        //public const string CLAIM_KEY_MOBILE = "https://live.dominionsystems.com/identity/claims/ismobileoverride";
        //public const string CLAIM_KEY_USER_STATUS = "https://live.dominionsystems.com/identity/claims/userStatus";

        //public const string AuthUserId = "https://live.dominionsystems.com/identity/claims/authUserid";
        //public const string UserId = "https://live.dominionsystems.com/identity/claims/userid";
        //public const string ClientId = "https://live.dominionsystems.com/identity/claims/clientid";
        //public const string EmployeeId = "https://live.dominionsystems.com/identity/claims/employeeid";
        //public const string UserTypeId = "https://live.dominionsystems.com/identity/claims/usertypeid";
        //public const string UserName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        /// <summary>
        /// This claim's value will be one of these options: <see cref="SocialSecurityNumberMaskOption "/> (enum)
        /// </summary>
        public const string SocialSecurityMaskOption = "https://live.dominionsystems.com/identity/claims/ssnmaskoption";


        /// <summary>
        /// The ID of the sys default user is -1.
        /// </summary>
        public const int SYS_DEFAULT_AUTH_USER_ID = -1;
        public const int SYS_TEMP_AUTH_USER_ID = -2;
        public const int SYS_DB_ID = 3;

        //public const string NO_SPACES_IN_TEXT = @"^(?!.*\s).*$";
        //public const string ONE_UPPER_CASE_AND_ONE_LOWER_CASE = @"((?=.*[A-Z])).*((?=.*[a-z]))";

        #endregion

    }
}