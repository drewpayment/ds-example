using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Employee
{
    public partial class EmployeeClientRate : Entity<EmployeeClientRate>, IHasModifiedOptionalData
    {
        public virtual int       EmployeeClientRateId { get; set; }
        public virtual int       EmployeeId           { get; set; }
        public virtual int       ClientRateId         { get; set; }
        public virtual double    Rate                 { get; set; }
        public virtual bool      IsDefaultRate        { get; set; }
        public virtual DateTime? RateEffectiveDate    { get; set; }
        public virtual DateTime? Modified             { get; set; }
        public virtual int?      ModifiedBy           { get; set; }
        public virtual int       ClientId             { get; set; }
        public virtual string    Notes                { get; set; }


        public virtual Employee  Employee    { get; set; }
        public virtual Client    Client      { get; set; }
        public virtual ClientRate ClientRate { get; set; }


    }
}
