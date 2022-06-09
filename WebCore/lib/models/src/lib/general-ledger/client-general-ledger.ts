export enum GeneralLedgerGroup_Cash {
    EmployeeChecks = 1,
    EmployeeDD = 2,
    VendorChecks = 3,
    VendorDD = 4,
    AllTaxes = 5,
    AllChecks = 33,
    DominionChecks = 36,
    AdjustmentChecks = 38,
    F941 = 40,
    TMCatchup = 43
}

export enum GeneralLedgerGroup_Liability {
    FederalTax = 6,
    SocialSecurity = 7,
    Medicare = 8,
    EmployerSocSec = 9,
    EmployerMedicare = 10,
    StateTax = 11,
    LocalTax = 12,
    SUTATax = 13,
    FUTATax = 14,
    Deductions = 15,
    GLAccrualLiability = 35,
    WorkersCompCredit = 41
}

export enum GeneralLedgerGroup_Expense {
    Earnings = 16,
    GrossPay = 17,
    EmployerSocSec = 18,
    EmployerMedicare = 19,
    FUTA = 20,
    SUTA = 21,
    Match = 34,
    Memo = 37,
    PremiumPay = 39,
    WorkersCompDebit = 42
}

export enum GeneralLedgerGroup_Payment {
    FederalTax = 22,
    SocialSecurity = 23,
    Medicare = 24,
    EmployerSocSec = 25,
    EmployerMedicare = 26,
    StateTax = 27,
    LocalTax = 28,
    SUTA = 29,
    FUTA = 30,
    ClientVendor = 31,
    DominionVendor = 32
}