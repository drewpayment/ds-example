using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Tax;


namespace Dominion.Core.Dto.Client
{
    public class ClientTaxDto
    {
        public int ClientTaxId { get; set; }
        public int ClientId { get; set; }
        public string TaxIdNumber { get; set; }
        public int TaxFrequencyId { get; set; }
        public int? StateTaxId { get; set; }
        public int? LocalTaxId { get; set; }
        public string Description { get; set; }
        public string UnemploymentId { get; set; }
        public byte? CalendarDebitId { get; set; }
        public bool IsIncludeInElectronicInterface { get; set; }
        public byte? SutaCalendarDebitId { get; set; }
        public bool IsIncludeInW2ElectronicFile { get; set; }
        public int? DisabilityTaxId { get; set; }
        public string AlternateVendorName { get; set; }
        public DateTime LastSutaCatchupDate { get; set; }
        public int? OtherTaxId { get; set; }
        public int? ResidentId { get; set; }
        public LegacyTaxType LegacyTaxType { get; set; }
        public bool? TaxIsActive { get; set; }
        public string EftCreditId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedByDescription { get; set; }
        public int LastModifiedByUserId { get; private set; }
        public virtual ICollection<ClientTaxInfoDto> TaxInfos { get; set; }
        public ICollection<ClientTaxPaymentInfoDto> TaxPaymentInfos { get; set; }
    }
}
