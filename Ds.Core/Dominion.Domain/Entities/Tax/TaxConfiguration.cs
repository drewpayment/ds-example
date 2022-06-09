using System;

namespace Dominion.Domain.Entities.Tax
{
    public class TaxConfiguration
    {
        public int      TaxConfigurationId    { get; set; }
        public int      TaxId                 { get; set; }
        public DateTime EffectiveDate         { get; set; }
        public decimal? AnnualExemptionAmount { get; set; }

        public Tax      Tax                   { get; set; }
    }
}
