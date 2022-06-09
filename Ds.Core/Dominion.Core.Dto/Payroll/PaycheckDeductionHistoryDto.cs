using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class PaycheckDeductionHistoryDto
    {
        public virtual decimal      Amount                { get; set; }
        public virtual bool         IsMemoDeduction       { get; set; }
        public virtual int          ClientId              { get; set; }
        public virtual int          EmployeeId            { get; set; }
        public virtual int          PayrollId             { get; set; }
        public virtual int?         PaycheckId            { get; set; }
        public virtual int?         GenPaycheckPayDataHistoryId { get; set; }
        public virtual int?         ClientDeductionId           { get; set; }
        public virtual int?         BondId                      { get; set; }
        public virtual string       RoutingNumber               { get; set; }
        public virtual string       AccountNumber               { get; set; }
        public virtual byte?        AccountType                 { get; set; }

        public virtual PaycheckHistoryDto PaycheckHistory { get; set; }
    }
}
