using System;

namespace Dominion.Core.Dto.Client
{
    public class ClientTurboTaxDto
    {
        public virtual int ClientTurboTaxId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int TaxYear { get; set; }
        public virtual ClientTurboTaxFileStatus TurboTaxFileStatus { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
    }
}
