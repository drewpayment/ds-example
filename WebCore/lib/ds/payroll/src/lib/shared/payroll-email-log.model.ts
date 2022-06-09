export interface IPayrollEmailLog {
    payrollEmailLogId : number,
    clientId          : number,
    payrollId         : number,
    logType           : number,
    startDate         : Date,
    endDate           : Date,
    modifiedBy        : number,
    hasError          : Boolean,

    //ADD ONS
    buttonClass : string,
    icon        : string
}