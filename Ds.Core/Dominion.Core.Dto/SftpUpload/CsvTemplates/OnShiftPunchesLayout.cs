using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates
{
    public class OnShiftPunchesLayout
    {
        public string division { get; set; }
        public string employeeNumber { get; set; }
        public DateTime punchDate { get; set; }
        public DateTime? inPunch { get; set; }
        public DateTime? outPunch { get; set; }
        public double totalHoursWorked { get; set; }
        public string costCenter { get; set; }
        public string payCode { get; set; }
        public string jobTitle { get; set; }
    }
}
