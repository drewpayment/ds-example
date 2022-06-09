using Dominion.Utility.Dto;
using Dominion.Taxes.Dto.TaxOptions;
using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Client;

namespace Dominion.Core.Dto.Tax
{
    /// <summary>
    /// Employee Tax Data Transfer Object used to provide employee tax information out of the API
    /// </summary>
    public class EmployeeTaxDto : DtoObject, IHas2020TaxOptions
    {
        public int EmployeeTaxId { get; set; }
        public int EmployeeId { get; set; }
        public int? ClientTaxId { get; set; }
        public int FilingStatusId { get; set; }
        public string FilingStatusDescription { get; set; }
        public int NumberOfExemptions { get; set; }
        public byte NumberOfDependents { get; set; }
        public int AdditionalAmountTypeId { get; set; }
        public double AdditionalAmount { get; set; }
        public double AdditionalPercent { get; set; }
        public bool IsResident { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }
        public int? ResidentId { get; set; }
        public bool Reimburse { get; set; }
        public decimal TaxCredit { get; set; }
        public decimal OtherTaxableIncome { get; set; }
        public decimal WageDeduction { get; set; }
        public bool HasMoreThanOneJob { get; set; }
        public bool Using2020FederalW4Setup { get; set; }
    }

    //public class EmployeeTaxCompleteDto : EmployeeTaxDto
    public class EmployeeTaxCompleteDto : IHas2020TaxOptions
    {
        public int EmployeeTaxId { get; set; }
        public int EmployeeId { get; set; }
        //public Employee.Employee Employee { get; set; }
        public int? ClientTaxId { get; set; }
        //public ClientTax ClientTax { get; set; }
        public FilingStatus FilingStatus { get; set; }
        //public FilingStatusInfo FilingStatusInfo { get; set; }
        public byte NumberOfExemptions { get; set; }
        public byte NumberOfDependents { get; set; }

        public byte AdditionalTaxAmountTypeId { get; set; }

        public double AdditionalTaxPercent { get; set; }
        public double AdditionalTaxAmount { get; set; }

        public bool IsResident { get; set; }
        public bool IsActive { get; set; }

        public string Description { get; set; }

        public int ClientId { get; set; }
        //public Client Client { get; set; }
        public int? ResidentId { get; set; }
        public bool Reimburse { get; set; }
        public decimal TaxCredit { get; set; }
        public decimal OtherTaxableIncome { get; set; }
        public decimal WageDeduction { get; set; }
        public bool HasMoreThanOneJob { get; set; }
        public bool Using2020FederalW4Setup { get; set; }
        //public int? MaritalStatusId { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public bool IsFederalTax
        {
            get => !this.ClientTaxId.HasValue && this.Description.ToUpper() == "FEDERAL";
        }

        public bool IsStateTax(IDictionary<int, ClientTaxCompleteDto> clientTaxes)
        {
            if (!this.ClientTaxId.HasValue) return false;

            ClientTaxCompleteDto clientTax;
            var clientTaxWasFound = clientTaxes.TryGetValue(this.ClientTaxId.Value, out clientTax);

            //&& x?.LegacyStateTax?.StateId == employee.StateId

            return clientTaxWasFound && clientTax.StateTaxId.HasValue && clientTax.IsStateTax();
        }

        public bool IsSutaTax(IDictionary<int, ClientTaxCompleteDto> clientTaxes)
        {
            if (!this.ClientTaxId.HasValue) return false;

            ClientTaxCompleteDto clientTax;
            var clientTaxWasFound = clientTaxes.TryGetValue(this.ClientTaxId.Value, out clientTax);

            //&& x?.LegacyStateTax?.StateId == employee.StateId

            return clientTaxWasFound && !clientTax.StateTaxId.HasValue && clientTax.IsSutaTax();
        }
    }
}