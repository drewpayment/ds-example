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
    /// Definition for PayGradeChangeHistory Entities representing entries in the PayGradeChangeHistory table.
    /// </summary>
    public class PayGradeChangeHistory : Entity<PayGradeChangeHistory>, IModifiableEntity<PayGradeChangeHistory>
    {
        public virtual int ChangeID { get; set; } //pk
        public virtual int PayGradeID { get; set; } //fk
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual decimal Minimum { get; set; }
        public virtual decimal Middle { get; set; }
        public virtual decimal Maximum { get; set; }
        public virtual PayGradeType Type { get; set; }
        public virtual int ClientID { get; set; } //fk
        public virtual string ChangeMode { get; set; }
        public virtual Client Client { get; set; }
        public virtual PayGrade PayGrade { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedByDescription { get; set; }
        public int LastModifiedByUserId { get; private set; }
        public User.User LastModifiedByUser { get; private set; }
        public void SetLastModifiedValues(int lastModifiedByUserId, string lastModifiedByUserName, DateTime lastModifiedDate)
        {
            LastModifiedByUserId = lastModifiedByUserId;
            LastModifiedByDescription = lastModifiedByUserName;
            LastModifiedDate = lastModifiedDate;
        }
    }
}
