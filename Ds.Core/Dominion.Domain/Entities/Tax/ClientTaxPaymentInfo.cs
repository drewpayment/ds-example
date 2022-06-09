using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Tax
{
    public partial class ClientTaxPaymentInfo : Entity<ClientTaxPaymentInfo>
    {
        public virtual int ClientTaxPaymentInfoId { get; set; }
        public virtual int ClientTaxId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual DateTime EffectiveDate { get; set; }
        public virtual int TaxFrequencyId { get; set; }
        public virtual byte? CalendarDebitId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }

        public virtual ClientTax Tax { get; set; }

        public ClientTaxPaymentInfo()
        {
        }
    }
}
