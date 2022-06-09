using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    public class PayrollAdjustment : Entity<PayrollAdjustment>, IHasModifiedUserNameData //IHasModifiedData
    {
        public virtual int         PayrollAdjustmentId         { get; set; }
        public virtual int         PayrollId                   { get; set; }
        public virtual int?        AppliesToPayrollId          { get; set; }
        public virtual int         EmployeeId                  { get; set; }
        public virtual string      SubCheck                    { get; set; }
        public virtual string      CheckNumber                 { get; set; } //nullable
        public virtual DateTime?   DateVoid                    { get; set; }
        public virtual decimal?    CheckAmount                 { get; set; }
        public virtual bool        IsLT3PSP                    { get; set; }
        public virtual bool        IsST3PSP                    { get; set; }
        public virtual DateTime    Modified                    { get; set; }
        public virtual string      ModifiedBy                  { get; set; }
        public virtual int?        ClientId                    { get; set; }
        public virtual int?        genPaycheckHistoryId        { get; set; }
        public virtual int?        genPaycheckPayDataHistoryId { get; set; }
        public virtual bool        IsSuccessorWageAdjustment   { get; set; }


        // Foreign keys
        public virtual Payroll                Payroll          { get; set; }
        public virtual Payroll                AppliesToPayroll { get; set; }
        public virtual Employee.Employee      Employee         { get; set; }
        public virtual PaycheckHistory        PaycheckHistory  { get; set; }
        public virtual PaycheckPayDataHistory PaycheckPayDataHistory { get; set; }
        public virtual ICollection<PayrollAdjustmentDetail> AdjustmentDetails { get; set; }
        public virtual ICollection<PayrollEmployeeOverride> PayrollEmployeeOverrides { get; set; }
    }
}
