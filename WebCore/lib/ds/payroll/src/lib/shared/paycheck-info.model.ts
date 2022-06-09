
export interface IPaycheckInfo {
    genPaycheckHistoryId:   number;
    genPayrollHistoryId:    number;
    employeeId:             number;
    periodEnd:              Date;
    checkAmount:            number;
    checkNumber:            number;
    grossPay:               number;
    partialGrossPay:        number;
    tips:                   number;
    partialTips:            number;
    netPay:                 number;
    isAdjustment:           boolean;
    isFICAExempt:           boolean;
    isFUTAExempt:           boolean;
    isSUTAExempt:           boolean;
    isIncomeTaxExempt:      boolean;
    is1099Exempt:           boolean;
    socSecWages:            number;
    medicareWages:          number;
    FUTAWages:              number;
    medicareTax:            number;
    employerMedicareTax:    number;
    socSecTax:              number;
    employerSocSecTax:      number;
    employerFUTATax:        number;
    exemptWages:            number;
    flexDeductions:         number;
    totalTax:               number;
    straightHours:          number;
    straightPay:            number;
    premiumHours:           number;
    isLT3PSP:               boolean;
    isST3PSP:               boolean;
    tipCredits:             number;
    hireActWages:           number;
    hireActCredit:          number;
    clientId:               number;
    customGrossPay:         number;
    payrollCheckDate:       Date;
    payrollPayrollId:       number;
    isStateTaxExempt:       boolean;
    void:                   boolean;
    payrollId:              number;
    checkDate:              Date;
    periodStart:            Date;
    bankAccount:            string;
    bankId:                 number;
    altBankId:              number | null;
    taxAccount:             string;
    taxBankId:              number;
    debitAccount:           string;
    debitBankId:            number | null;
    payrollRunTypeId:       number;
    deductions:             IPaycheckDeduction[];
    earnings:               IPaycheckEarnings[];
    disbursements:          IPaycheckDisbursement[];
    companyPaidBenefits:    IPaycheckCompanyPaidBenefit[];
}

export interface IPaycheckDeduction {
    uniqueId:               string;
    displayOrder:           IPaycheckDeductionDisplayOrder;
    description:            string;
    employeeId:             number;
    currentAmount:          number;
    ytdAmount:              number;
    genPaycheckHistoryId:   number | null;
}

export enum IPaycheckDeductionDisplayOrder {
    grossPay            = 100,
    preTaxDeductions    = 110,
    taxableWages        = 150,
    medicareTax         = 200,
    socialSecurityTax   = 210,
    federalTax          = 250,
    stateTax            = 310,
    localTax            = 320,
    disabilityTax       = 330,
    tips                = 390,
    serviceChargeTips   = 395,
    deductions          = 400,
    adjustToNet         = 490,
    netPay_CheckAmount  = 700,
    miscellaneous_Memo  = 800
}

export interface IPaycheckEarnings {
    employeeId:             number;
    description:            string;
    hours:                  number;
    totalAmount:            number;
    ytdTotalAmount:         number;
    rate:                   number;
    amount:                 number;
    employeeClientRate:     number;
    sequence:               number;
    clientEarningId:        number;
    genPaycheckHistoryId:   number | null;
}

export interface IPaycheckDisbursement {
    employeeId:             number;
    headerOne:              string;
    headerTwo:              string;
    headerThree:            string;
    headerFour:             string;
    description:            string;
    groupCode:              number;
    displayOrder:           number;
    routingNumber:          string;
    accountNumber:          string;
    currentAmount:          number;
    printRoutingOption:     number;
    genPaycheckHistoryId:   number | null;
}

export interface IPaycheckCompanyPaidBenefit {
    displayOrder:           number;
    description:            string;
    employeeId:             number;
    current:                number;
    endYtd:                 number;
    genPaycheckHistoryId:   number | null;
}

export interface IPaycheckEarningsDetail {
    payCode:                string;
    earning:                string;
    rate:                   number;
    hours:                  number;
    pay:                    number;
    genPaycheckHistoryId:   number | null;
}

export interface IPaycheckEarningsHours {
    header1:                string;
    header2:                string;
    header3:                string;
    description:            string;
    groupCode:              number;
    displayOrder:           number;
    ytdTotalAmount:         number;
    currentRate:            number;
    genPaycheckHistoryId:   number | null;
}

export interface IPaycheckEarningsHoursList {
    header1:                string;
    header2:                string;
    groupCode:              number;
    paycheckHoursList:      IPaycheckEarningsHours[];
}

export interface IPaystubOptions { 
    checkStubOptionId:              number;
    clientId:                       number;
    checkStubTaxableGrossId:        number;
    checkStubStraightPremiumId:     number;
    checkStubDatesId:               number;
    checkStubNetPayId:              number;
    printRoutingAccount:            number;
    printRateChange:                boolean;
    printSocialSecurityNumber:      boolean;
    checkStubVoidDaysID:            number;
    printSSLastFourDigitsOnly:      boolean;
    punchDetail:                    boolean;
    specialCheckHeader:             string;
    tipsInGross:                    boolean;
    earningsDetail:                 boolean;
    printPointBal:                  boolean;
    showCheckDateInWindow:          boolean;
    lastPayPrintedAtEnd:            boolean;
    DeductionShowOnlyCurrent:       boolean;
    showLifetimeHours:              boolean;
    maskBankInfo:                   boolean;
    combineCompanyEarnings:         boolean;
    voidDays:                       string;
    pointBalance:                   string;
    showWindow:                     boolean;
}