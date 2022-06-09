export interface IEmployeeOnboardingTax{
    empTaxId: number,
    employeeId: number
    taxCategory: number,
    stateId: number,
    //localCode: string,
    //isTaxExempt: boolean,
    //isTaxExemptionLastYr: boolean,
    //isTaxExemptionCurrYr: boolean,
    //taxExemptReasonId: number
    allowances: number
    filingStatus: number,
    isAdditionalAmountWithheld: boolean,
    additionalWithholdingAmt: number,

    // isResident: boolean,
    // renaissanceZone: string,
    // taxExemptReason: string,
    // totalExemptions: number,
    createDt: Date,
    // modified: Date,
    // modifiedBy: number,
    // countyId: number,
    // employmentCountyId: number,
    // additionalExemptions: number,
    // additionalCountyWithholdingAmt: number,
    // isIndianaResident: boolean,
    // isEmployedInIndiana: boolean,
    // schoolDistrictId: number,

    // isAdditionalCountyAmountWithheld: boolean,
    //List<SchoolDistrictDto> SchoolDistricts,

    // isFederalSubtractions: boolean,
    // federalSubtractions: number,

    // allowableDeductionsToFedAdjGrossIncome: number,
    // incomeNotSubjectToWithholding: number,
    // estimatedItemizedDeductions: number,
    // publicEstimatedDeductionAllowances: number,
    // reciprocalStateId: number,
    // withheldTaxAtLowerRate: boolean,

    // birthDate: Date,
    // age: number,
    // recommendedAllowanceCount: number,

    //Fields added for 2020 FederalW4 Changes
    taxCredit: number,

    //next 2 are used to calculate EmployeeTax.TaxCredit = QualifyingChildren*2000 + OtherDependents*500
    qualifyingChildren: number,
    qualifyingChildrenAmount: number,
    otherDependents: number,
    otherDependentsAmount: number,
    otherTaxableIncome: number,
    wageDeduction: number,
    hasMoreThanOneJob: boolean,
    using2020FederalW4Setup: boolean,

    //StateW4 Changes - PAY-1001
    // isSpouseOver65: boolean,
    // isBlind: boolean,
    // isSpouseBlind: boolean,
    // isSpouseEmployed: boolean,
    // totalNumberOfDependents: number,
    // isClaimedAsDependent: boolean,
    // isClaimingSpouseExemption: boolean,
    // isMarriedFilingSeparately: boolean,
};
