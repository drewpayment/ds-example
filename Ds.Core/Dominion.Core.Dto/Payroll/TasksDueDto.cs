using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class TasksDueDto
    {
        public int  ReminderTasksDue        { get; set; }
        public bool ReminderEnabled         { get; set; }
        public int  EmployeeRequestTasksDue { get; set; }
        public bool EmployeeRequestEnabled  { get; set; }
        public int  LeaveMangementTasksDue  { get; set; }
        public bool LeaveMangementEnabled   { get; set; }
        public int  OnboardingTasksDue      { get; set; }
        public bool OnboardingEnabled       { get; set; }
        public bool IsPayrollOpen           { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

    }
}
