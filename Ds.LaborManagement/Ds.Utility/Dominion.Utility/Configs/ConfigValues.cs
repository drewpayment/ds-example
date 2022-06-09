using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.Configs
{
    public class ConfigValues
    {
        #region CONSTANTS

        public const string SECTION_APP_SETTINGS = "appSettings";


        /// <summary>
        /// Default dbo.ClientRelation ClientRelationID of the relation containing all demo/test clients in the system.
        /// </summary>
        public const int DEFAULT_DEMONSTRATION_CLIENT_RELATION_ID = 23;

        /// <summary>
        /// Default 'DemonstrationMode' app-setting value. Default is 'Production' indicating test/demo clients should 
        /// be excluded from certain queries (see IClientQuery.ExcludeDemonstrationClients).
        /// </summary>
        public const string DEFAULT_DEMONSTRATION_MODE = "Production";

        /// <summary>
        /// Allowed 'DemonstrationMode' app-setting values that will put the site into Test-mode allowing clients in the
        /// 'Demonstration Payroll' relationship to be included in relevant queries (see IClientQuery.ExcludeDemonstrationClients). 
        /// Current allowed values are 'Test' or 'Demo'.
        /// </summary>
        public static readonly string[] DemonstrationTestModeValues = {"Test", "Demo"};


        public const string FEEDBACK_EMAIL_KEY = "FeedbackEmail";
        public const string DEMONSTRATION_MODE_KEY = "DemonstrationMode";
        public const string DEMONSTRATION_CLIENT_RELATION_ID_KEY = "DemonstrationClientRelationId";

        /// <summary>
        /// Used in SessionManagedPage.vb
        /// </summary>
        public const string LOGIN_PAGE_URL_KEY = "LoginPageUrl";

        /// <summary>
        /// Used in SessionManagedPage.vb
        /// </summary>
        public const string LOGOUT_PAGE_URL_KEY = "LogoutPageUrl";

        #endregion

        #region APP SETTINGS

        #region ORIGINAL

        /// <summary>
        /// Root URL to the legacy web application. (key: "legacyRootUrl")
        /// </summary>
        public static string LegacyRootUrl => ConfigurationManager.AppSettings["legacyRootUrl"];

        /// <summary>
        /// Root URL to the phoenix ESS web application. (key: "PhoenixRootURL")
        /// </summary>
        public static string PhoenixRootUrl => ConfigurationManager.AppSettings[nameof(PhoenixRootUrl)];

        /// <summary>
        /// Alias of <see cref="PhoenixRootUrl"/> : Root URL to the phoenix ESS web application. (key: "PhoenixRootURL")
        /// </summary>
        public static string PhoenixEssUrl => PhoenixRootUrl;

        /// <summary>
        /// Root URL to the company web appliction. (key: "CompanyRootUrl")
        /// </summary>
        public static string CompanyRootUrl => ConfigurationManager.AppSettings["CompanyRootUrl"];

        public static string DemonstrationClientRelationId => ConfigurationManager.AppSettings["DemonstrationClientRelationId"] ?? "0";


        public static bool ActivateRedirection
            => ConfigurationManager.AppSettings[nameof(ActivateRedirection)].ConvertToBool();

        public static int NumberOfUriSegmentsToCheckForRedirection
            =>
            ConfigurationManager.AppSettings[nameof(NumberOfUriSegmentsToCheckForRedirection)]
                .ConvertToOrDefault<int>();

        public static string MainRedirectRootUrl
            => ConfigurationManager.AppSettings[nameof(MainRedirectRootUrl)];

        public static string StsServerBaseUrl
            => ConfigurationManager.AppSettings[nameof(StsServerBaseUrl)];

        public static string DominionDotCom = "https://www.dominionsystems.com";

        public static bool DebugSql => ConfigurationManager.AppSettings[nameof(DebugSql)].ConvertToBool();


        //StsServerBaseUrl

        public static byte SiteConfigurationID
            => ConfigurationManager.AppSettings[nameof(SiteConfigurationID)].ConvertToOrDefault<byte>(0);

        public static string ApplicationBasePath
            => ConfigurationManager.AppSettings[nameof(ApplicationBasePath)];

        /// <summary>
        /// Connection string for the sts server
        /// </summary>
        public static string ConnectionStringSTS
            => ConfigurationManager.AppSettings[nameof(ConnectionStringSTS)];

        /// <summary>
        /// highfix: auth: this is temporary; remove after testing
        /// </summary>
        public static bool SimulateMultiServerEnvironment
            => ConfigurationManager.AppSettings[nameof(SimulateMultiServerEnvironment)].ConvertToBool();

        /// <summary>
        /// Root folder directory where various export files are placed.
        /// </summary>
        public static string ExportDirectory => ConfigurationManager.AppSettings[nameof(ExportDirectory)];

        /// <summary>
        /// Microsoft OLEDB Settings
        /// </summary>
        public static string MicrosoftOffice => ConfigurationManager.AppSettings[nameof(MicrosoftOffice)];

        /// <summary>
        /// anything but "production" limit emails to @dominionsystems.com
        /// </summary>
        public static string EmailRestriction => ConfigurationManager.AppSettings[nameof(EmailRestriction)];

        public static string SeqUrl => ConfigurationManager.AppSettings[nameof(SeqUrl)];

        public static string SeqKey => ConfigurationManager.AppSettings[nameof(SeqKey)];

        #endregion

        #region Reporting

        public static bool Show1095PressureSealBackground => (ConfigurationManager.AppSettings[nameof(Show1095PressureSealBackground)] ?? "False").ConvertToBool();
        #endregion

        #region CLAIMS

        public static string DominionClaimType_UserTypeId
            => ConfigurationManager.AppSettings[nameof(DominionClaimType_UserTypeId)];

        public static string DominionClaimType_HomeSite
            => ConfigurationManager.AppSettings[nameof(DominionClaimType_HomeSite)];

        public static string DominionClaimType_AuthUserId
            => ConfigurationManager.AppSettings[nameof(DominionClaimType_AuthUserId)];

        #endregion

        #region FROM AUTH

        /// TwilioNoMsgMode: All dev and qa sites should use the test data; unless specifically testing the twilio service. 
        /// TwilioNoMsgMode: If this is set to TRUE there the other settings will no be read 
        /// TwilioNoMsgMode: The code will be datetime formatted as: 'HHyyyy' 
        /// TwilioNoMsgMode: The system will not try to send any message, you'll know what the code is if you know the hour (military) and year 
        /// TwilioNoMsgMode: This tells the system to use hour/date code instead of sending sms/voice 
        /// TwilioNoMsgMode: Every message costs us money, therefore unless we neeeeed to test it, we should not. 
        /// If the app setting isn't in the config then the default is TRUE.
        /// </summary>
        public static bool TwilioNoMsgMode
            => ConfigurationManager
                .AppSettings[nameof(TwilioNoMsgMode)]
                .ConvertToBool();

        /// <summary>
        /// 
        /// </summary>
        public static string TwilioAccountSid
            => ConfigurationManager.AppSettings[nameof(TwilioAccountSid)];

        /// <summary>
        /// 
        /// </summary>
        public static string TwilioAuthToken
            => ConfigurationManager.AppSettings[nameof(TwilioAuthToken)];

        /// <summary>
        /// 
        /// </summary>
        public static string TwilioPhoneNumber
            => ConfigurationManager.AppSettings[nameof(TwilioPhoneNumber)];

        /// <summary>
        /// 
        /// </summary>
        public static string TwilioVoiceUrl
            => ConfigurationManager.AppSettings[nameof(TwilioVoiceUrl)];

        public static string HostPath
            => ConfigurationManager.AppSettings[nameof(HostPath)];

        #endregion

        #endregion

        #region Auth Settings

        public static int AuthSignInTokenExpiration => ConfigurationManager
            .AppSettings[nameof(AuthSignInTokenExpiration)].ConvertToOrDefault<int>();
        #endregion

        #region CONNECTION STRINGS

        #region ORIGINAL

        /// <summary>
        /// Connection string for the AH DB (DOMSQL)
        /// </summary>
        public static string ConnectionStringAH
            => ConfigurationManager.ConnectionStrings[nameof(ConnectionStringAH)]?.ConnectionString;

        /// <summary>
        /// This is the connection string of the current app environment.
        /// This is the same value as the 'ConnectionString' app setting.
        /// I'd like to see that 'ConnectionString' app setting disappear.
        /// </summary>
        public static string DominionContext
            => ConfigurationManager.ConnectionStrings[nameof(DominionContext)]?.ConnectionString;

        /// <summary>
        /// Better named alias for code references.
        /// This is the connection string of the current app environment.
        /// This is the same value as the 'ConnectionString' app setting.
        /// I'd like to see that 'ConnectionString' app setting disappear./// 
        /// </summary>
        public static string ConnectionString => DominionContext;

		
        /// <summary>
        /// USED IN THE AUTHENTICATION (STS) APPLICATION
        /// </summary>
        public static string DominionPayrollURL
            => ConfigurationManager.AppSettings[nameof(DominionPayrollURL)];

        #endregion

        #region FROM AUTH

        public static string AuthCtx => ConfigurationManager.ConnectionStrings[nameof(AuthCtx)]?.ConnectionString ?? AuthCtxOverride;

        /// <summary>
        /// THIS IS USED FOR GATHERING DATA FROM THE APP FOR AUTH.DB CONVERSION PURPOSES.
        /// ie. Copy users to the auth.AuthUser table.
        /// </summary>
        public static string AppData => ConfigurationManager.ConnectionStrings[nameof(AppData)]?.ConnectionString;

        /// <summary>
        /// This is a flag that skips checking the pass but says that it matched.
        /// This is for known DEV, QA, and Staging environments only.
        /// </summary>
        public static bool PassAlwaysMatches
            => ConfigurationManager
                .AppSettings[nameof(PassAlwaysMatches)]
                .ConvertToBool();

        #endregion

        #endregion

        #region AzureStorageAccounts

        public static string AzureFile => ConfigurationManager.AppSettings[nameof(AzureFile)];
        public static string AzureProfileImage = ConfigurationManager.AppSettings[nameof(AzureProfileImage)];

        #endregion
		
		#region Elmah

        /// <summary>
        /// This is used for linking the url of Elmah to UI
        /// This is for known DEV, QA, and Staging environments only.
        /// </summary>
        public static string ElmahAdminUrl => ConfigurationManager.AppSettings[nameof(ElmahAdminUrl)];

        /// <summary>
        /// This is used for writing errors to Elmah database
        /// </summary>
        public static string ElmahConnectionString => ConfigurationManager.AppSettings[nameof(ElmahConnectionString)];

        public static string ElmahApplicationName => ConfigurationManager.AppSettings[nameof(ElmahApplicationName)];

        #endregion

        #region Notifications

        /// <summary>
        /// Root URL to the legacy application. Used to build links for notifications that redirect to the legacy app.
        /// </summary>
        public static string LegacyNotificationsUrl => ConfigurationManager.AppSettings["LegacyNotificationsUrl"];

        /// <summary>
        /// Root URL to the Ess application. Used to build links for notifications that redirect to the Ess app.
        /// </summary>
        public static string EssNotificationsUrl => ConfigurationManager.AppSettings["EssNotificationsUrl"];

        /// <summary>
        /// Root URL to the Company application. Used to build links for notifications that redirect to the Company app.
        /// </summary>
        public static string CompanyNotificationsUrl => ConfigurationManager.AppSettings["CompanyNotificationsUrl"];

        #endregion

        #region Mobile Auth

        public static bool PassAlwaysMatchesOverride = false;

        public static string HostPathOverride = "";

        public static string AuthCtxOverride = "";

        public static string SiteConfigHeader = "scid";

        public static readonly string DominionClaimType_SiteConfiguration = "https://live.dominionsystems.com/identity/claims/siteConfigId";

        #endregion

        #region INTERNAL API

        public static string InternalApiUri => ConfigurationManager.AppSettings[nameof(InternalApiUri)];

        public static string InternalApiClientSecret => ConfigurationManager.AppSettings[nameof(InternalApiClientSecret)];

        public static string InternalApiClientId => ConfigurationManager.AppSettings[nameof(InternalApiClientId)];

        public static string InternalApiScopes => ConfigurationManager.AppSettings[nameof(InternalApiScopes)];

        public static string InternalApiGrantType = "client_credentials";

        public static string InternalApiResponseType = "token";

        public static string InternalApiUsername => ConfigurationManager.AppSettings[nameof(InternalApiUsername)];

        public static string InternalApiAuthUri => ConfigurationManager.AppSettings[nameof(InternalApiAuthUri)];

        public static string EmployeeNavigatorApi => ConfigurationManager.AppSettings[nameof(EmployeeNavigatorApi)];

        public static string EmployeeNavigatorApiKey => ConfigurationManager.AppSettings[nameof(EmployeeNavigatorApiKey)];
        
        public static bool IsEnInDevelopment => ConfigurationManager.AppSettings[nameof(IsEnInDevelopment)].ConvertToOrDefault<bool>(true);

        #endregion
    }
}
