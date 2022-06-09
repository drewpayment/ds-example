using Dominion.Core.Dto.Common;

namespace Dominion.Core.Dto.Tax.EmployeeTaxAdmin
{
    public class EmployeeNonFederalTaxDto : EmployeeTaxSetupDto, ILegacyTaxIdAndType
    {
        public LegacyTaxType TaxTypeId { get; set; }
        public int? StateTaxId { get; set; }
        public int? LocalTaxId { get; set; }
        public int? DisabilityTaxId { get; set; }
        public int? OtherTaxId { get; set; }
        public int? SutaTaxId { get; set; }
        public int? EmployerPaidTaxId { get; set; }
        public string LocalTaxCode { get; set; }
        //public byte DisplayOrder { get; set; }
        public bool IsReimbursable { get; set; }
        public bool BlockOverrides { get; set; }
        public KeyValueDto StateInfo { get; set; }
        public bool TaxHasHistory { get; set; }

        public int TaxId => StateTaxId ?? LocalTaxId ?? DisabilityTaxId ?? OtherTaxId ?? default;

        public LegacyTaxType? LegacyTaxType => TaxTypeId;

        public byte DisplayOrder
        {
            get
            {
                if (StateTaxId.HasValue)
                    return 1;

                if (LocalTaxId.HasValue)
                    return 2;

                if (DisabilityTaxId.HasValue)
                    return 3;

                if (EmployerPaidTaxId.HasValue)
                    return 4;

                if (OtherTaxId.HasValue)
                    return 10;

                if (SutaTaxId.HasValue)
                    return 99;

                return byte.MaxValue;
            }
        }
    }
}
