using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeChangesParametersDto
    {
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SequenceId { get; set; }
        public int PayType { get; set; }
        public int EmployeeStatusId { get; set; }
        public bool ReturnFilterOnly { get; set; }
        public string CsvChangeLogIds { get; set; }
    }
}
