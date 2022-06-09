using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Web.ConfigFiles
{
    public class CsRules
    {
        public class ConfigReport1 : IWcSectionRule<IEnumerable<ConnStrDto>>
        {
            public static ConfigReport1 C()
            {
                return new ConfigReport1();
            }

            public IOpResult<IEnumerable<ConnStrDto>> Execute(IWcSectionEleInfo sectionInfo)
            {
                var r = new OpResult<IEnumerable<ConnStrDto>>();
                var list = new List<ConnStrDto>();

                r.TryCatch(() =>
                {
                    foreach (XElement ele in sectionInfo.Elements)
                    {
                        var dto = new ConnStrDto();
                        var csb = new SqlConnectionStringBuilder();
                        dto.Name = ele.Attribute(CsAttributes.Name)?.Value;
                        var cs = ele.Attribute(CsAttributes.ConnStr)?.Value;

                        if (cs != null)
                        { 
                            csb.ConnectionString = cs;
                            dto.IsExternal = sectionInfo.IsExternal;
                            dto.DbServer = csb.DataSource;
                            dto.Db = csb.InitialCatalog;
                            dto.UserName = csb.UserID;
                            dto.Password = csb.Password;
                            dto.CsBuilder = csb;
                        }

                        list.Add(dto);
                    }
                });

                r.Data = list;
                return r;
            }
        }
    }

    public class ConnStrDto
    {
        public bool IsExternal { get; set; }

        public string Name { get; set; }

        public string DbServer { get; set; }
        public string Db { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        //public string Provider { get; set; }
        public SqlConnectionStringBuilder CsBuilder { get; set; }

    }


}
