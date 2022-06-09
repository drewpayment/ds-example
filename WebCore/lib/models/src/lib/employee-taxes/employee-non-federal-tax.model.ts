import { KeyValue } from "@models/key-value.model";
import { IEmployeeTaxSetup } from "@models/employee-taxes/employee-tax-setup.model";

export interface IEmployeeNonFederalTax extends IEmployeeTaxSetup{
    stateTaxId?: number;
    localTaxId?: number;
    disabilityTaxId?: number;
    otherTaxId?: number;
    sutaTaxId?: number;
    employerPaidTaxId?: number;
    localTaxCode: string;
    taxTypeId: number;
    isReimbursable: boolean;
    blockOverrides: boolean;
    stateInfo: KeyValue;
    taxHasHistory: boolean;
}
