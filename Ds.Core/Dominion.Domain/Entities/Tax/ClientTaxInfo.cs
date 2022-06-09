using System;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Tax
{
    public partial class ClientTaxInfo : Entity<ClientTaxInfo>
    {
        public virtual int ClientTaxInfoId { get; set; }
        public virtual int ClientTaxId { get; set; }
        public virtual DateTime EffectiveDate { get; set; }
        public virtual double Rate { get; set; }
        public virtual double? Limit { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string Modifiedby { get; set; }

        public ClientTax Tax { get; set; }
    }
}
