using System.Configuration;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.Constants
{
    public class ConfigurationFileConstants
    {
        public const string SECTION_APP_SETTINGS = "appSettings";
        public const string APP_SETTINGS_FEEDBACK_EMAIL = "FeedbackEmail";
        public const string APP_SETTINGS_DEMONSTRATION_MODE = "DemonstrationMode";
        public const string APP_SETTINGS_DEMONSTRATION_CLIENT_RELATION_ID = "DemonstrationClientRelationId";
        public const string APP_SETTINGS_SERVER_NAME = "ServerName";

        ///// <summary>
        ///// AppSettings constants related to EmailRestriction setting.
        ///// </summary>
        public const string APP_SETTINGS_EMAIL_RESTRICTION = "EmailRestriction";
        public const string APP_SETTINGS_EMAIL_RESTRICTION_PRODUCTION = "PRODUCTION";
        public const string APP_SETTINGS_EMAIL_RESTRICTION_DOMINION_ONLY = "DOMINIONONLY";
        public const string APP_SETTINGS_EMAIL_RESTRICTION_DOMINION_EMAIL = "DOMINIONSYSTEMS.COM";
        public const string APP_SETTINGS_EMAIL_RESTRICTION_ECGROUP_EMAIL = "ECGROUP-INTL.COM";

        ///// <summary>
        ///// Default dbo.ClientRelation ClientRelationID of the relation containing all demo/test clients in the system.
        ///// </summary>
        public const int DEFAULT_DEMONSTRATION_CLIENT_RELATION_ID = 23; 

        ///// <summary>
        ///// Default 'DemonstrationMode' app-setting value. Default is 'Production' indicating test/demo clients should 
        ///// be excluded from certain queries (see IClientQuery.ExcludeDemonstrationClients).
        ///// </summary>
        public const string DEFAULT_DEMONSTRATION_MODE = "Production";

        public const string APP_SETTINGS_SSH_PRIVATE_KEY = "SSHPrivateKey";

        /// <summary>
        /// AppSettings constants related to the TurboTax W-2 Upload API
        /// </summary>
        public const string APP_SETTINGS_TURBOTAX_W2_AUTH_URL = "TurboTaxW2AuthUrl";
        public const string APP_SETTINGS_TURBOTAX_W2_EXPORT_URL = "TurboTaxW2ExportUrl";

        ///// <summary>
        ///// Allowed 'DemonstrationMode' app-setting values that will put the site into Test-mode allowing clients in the
        ///// 'Demonstration Payroll' relationship to be included in relevant queries (see IClientQuery.ExcludeDemonstrationClients). 
        ///// Current allowed values are 'Test' or 'Demo'.
        ///// </summary>
        public static readonly string[] DemonstrationTestModeValues = { "Test", "Demo" };

        

        //public static bool ActivateRedirection 
        //    => ConfigurationManager.AppSettings[nameof(ActivateRedirection)].ConvertToBool();

        //public static string DominionClaimType_UserTypeId 
        //    => ConfigurationManager.AppSettings[nameof(DominionClaimType_UserTypeId)];

        //public static string DominionClaimType_HomeSite 
        //    => ConfigurationManager.AppSettings[nameof(DominionClaimType_HomeSite)];

        //public static int NumberOfUriSegmentsToCheckForRedirection 
        //    => ConfigurationManager.AppSettings[nameof(NumberOfUriSegmentsToCheckForRedirection)].ConvertToOrDefault<int>();

        //public static string MainRedirectRootUrl 
        //    => ConfigurationManager.AppSettings[nameof(MainRedirectRootUrl)];

        //public static int SiteConfigurationID 
        //    => ConfigurationManager.AppSettings[nameof(SiteConfigurationID)].ConvertToOrDefault<int>(1);


    }
}