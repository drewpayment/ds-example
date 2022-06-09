using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class EmployeeTimeCard
    {
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
        public int EmployeeId { get; set; }
        public string FirstDay { get; set; }
        public int EmployeeActivity { get; set; }
        public int PunchOption { get; set; }
        public int TimePolicyId { get; set; }
        public string TimePolicyName { get; set; }
        public string Schedule { get; set; }
        public ICollection<TimeCardPunchRow> PunchRows { get; set; }
        public ICollection<TimeCardBenefitRow> BenefitRows { get; set; }
        public ICollection<TimeCardEarningRow> TimeCardEarningRows { get; set; }
        public ICollection<TotalHours> TotalRows { get; set; }
    }
}
