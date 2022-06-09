using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Employee
{
    /// <summary>
    /// Entity containing detail info about a <see cref="EmployeeDeductionAmountType"/>.
    /// </summary>
    public partial class EmployeeDeductionAmountTypeInfo : Entity<EmployeeDeductionAmountTypeInfo>
    {
        public virtual EmployeeDeductionAmountType EmployeeDeductionAmountTypeId { get; set; } 
        public virtual string                      Description                   { get; set; } 
        public virtual string                      Code                          { get; set; } 
        public virtual byte?                       DisplayOrder                  { get; set; } 
        public virtual string                      NumberPrefix                  { get; set; }
    }
}
