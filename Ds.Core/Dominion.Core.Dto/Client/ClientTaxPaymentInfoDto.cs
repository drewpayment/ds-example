using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientTaxPaymentInfoDto
    {
        public virtual int ClientTaxPaymentInfoId { get; set; }
        public virtual int ClientTaxId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual DateTime EffectiveDate { get; set; }
        public virtual int TaxFrequencyId { get; set; }
        public virtual byte? CalendarDebitId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }

        public virtual ClientTaxDto Tax { get; set; }
    }
}
