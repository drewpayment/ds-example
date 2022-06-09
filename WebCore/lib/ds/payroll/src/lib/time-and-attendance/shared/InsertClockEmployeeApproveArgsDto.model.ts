export interface InsertClockEmployeeApproveDateArgsDto{
    employeeID?: number;
    eventDate?: string;
    modifiedBy?: number;
    isApproved?: boolean;
    clientCostCenterID?: number;
    clockClientNoteID?: number;
    payToSchedule?: boolean;
}