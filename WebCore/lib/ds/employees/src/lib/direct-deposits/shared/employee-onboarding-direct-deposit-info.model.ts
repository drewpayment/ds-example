import { IEmployeeDirectDepositInfo } from "./employee-direct-deposit-info.model"

export interface IEmployeeOnboardingDirectDepositInfo extends IEmployeeDirectDepositInfo {
    employeeId:number,
    employeeDeductionId:number,
    employeeBankId:number
}
