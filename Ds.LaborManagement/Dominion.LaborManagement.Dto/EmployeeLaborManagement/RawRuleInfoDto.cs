using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.EmployeeLaborManagement
{
    public class RawRuleInfoDto
    {
        public int RuleId { get; set; }
        public byte? BiWeeklyStartingDayOfWeekId { get; set; }
        public byte? WeeklyStartingDayOfWeekId { get; set; }
        public byte? MonthlyStartingDayOfWeekId { get; set; }
        public byte? SemiMonthlyStartingDayOfWeekId { get; set; }
    }
}
