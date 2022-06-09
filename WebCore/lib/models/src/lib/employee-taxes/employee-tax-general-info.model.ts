export interface IEmployeeTaxGeneralInfo {
    employeePayId: number;
    sutaClientTaxId?: number;
    psdCode: string;
    wotcReasonId?: number;
    deferEmployeeSocSecTax: boolean;
    is1099Exempt: boolean;
    isFicaExempt: boolean;
    isFutaExempt: boolean;
    isSutaExempt: boolean;
    isSocSecExempt: boolean;
    isStateTaxExempt: boolean;
    isIncomeTaxExempt: boolean;
    isHireActQualified: boolean;
    hireActStartDate: Date;
    hasYtdTaxOrWages: boolean;
    clientHasReimbursableEarning: boolean;
    allowIncomeWageExemptOption: boolean;
}