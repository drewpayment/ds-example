using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates.Branch
{
    public class BranchLocationsLayout
    {
        public string   NAME          { get; set; }
        public string   LATITUDE { get; set; }
        public string   LONGITUDE { get; set; }
        public string   STORENUM { get; set; }
        public string   ADDRESS       { get; set; }
        public string   CITY          { get; set; }
        public string   STATE         { get; set; }
        public string   POSTAL        { get; set; }
        public string   CLIENTCODE    { get; set; }
        public string   ACTIVE        { get; set; }
        public DateTime EFFECTIVEDATE { get; set; }
    }
}
