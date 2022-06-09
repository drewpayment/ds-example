import { IEmployeeNonFederalTax } from "@models/employee-taxes/employee-non-federal-tax.model";
import { IEmployeeTaxGeneralInfo } from "./employee-tax-general-info.model";
import { IEmployeeTaxSetup } from "./employee-tax-setup.model";

export interface IEmployeeTaxAdmin{
    employeeId: number;
    clientId: number;
    federalTax: IEmployeeTaxSetup;
    nonFederalTaxes: IEmployeeNonFederalTax[];
    generalInfo: IEmployeeTaxGeneralInfo;
}
