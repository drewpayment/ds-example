using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Web.ConfigFiles
{
    public class WcSimRules
    {
        public class ConfigReport1 : IWcSectionRule<SimReportDto>
        {
            public static ConfigReport1 C()
            {
                return new ConfigReport1();
            }

            public IOpResult<SimReportDto> Execute(IWcSectionEleInfo sectionInfo)
            {
                var r = new OpResult<SimReportDto>();

                r.TryCatch(() =>
                {
                    var o = new SimReportDto();
                     
                    o.IsExternal = sectionInfo.IsExternal;
                    o.AuthorityUrl = sectionInfo.Elements?
                        .FirstOrDefault()?
                        //the double backward slashes consider the root when evaluating the xpath
                        .XPathSelectElement(@"//identityConfiguration/issuerNameRegistry/authority")?
                        .Attribute("name")?
                        .Value;

                    r.Data = o;
                });

                return r;
            }
        }

    }

    public class SimReportDto
    {
        public bool IsExternal { get; set; }

        public string AuthorityUrl { get; set; }
    }
}
