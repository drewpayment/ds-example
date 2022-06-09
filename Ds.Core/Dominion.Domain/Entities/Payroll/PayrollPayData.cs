using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Payroll
{
    public class PayrollPayData : Entity<PayrollPayData>
    {
        public virtual int PayrollPayDataId { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual int PayrollId { get; set; }
        public virtual bool IsPay { get; set; }
        public virtual string SubCheck { get; set; }
        public virtual int? ClientRateId { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual int? ClientPayDataInterfaceId { get; set; }
        public virtual PayrollPayDataInterfaceType? PayrollPayDataInterfaceId { get; set; }
        public virtual string SourceId { get; set; }
        public virtual int? ClientId { get; set; }
        public virtual int Week { get; set; }

        // REVERSE NAVIGATION
        public virtual ICollection<PayrollEmployeeOverride> EmployeeOverrides { get; set; } // many-to-one;
        public virtual ICollection<PayrollPayDataDetail> PayDataDetails { get; set; } // many-to-one;

        // FOREIGN KEYS
        public virtual Employee.Employee Employee { get; set; }
        public virtual Payroll Payroll { get; set; }
        public virtual ClientPayDataInterface ClientPayDataInterface { get; set; }
        public virtual PayrollPayDataInterface PayDataInterface { get; set; }
    }
}