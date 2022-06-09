using System;

using Dominion.Core.Dto.Onboarding;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Taxes.Dto.TaxOptions;

namespace Dominion.Domain.Entities.Onboarding
{
    public  class EmployeeOnboardingW4Assist : Entity<EmployeeOnboardingW4Assist>, IHasModifiedData
    {
        public virtual int EmployeeId { get; set; }
        public virtual FilingStatus? TaxFilingStatus { get; set; }
        public virtual W4MaritalStatus? MaritalStatus { get; set; }
        public virtual byte? HouseholdIncomeStatus { get; set; }
        public virtual byte? ChildCount { get; set; }
        public virtual byte? DependentCount { get; set; }
        public virtual bool? IsDependentOtherReturn { get; set; }
        public virtual bool? IsSpouseEmployed { get; set; }
        public virtual bool? IsWidowerCurrYr { get; set; }
        public virtual bool? HasChildren { get; set; }
        public virtual bool? HasDependents { get; set; }
        public virtual bool? IsDependentCareOverLimit { get; set; }
        public virtual bool? IsDependentCareClaim { get; set; }
        public virtual bool? IsNameDifferFromSSC { get; set; }
        public virtual bool? DoYouHaveAnotherJob { get; set; }
        public virtual bool? IsSecondJobAndSpouseEarnMoreThan1500 { get; set; }
        public virtual bool? IsDependentOrChildCareExpensesIsMoreThan2000 { get; set; }
        public virtual int? ChildTaxCredit { get; set; }
        public virtual DateTime CreateDt { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual int? TotalExemptions { get; set; }
        public virtual bool? IsEmployeeBlind { get; set; }
        public virtual bool? IsSpouseBlind { get; set; }
        public virtual bool? IsSpouseOver65 { get; set; }
        public virtual Employee.Employee Employee                       { get; set; }
    }
}
