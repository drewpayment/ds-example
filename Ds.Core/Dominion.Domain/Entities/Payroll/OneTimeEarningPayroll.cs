using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Indicates that a <see cref="OneTimeEarning"/> has been imported into a Payroll
    /// </summary>
    public class OneTimeEarningPayroll : Entity<OneTimeEarningPayroll>, IHasModifiedData
    {
        public int OneTimeEarningId { get; set; }
        public int PayrollPayDataId { get; set; }
        public int PayrollId { get; set; }
        public virtual Payroll Payroll { get; set; }
        public virtual OneTimeEarning OneTimeEarning { get; set; }
        public virtual PayrollPayData PayrollPayData { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}
