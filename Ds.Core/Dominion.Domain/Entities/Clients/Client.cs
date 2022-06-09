using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Aca;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Api;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Benefit;
using Dominion.Domain.Entities.Billing;
using Dominion.Domain.Entities.EEOC;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Domain.Entities.User;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Clients
{
    public class Client : Entity<Client>, IModifiableEntity<Client>
    {
        // Basic Properties
        public virtual int ClientId { get; set; }
        public virtual string ClientName { get; set; }
        public virtual string TaxPayerName { get; set; }
        public virtual string ClientCode { get; set; }
        public virtual string FederalIDNumber { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string City { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual int StateId { get; set; }
        public virtual State State { get; set; }
        public virtual int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string PhoneExtension { get; set; }
        public virtual string FaxNumber { get; set; }
        public virtual string FullServicePin { get; set; }
        public virtual string TaxMgmtPin { get; set; }
        public virtual string Form941Pin { get; set; }
        public virtual string NameControl { get; set; }
        public virtual string SoftwareContactEmailAddress { get; set; }
        public virtual int? ConvertedFromId { get; set; }
        public virtual ClientStatusType? ClientStatus { get; set; }
        public virtual int? ReasonId { get; set; }
        public virtual DateTime? TermDate { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? TaxManagementDate { get; set; }
        public virtual DateTime? FiscalStartDate { get; set; }
        public virtual string SalesRepresentative { get; set; }
        public virtual string EmployeeRepresentative { get; set; }
        public virtual string Notes { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool SecuritySettings { get; set; }
        public virtual DateTime? PowerOfAttorneyFederalDate { get; set; }
        public virtual DateTime? PowerOfAttorneyStateDate { get; set; }
        public virtual DateTime? AllowAccessUntilDate { get; set; }
        public virtual bool UseTaxPayerOnW2 { get; set; }
        public virtual bool IsFederalIdSecondary { get; set; }
        public virtual int AllowTurboTax { get; set; }
        public virtual string ArcadiaTpa { get; set; }
        public virtual int? PayrollAdminUserId { get; set; }
        public virtual DateTime? TimeAttDate { get; set; }
        public virtual int? TimeAttAdminUserId { get; set; }
        public virtual DateTime? AppTrackDate { get; set; }
        public virtual int? AppTrackAdminUserId { get; set; }
        public virtual DateTime? BenAdminDate { get; set; }
        public virtual int? BenAdminAdminUserId { get; set; }
        public virtual DateTime? OnboardDate { get; set; }
        public virtual int? OnboardAdminUserId { get; set; }
        public virtual int? PayrollSalesUserId { get; set; }
        public virtual int? TimeAttSalesUserId { get; set; }
        public virtual int? AppTrackSalesUserId { get; set; }
        public virtual int? BenAdminSalesUserId { get; set; }
        public virtual int? OnboardSalesUserId { get; set; }
        public virtual int? IndustryId { get; set; }

        // Entity Reference Collections
        public virtual ICollection<ClientApiAccount> ClientApiAccount { get; set; } // many-to-many ClientApiAccount.FK_ClientApiAccount_Client;
        public virtual ICollection<AcaCompanyStatus> AcaCompanyStatuses { get; set; }
        public virtual ICollection<Employee.Employee> Employees { get; set; }
        public virtual ICollection<ClockClientSchedule>  Schedules { get; set; }
        public virtual ICollection<ClientAccountFeature> AccountFeatures { get; set; }
        public virtual ICollection<ClientAccountOption> AccountOptions { get; set; }
        public virtual ICollection<ClientCostCenter> ClientCostCenters { get; set; }
        public virtual ICollection<ClientEarning> Earnings { get; set; }
        public virtual ICollection<Payroll.Payroll> Payrolls { get; set; }
        public virtual ICollection<ClientDivision> Divisions { get; set; }
        public virtual ICollection<UserClient> UserClients { get; set; }
        public virtual ICollection<GroupSchedule> GroupSchedules { get; set; }
        public virtual ICollection<ClientOrganization> ClientRelations { get; set; }
        public virtual ICollection<ClientContact> ClientContacts { get; set; }
        public virtual ICollection<AcaAleMemberClient> AcaAleMemberClients { get; set; }
        public virtual ICollection<PayrollHistory> PayrollHistory { get; set; } 
        public virtual ICollection<BillingItem> BillingItems { get; set; } 
        public virtual ICollection<AcaClientConfiguration> AcaClientConfigurations { get; set; } 
        public virtual ICollection<AcaTransmissionSubmission> AcaTransmissionSubmissions { get; set; } 
        public virtual ICollection<BenefitPackage> BenefitPackages { get; set; }
        public virtual ICollection<JobProfile> JobProfile { get; set; }
        public virtual ICollection<ClientWorkersComp> ClientWorkersComp { get; set; }
        public virtual ICollection<ClientGroup> ClientGroups { get; set; }
        public virtual ICollection<ClientShift> ClientShift { get; set; } //if adding, set up configuration for mapping
        public virtual ClientEEOC ClientEeoc { get; set; }
        public virtual ClientEssOptions ClientEssOptions { get; set; } // one-to-one ClientEssOptions.FK_ClientEssOptions_Client;
        public virtual ClientBankInfo ClientBankInfo { get; set; } // one-to-one ClientBankInfo.FK_ClientBankInfo_Client;
        public virtual ICollection<ClientBank> ClientBanks { get; set; }
        public virtual ICollection<EmployeeDeductionAmountTypeInfo> ClientDeductionTypes { get; set; }
        public virtual ICollection<SpecialInstruction> SpecialInstructions { get; set; }
        public virtual ApplicantClient ApplicantClient { get; set; } // one-to-one applicantClient.FK_ApplicantClient_Client;
        public virtual ICollection<ClientJobSite> ClientJobSites { get; set; }
        public virtual ICollection<ReviewTemplate> ReviewTemplates { get; set; }
        public virtual ICollection<ScoreModel> ScoreModels { get; set; }

        // Entity Reference
        public virtual ClientCalendar ClientCalendar { get; set; }
        public virtual ClientGLSettings ClientGLSettings { get; set; }
        // IModifiableEntity Properties
        public virtual DateTime LastModifiedDate { get; set; }
        public virtual string LastModifiedByDescription { get; set; }
        public virtual User.User LastModifiedByUser { get; private set; }
        public virtual int LastModifiedByUserId { get; private set; }

        public void SetLastModifiedValues(int lastModifiedByUserId, string lastModifiedByUserName, 
            DateTime lastModifiedDate)
        {
            LastModifiedByUserId = lastModifiedByUserId;
            LastModifiedByDescription = lastModifiedByUserName;
            LastModifiedDate = lastModifiedDate;
        }

        #region FILTERS

        public static Expression<Func<Client, bool>> HasEmployeesWithDependentCountOfAtLeast(int employeeDependentCount)
        {
            return
                c =>
                    c.Employees.Any(
                        e =>
                            Employee.Employee.HasEmployeeDependentCountOfAtLeast(employeeDependentCount)
                                .Compile()
                                .Invoke(e));
        }

        public static Expression<Func<Client, bool>> HasEmployeeCountOfAtLeast(int employeeCount)
        {
            return c => c.Employees.Count() >= employeeCount;
        }

        public static Expression<Func<Client, bool>> HasAccountFeature(AccountFeatureEnum feature)
        {
            return c => c.AccountFeatures.Any(f => f.AccountFeature == feature);
        }

        #endregion // region FILTERS

        public virtual ICollection<EEOCLocation> EEOCLocations { get; set; } // many-to-one;
        //public virtual ClientEEOC ClientEEOC { get; set; } // one-to-one ClientEEOC.FK_ClientEEOC_Client;
        public virtual ICollection<GenW2EmployeeHistory> EmployeeW2Totals { get; set; }
        public virtual ICollection<PaycheckEarningHistory> PaycheckEarningHistory { get; set; }

        public virtual ICollection<Competency> Competencies { get; set; }
        public virtual ICollection<CompetencyModel> CompetencyModels { get; set; }
        public virtual ICollection<ReviewProfile> ReviewProfiles { get; set; }
        public virtual ICollection<ClientTurboTax> ClientTurboTax { get; set; }

        public virtual ICollection<GenW2ClientHistory> ClientW2Histories { get; set; }
    }


// class Client
}