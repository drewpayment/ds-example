using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class JobProfileCompensationDto
    {
        public int JobProfileID { get; set; }
        public PayType? EmployeeTypeID { get; set; }
        public bool? IsExempt { get; set; }
        public bool? IsTipped { get; set; }
        public PayFrequencyType? PayFrequencyID { get; set; }
        public bool? IsBenefitsEligibility { get; set; }
        public int? BenefitPackageId { get; set; }
        public int? SalaryMethodTypeId { get; set; }
        public int? Hours { get; set; }

    }
}
