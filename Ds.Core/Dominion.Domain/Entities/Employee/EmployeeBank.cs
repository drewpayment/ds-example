using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.InstantPay;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Employee
{
    public partial class EmployeeBank : Entity<EmployeeBank> , IHasModifiedData
    {
        public virtual int                          EmployeeBankId      { get; set; }
        public virtual string                       AccountName         { get; set; }
        public virtual string                       AccountNumber       { get; set; } 
        public virtual string                       RoutingNumber       { get; set; } 
        public virtual bool                         IsPreNote           { get; set; } 
        public virtual EmployeeBankAccountType?     AccountType         { get; set; } 
        public virtual DateTime                    Modified            { get; set; } 
        public virtual int                       ModifiedBy          { get; set; } 
        public virtual int                          ClientId            { get; set; } 
        public virtual int                          EmployeeId          { get; set; } 
        public virtual int?                         EmployeeDeductionId { get; set; }
        public virtual InstantPayProvider? InstantPayProvider { get; set; }

        public virtual ICollection<EmployeeDeduction> EmployeeDeductions { get; set; } 
    }
}
