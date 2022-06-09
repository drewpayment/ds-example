using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Definition for PayGrade Entities representing entries in the PayGrade table.
    /// </summary>
    public class PayGrade : Entity<PayGrade>, IHasModifiedData
    {
        public virtual int PayGradeID { get; set; } //pk
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual decimal Minimum { get; set; }
        public virtual decimal Middle { get; set; }
        public virtual decimal Maximum { get; set; }
        public virtual PayGradeType Type { get; set; }
        public virtual int ClientID { get; set; } //fk
        public virtual Client Client { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}
