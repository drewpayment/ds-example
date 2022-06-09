using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Benefit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominion.Domain.Entities.Employee
{
    /// <summary>
    /// dbo.EmployeeDeductionMaxType
    /// Workaround to accomodate existing tables that were implemented to reference IDs from this table as byte/tinyint.
    /// This was only possible at the time, because they didn't setup the FK constraint... Ugh.
    /// </summary>
    public class EmployeeDeductionMaxTypeEntity : Entity<EmployeeDeductionMaxTypeEntity>, IEmployeeDeductionMaxTypeBasicDto
    {
        public int EmployeeDeductionMaxTypeId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public EmployeeDeductionMaxType? EmployeeDeductionMaxType => this.GetEmployeeDeductionMaxType();

        public virtual ICollection<PlanType> PlanTypes { get; set; }
    }
}
