using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Payroll;

namespace Dominion.Domain.Entities.Labor
{
    public class JobProfileCompensation : Entity<JobProfileCompensation>
    {
        public virtual int JobProfileID { get; set; }
        public virtual PayType? EmployeeTypeID { get; set; }
        public virtual bool? IsExempt { get; set; }
        public virtual bool? IsTipped { get; set; }
        public virtual PayFrequencyType? PayFrequencyID { get; set; }
        public virtual bool? BenefitsEligibility { get; set; }
        public virtual int? BenefitPackageId { get; set; }
        public virtual int? SalaryMethodTypeId { get; set; }
        public virtual int? Hours { get; set; }

        public virtual JobProfile JobProfile { get; set; }
        public virtual PayFrequency PayFrequency { get; set; }
        
    }
}
