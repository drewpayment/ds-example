using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Employee
{
    public class EmployeeHRInfo : Entity<EmployeeHRInfo>
    {
        public virtual int EmployeeHRInfoId { get; set; }
        public virtual int ClientHRInfoId { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual string Value { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual int ClientId { get; set; }
        public virtual bool? InsertApproved { get; set; }

        public virtual ClientHRInfo ClientHRInfo { get; set; }

        public virtual Employee Employee { get; set; }
    }
}