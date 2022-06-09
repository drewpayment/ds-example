export interface IEmployeePayType {
    payTypeId      : EmployeePayTypeEnum;
    description     : string;
}

export enum EmployeePayTypeEnum {
    hourly = 1,
    salary = 2,
    hourlyAndSalary = 3
}
