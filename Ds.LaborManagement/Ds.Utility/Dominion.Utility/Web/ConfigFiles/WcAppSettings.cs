using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Web.ConfigFiles
{
    public class WcAsRules 
    {
        public class ConfigReport1 : IWcSectionRule<AsReportDto>
        {
            public static ConfigReport1 C()
            {
                return new ConfigReport1();
            }

            public IOpResult<AsReportDto> Execute(IWcSectionEleInfo sectionInfo)
            {
                var r = new OpResult<AsReportDto>();

                r.TryCatch(() =>
                {
                    var o                 = new AsReportDto();
                    o.IsExternal          = sectionInfo.IsExternal;
                    o.UseEmbeddedSts      = sectionInfo.Elements.FirstOrDefault(x => x.Attribute("key").Value == "ests:UseEmbedded")?.Attribute("value").Value;
                    o.StsMetadataUrl      = sectionInfo.Elements.FirstOrDefault(x => x.Attribute("key").Value == "sts:MetadataAddress")?.Attribute("value").Value;
                    o.StsWTRealm          = sectionInfo.Elements.FirstOrDefault(x => x.Attribute("key").Value == "sts:Wtrealm")?.Attribute("value").Value;
                    o.StsLogout           = sectionInfo.Elements.FirstOrDefault(x => x.Attribute("key").Value == "sts:logoutPath")?.Attribute("value").Value;
                    o.LegacyRootUrl       = sectionInfo.Elements.FirstOrDefault(x => x.Attribute("key").Value == "legacyRootUrl")?.Attribute("value").Value;
                    o.MainRedirectRootUrl = sectionInfo.Elements.FirstOrDefault(x => x.Attribute("key").Value == "MainRedirectRootUrl")?.Attribute("value").Value;

                    r.Data = o;
                });

                return r;
            }
        }
    }

    public class AsReportDto
    {
        public bool IsExternal { get; set; }

        public string UseEmbeddedSts { get; set; }
        public string StsMetadataUrl { get; set; }
        public string StsWTRealm { get; set; }
        public string StsLogout { get; set; }
        public string LegacyRootUrl { get; set; }
        public string MainRedirectRootUrl { get; set; }
    }

}

//public class AsDto
//{
//    public string Key { get; set; }
//    public string Value { get; set; }
//}

//private readonly string[] _keys = new[]
//{
//    "ests:UseEmbedded",
//    "sts:MetadataAddress",
//    "sts:Wtrealm",
//    "sts:logoutPath",
//    "legacyRootUrl",
//    "MainRedirectRootUrl",
//};

//IOpResult<ConfigReportDto> IWcSectionRule.Execute(IEnumerable<XElement> elems)
//{
//    //var items = elems.Where(x => _keys.Contains(x.Name.LocalName));
//    var o = new ConfigReportDto();
//    o.UseEmbeddedSts = elems.FirstOrDefault(x => x.Name.LocalName == "sts:UseEmbedded")?.Value;
//    o.StsMetadataUrl = elems.FirstOrDefault(x => x.Name.LocalName == "sts:MetadataAddress")?.Value;
//    o.StsWTRealm = elems.FirstOrDefault(x => x.Name.LocalName == "sts:Wtrealm")?.Value;
//    o.StsLogout = elems.FirstOrDefault(x => x.Name.LocalName == "sts:logoutPath")?.Value;
//    o.LegacyRootUrl = elems.FirstOrDefault(x => x.Name.LocalName == "legacyRootUrl")?.Value;
//    o.MainRedirectRootUrl = elems.FirstOrDefault(x => x.Name.LocalName == "MainRedirectRootUrl")?.Value;

//    return null;
//}