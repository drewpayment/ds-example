using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class PayrollEmailLog : Entity<PayrollEmailLog>
    {
        public virtual int       PayrollEmailLogId { get; set; }
        public virtual int       ClientId          { get; set; }
        public virtual int       PayrollId         { get; set; }
        public virtual int       LogType           { get; set; }
        public virtual DateTime  StartDate         { get; set; }
        public virtual DateTime? EndDate           { get; set; }
        public virtual int       ModifiedBy        { get; set; }
        public virtual bool      HasError          { get; set; }
    }
}
