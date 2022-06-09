using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Payroll
{
    public class PayrollPayDataDetail : Entity<PayrollPayDataDetail>
    {
        public virtual int PayrollPayDataDetailId { get; set; }
        public virtual int PayrollPayDataId { get; set; }
        public virtual int ClientEarningId { get; set; }
        public virtual double? Hours { get; set; }
        public virtual double? Amount { get; set; }
        public virtual int? ClientPayDataInterfaceId { get; set; }
        public virtual PayrollPayDataInterfaceType? PayrollPayDataInterfaceId { get; set; }
        public virtual string SourceId { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual int? ClientId { get; set; }

        // FOREIGN KEYS
        public virtual PayrollPayData PayrollPayData { get; set; }
        public virtual ClientEarning ClientEarning { get; set; }

        public virtual ClientPayDataInterface ClientPayDataInterface { get; set; }
        public virtual PayrollPayDataInterface PayrollPayDataInterface { get; set; }
    }
}