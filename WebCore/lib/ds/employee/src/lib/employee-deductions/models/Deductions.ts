import { UserInfo } from '@ds/core/shared';

export interface IDeductions{
    numberPrefix: string;
    employeeDeductionID: number;
    clientDeductionID: number | null;
    employeeBankID: number;
    employeeBondID: number;
    accountType: number;
    clientPlanID: number;
    amount: number;
    deductionAmountTypeID: number;
    max: number;
    maxType: number;
    maxTypeDescription: string;
    totalMax: number;
    clientVendorID: number;
    additionalInfo: string;
    code: string;
    deduction: string;
    employeeBank: string;
    employeeBond: string;
    plan: string;
    amountType: string;
    descriptionCode: string;
    vendor: string;
    isActive: boolean;
    groupSequence: string; // 1 for earning, 2 for deduction, 3 for direct deposit
    earningDescription: string;
    accountNumber: string;
    routingNumber: string;
    isPreNote: boolean;
    clientCostCenterID: number;
};

export class IDeductionsData {
    data: IDeductions;
    option: string;
    clientInfo: IClientDeductionInfo;
    userInfo: UserInfo;
    reminderDateTime?: Date | null;
    fromHSA?: boolean;
    usedEarningsIds?: number[];
    multipleHundreds?: boolean;
}

export class IDeductionsResult {
    data: IDeductions;
}

export class IClientDeductionInfo {
    hasBankInfoSetup: boolean;
    aCHBankID: number;
    noPreNote: boolean;
    amountTypeList: any[];
    maxTypeList: IMaxTypeDeduction[];
    vendorList: IVendor[];
    planList: IPlanDeduction[];
    earningList: any[];
    costCenterList: IClientCostCenter[];
    clientEssOptions: IClientEssOptions;
}

export class IClientEssOptions {
    clientId: number;
    directDepositLimit? : number;
    allowDirectDeposit: boolean;
    manageDirectDepositAmount: boolean;
    manageDirectDepositAmountAndAccountInfo: boolean;
}

export class Deductions {

    employeeDeductionID: number;
    clientDeductionID: number | null;
    employeeBankID: number;
    employeeBondID: number;
    accountType: number; //1 for checking, 0 for savings
    accountTypeDescription: string;
    clientPlanID: number;
    amount: number;
    deductionAmountTypeID: number;
    max: number;
    maxType: number;
    maxTypeDescription: string;
    totalMax: number;
    clientVendorID: number;
    additionalInfo: string;
    code: string;
    deduction: string;
    employeeBank: string;
    employeeBond: string;
    plan: string;
    amountType: string;
    descriptionCode: string;
    vendor: string;
    isActive: boolean;
    groupSequence: string; // 1 for earning, 2 for deduction, 3 for direct deposit
    earningDescription: string;
    accountNumber: string;
    routingNumber: string;
    isPreNote: boolean;
    clientCostCenterID: number;
    numberPrefix: string;
    

    constructor(obj: IDeductions | null){
        if( obj != null){
            this.employeeDeductionID = obj.employeeDeductionID;
            this.clientDeductionID = obj.clientDeductionID;
            this.employeeBankID = obj.employeeBankID;
            this.employeeBondID = obj.employeeBondID;
            this.accountType = obj.accountType;
            this.accountTypeDescription = this.accountType == 1 ? "Checking" : "Savings";
            this.clientPlanID = obj.clientPlanID;
            this.amount = obj.amount;
            this.deductionAmountTypeID = obj.deductionAmountTypeID;
            this.max = obj.max;
            this.maxType = obj.maxType;
            this.maxTypeDescription = obj.maxTypeDescription;
            this.totalMax = obj.totalMax;
            this.clientVendorID = obj.clientVendorID;
            this.additionalInfo = obj.additionalInfo;
            this.code = obj.code;
            this.deduction = obj.deduction;
            this.employeeBank = obj.employeeBank;
            this.employeeBond = obj.employeeBond;
            this.plan = obj.plan;
            this.amountType = obj.amountType;
            this.descriptionCode = obj.descriptionCode;
            this.vendor = obj.vendor;
            this.isActive = obj.isActive;
            this.groupSequence = obj.groupSequence;
            this.earningDescription = obj.earningDescription;
            this.accountNumber = obj.accountNumber;
            this.routingNumber = obj.routingNumber;
            this.isPreNote = obj.isPreNote;
            this.clientCostCenterID = obj.clientCostCenterID;
            this.numberPrefix = obj.numberPrefix;
        }
        else{ //initializing defaults for creating a new object
            this.employeeDeductionID = 0;
            this.clientDeductionID = null;
            this.employeeBankID = 0;
            this.employeeBondID = 0;
            this.accountType = null; //defaulting to null
            this.accountTypeDescription = "";
            this.clientPlanID = 0;
            this.amount = 0;
            this.deductionAmountTypeID = 0;
            this.max = null;
            this.maxType = null;
            this.maxTypeDescription = "";
            this.totalMax = null;
            this.clientVendorID = 0;
            this.additionalInfo = "";
            this.code = "";
            this.deduction = "";
            this.employeeBank = "";
            this.employeeBond = "";
            this.plan = "";
            this.amountType = "";
            this.descriptionCode = "";
            this.vendor = "";
            this.isActive = true; //defaults to true
            this.groupSequence = "";
            this.earningDescription = "";
            this.accountNumber = "";
            this.routingNumber = "";
            this.isPreNote = true; //defaults to true
            this.clientCostCenterID = 0;
        }
    }
}

export class IPlanDeduction{
    clientPlanID: number;
    clientID: number;
    description: string;
    amount: number;
    deductionAmountTypeID: number;
    modifiedBy: number;
    isActive: boolean;
    deductionAmountType: string;
}

export class IMaxTypeDeduction{
    description: string;
    employeeDeductionMaxTypeID: number;
}

export class IVendor{
    clientId: number;
    clientVendorID: number;
    name: string;
}

export class IInsertEmployeeDeductionDto{
    clientDeductionID?: number;
    employeeBankID?: number;
    employeeBondID?: number;
    employeeID?: number;
    clientPlanID?: number;
    amount?: number;
    deductionAmountTypeID?: number;
    max?: number;
    maxType?: number;
    totalMax?: number;
    clientVendorID?: number;
    additionalInfo?: string;
    isActive?: boolean;
    modifiedBy?: number;
    clientCostCenterID?: number;
}

export class IUpdateEmployeeDeductionDto{
    employeeDeductionID?: number;
    clientDeductionID?: number;
    employeeBankID?: number;
    employeeBondID?: number;
    employeeID?: number;
    clientPlanID?: number;
    amount?: number;
    deductionAmountTypeID?: number;
    max?: number;
    maxType?: number;
    totalMax?: number;
    clientVendorID?: number;
    additionalInfo?: string;
    isActive?: boolean;
    modifiedBy?: number;
    clientCostCenterID?: number;
}

export class IInsertEmployeeBankDto{
    accountType: number;
    accountNumber: string;
    routingNumber: string;
    isPreNote: boolean;
    employeeID: number;
    clientID: number;
    modifiedBy: number;
}

export class IUpdateEmployeeBankDto{
    employeeBankID: number;
    accountType: number;
    accountNumber: string;
    routingNumber: string;
    isPreNote: boolean;
    employeeID: number;
    clientID: number;
    modifiedBy: number;
}

export class IDeleteEmployeeDeductionDto{
    employeeDeductionID: number;
    userID: number;
}

export class IDeleteEmployeeBankDeductionDto{
    employeeDeductionID: number;
    employeeBankID: number;
    modifiedBy: number;
}

export class IDeductionDescription{
    clientDeductionID: number;
    descriptionCode: string;
}

export class IAmountTypeDescription{
    description: string;
    employeeDeductionAmountTypeID: number;
    displayOrder: number;
    numberPrefix: string;
}

export class IInsertEmployeeBankDeductionDto{
    accountType: number;
    accountNumber: string;
    routingNumber: string;
    isPreNote: boolean;
    employeeID: number;
    clientID: number;
    modifiedBy: number;
    employeeDeductionID: number;
}

export class IInsertEffectiveDateDto{
    effectiveDate: Date | string;
    table: string;
    column: string;
    oldValue: string;
    newValue: string;
    appliedBy: string;
    appliedOn: Date | string;
    createdBy: string;
    createdOn: Date | string;
    type: number;
    tablePKID: number;
    friendlyView: string;
    accepted: number;
    employeeID: number;
    dataType: string;
}

export class IClientCostCenter{
    clientCostCenterId: number;
    clientId: number;
    code: string;
    description: string;
    isActive: boolean;
    isDefaultGlCostCenter: boolean;
    modified: Date | string;
}
