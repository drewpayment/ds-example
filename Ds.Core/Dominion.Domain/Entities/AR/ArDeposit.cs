using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;


namespace Dominion.Domain.Entities.AR
{
    public class ArDeposit : Entity<ArDeposit>
    {
        public virtual int       ArDepositId { get; set; }
        public virtual decimal   Total       { get; set; }
        public virtual DateTime? PostedDate  { get; set; }
        public virtual int?      PostedBy    { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string    Type        { get; set; }
        public virtual int       CreatedBy   { get; set; }

        public virtual ICollection<ArPayment> ArPayments { get; set; }
        public virtual User.User CreatedByUser { get; set; }
        public virtual User.User PostedByUser { get; set; }
    }
}
