using System;
using System.Collections.Generic;
using System.Text;

namespace Dominion.Core.Dto.Client
{
    public class Client401KSetupDto
    {
        public int Client401KSetupId { get; set; }
        public int ClientId { get; set; }
        public int Client401KProviderId { get; set; }
        public string ContractPlanNumber { get; set; }
        public int FrequencyId { get; set; }
        public string FileName { get; set; }
        public int? TaxYear { get; set; }
        public int? BankId { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int EmployeeOptionId { get; set; }
        public string SubPlanNumber { get; set; }
        public int EmployeeTypeToInclude { get; set; }
        public string ContractPlanName { get; set; }
        public string RecordVersionNumber { get; set; }
        public string TransmissionNumber { get; set; }
        public string DemographicLocationCode { get; set; }
        public string DemographicSubsetCode { get; set; }
        public string ContributionPlanCode { get; set; }
        public string VendorName { get; set; }
        public bool IsMep { get; set; }
        public string DemographicLanguageCode { get; set; }
        public int? DepositMethod { get; set; }
        public string AchNumber { get; set; }
        public bool IsExcludeIneligible { get; set; }
        public bool IsIncludeForfeituresByDivision { get; set; }
    }
}
