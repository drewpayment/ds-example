using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates.Branch
{
    public class BranchTimeAndAttendanceLayout
    {
        public string EmployeeId { get; set; }
        public DateTime PunchIn { get; set; }
        public DateTime PunchOut { get; set; }
        public double? HourlyRate { get; set; }
        public string ClientCode { get; set; }
    }
}
