using Dominion.Domain.Entities.Base;
using System;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Core.Dto.Tax;

namespace Dominion.Domain.Entities.Tax
{
    public class ClientTaxDeferral : Entity<ClientTaxDeferral>, IHasModifiedData
    {
        public int ClientTaxDeferralID { get; set; }
        public TaxDeferralType TaxType { get; set; }
        public bool IsDeferred { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public int ClientID { get; set; }
    }
}
