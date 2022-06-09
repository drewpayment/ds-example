using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Web.ConfigFiles
{


    public class XpathExp
    {
        /// <summary>
        /// If this returns a value, then the site is setup for authenticating with an STS server.
        /// Otherwise it's using embedded.
        /// </summary>
        public const string SimAuthorityUrl =
            @"/system.identityModel/identityConfiguration/issuerNameRegistry/authority/@name";
    }

    public class WcEleNames
    {
        public const string Root = "configuration";
        public const string As = "appSettings";
        public const string Cs = "connectionStrings";
        public const string Sim = "system.identityModel";
        public const string Sims = "system.identityModel.services";
    }

    public class WcAttrNames
    {
        public const string CS = "configSource";
    }

    public class CsFileNames
    {
        public const string DcDir = "DevConfigs";
        public const string WcAS = "AppSettings.config";
        public const string WcCS = "ConnectionStrings.config";
        public const string WcSim = "System.IdentityModel.config";
        public const string WcSims = "System.IdentityModel.Services.config";
    }

    public class CsAttributes
    {
        public const string Name = "name";
        public const string ConnStr = "connectionString";
        public const string ProvName = "providerName";

    }
}

//public class WcSectionBase : IWcSection
//{
//    public IWcSection Self => this as IWcSection;

//    bool IWcSection.IsExternal { get; set; }

//    IEnumerable<XElement> IWcSection.Elements { get; set; }

//    IOpResult<T> IWcSection.ExecuteRule<T>(IWcSectionRule<T> rule)
//    {
//        return rule.Execute(Self.Elements);
//    }
//}