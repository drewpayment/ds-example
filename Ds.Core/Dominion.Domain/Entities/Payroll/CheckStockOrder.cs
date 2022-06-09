using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class CheckStockOrder : Entity<CheckStockOrder>

    {
        public virtual int CheckStockOrderId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual DateTime RequestedDate { get; set; }
        public virtual int NextCheckNumber { get; set; }
        public virtual bool IsDelivery { get; set; }
        public virtual int TotalChecks { get; set; }
        public virtual int CheckEnvelopes { get; set; }
        public virtual int W2Envelopes { get; set; }
        public virtual int ACAEnvelopes { get; set; }
        public virtual decimal OrderPrice { get; set; }
        public virtual DateTime? DatePrinted { get; set; }
        public virtual int RequestedByUserId { get; set; }
        public virtual int? PrintedByUserId { get; set; }

        public CheckStockOrder()
        {
        }
        public User.User RequestedByUser { get; set; }
        public User.User PrintedByUser { get; set; }
        public Client Client { get; set; }
    }
}
