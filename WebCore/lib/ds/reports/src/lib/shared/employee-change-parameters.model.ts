export interface IEmployeeChangeParameters {
    employeeId              : number;
    startDate               : string;
    endDate                 : string;
    sequenceId              : number;
    payType                 : number;
    employeeStatusId        : number;
    returnFilterOnly        : Boolean;
    csvChangeLogIds         : string;
}