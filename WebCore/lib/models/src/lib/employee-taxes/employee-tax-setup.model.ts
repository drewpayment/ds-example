import { KeyValue } from "@models/key-value.model";
import { IEmployeeTax } from "./employee-tax.model";
export interface IEmployeeTaxSetup extends IEmployeeTax {
    filingStatuses: KeyValue[];
    withholdingOptions: KeyValue[];
}