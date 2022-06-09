using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates.ComPsychExport
{
    public class ComPsychExportPayrollDto
    {
        public int PayrollId { get; set; }
        public int ClientId { get; set; }
        public DateTime CheckDate { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }

        /// <summary>
        /// EmployeeIds of employees paid in this payroll.
        /// </summary>
        public IEnumerable<int> EmployeeIds { get; set; }
    }
}
