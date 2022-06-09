using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class PayrollEmailLogDto
    {
        public int       PayrollEmailLogId { get; set; }
        public int       ClientId          { get; set; }
        public int       PayrollId         { get; set; }
        public int       LogType           { get; set; }
        public DateTime  StartDate         { get; set; }
        public DateTime? EndDate           { get; set; }
        public int       ModifiedBy        { get; set; }
        public bool      HasError          { get; set; }
    }
}
