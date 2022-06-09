using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Tax
{
    public class ClientTaxDeferralDto
    {
        public int ClientTaxDeferralId { get; set; }
        public int ClientId { get; set; }
        public TaxDeferralType TaxType { get; set; }
        public bool IsDeferred { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
    }

    public enum TaxDeferralType
    {
        EmployerSocialSecurity
    }
}
