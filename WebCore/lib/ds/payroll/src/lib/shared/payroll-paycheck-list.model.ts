import { IEmployeeImage } from "@ajs/core/ds-resource/models/employee-image.model";

export interface IPayrollPayCheckList {
    voidCheck: boolean;
    genPaycheckHistoryId : number,
    genPayrollHistoryId  : number,
    checkAmount          : number,
    ownerId              : number,
    subCheck             : string,
    checkNumber          : number | null,
    grossPay             : number,
    netPay               : number,
    name                 : string,
    employeeNumber       : number,
    checkDate            : Date,
    payrollId            : number,
    viewrates            : number,
    isVendor             : Boolean
    vendorTypeId         : number  | null,
    profileImage         : IEmployeeImage;
    employeeId: number;
}
