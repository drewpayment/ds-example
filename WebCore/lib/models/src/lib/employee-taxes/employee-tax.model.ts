import { KeyValue } from "@models/key-value.model";

export interface IEmployeeTax{
    employeeTaxId: number;
    employeeId: number;
    clientTaxId?: number;
    filingStatusId: number;
    filingStatusDesc: string;
    numberOfExemptions: number;
    numberOfDependents: number;
    additionalAmountTypeId: number;
    additionalAmount: string;
    additionalPercent: string;
    isResident: boolean;
    isActive: boolean;
    description: string;
    clientId: number;
    residentId?: number;
    reimburse: boolean;
    taxCredit: string;
    otherTaxableIncome: string;
    wageDeduction: string;
    hasMoreThanOneJob: boolean;
    using2020FederalW4Setup: boolean;
    //withholdingOptions: KeyValue[];
}