import { FilingStatus } from '@ds/employees/taxes/shared/filing-status';

export interface IEmployeeTaxInfo {
    employeeId: number;
    employeeTaxId: number;
    description: string;

}

export interface IEmployeeTaxDetails {
    employeeId: number;
    employeeTaxId: number;
    filingStatusId: FilingStatus;
    numberOfExemptions: number;
    filingStatusDescription: string;
    taxCredit: number;
    wageDeduction: number;
    otherTaxableIncome: number;
    hasMoreThanOneJob: boolean;
    using2020FederalW4Setup: boolean;
    additionalAmount: number;
    additionalPercent: number;

}
