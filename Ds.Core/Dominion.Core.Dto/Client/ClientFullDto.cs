using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientFullDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string TaxPayerName { get; set; }
        public string ClientCode { get; set; }
        public string FederalIDNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneExtension { get; set; }
        public string FaxNumber { get; set; }
        public string FullServicePin { get; set; }
        public string TaxMgmtPin { get; set; }
        public string Form941Pin { get; set; }
        public string NameControl { get; set; }
        public string SoftwareContactEmailAddress { get; set; }
        public int? ConvertedFromId { get; set; }
        public ClientStatusType? ClientStatus { get; set; }
        public int? ReasonId { get; set; }
        public DateTime? TermDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? TaxManagementDate { get; set; }
        public DateTime? FiscalStartDate { get; set; }
        public string SalesRepresentative { get; set; }
        public string EmployeeRepresentative { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public bool SecuritySettings { get; set; }
        public DateTime? PowerOfAttorneyFederalDate { get; set; }
        public DateTime? PowerOfAttorneyStateDate { get; set; }
        public DateTime? AllowAccessUntilDate { get; set; }
        public bool UseTaxPayerOnW2 { get; set; }
        public bool IsFederalIdSecondary { get; set; }
        public int AllowTurboTax { get; set; }
        public string ArcadiaTpa { get; set; }
        public int? PayrollAdminUserId { get; set; }
        public DateTime? TimeAttDate { get; set; }
        public int? TimeAttAdminUserId { get; set; }
        public DateTime? AppTrackDate { get; set; }
        public int? AppTrackAdminUserId { get; set; }
        public DateTime? BenAdminDate { get; set; }
        public int? BenAdminAdminUserId { get; set; }
        public DateTime? OnboardDate { get; set; }
        public int? OnboardAdminUserId { get; set; }
        public int? PayrollSalesUserId { get; set; }
        public int? TimeAttSalesUserId { get; set; }
        public int? AppTrackSalesUserId { get; set; }
        public int? BenAdminSalesUserId { get; set; }
        public int? OnboardSalesUserId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int? IndustryId { get; set; }
    }
}
