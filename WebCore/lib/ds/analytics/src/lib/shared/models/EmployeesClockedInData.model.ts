export interface EmployeesClockedIn {
    clockedInEmployees: ClockedInEmployees[];
}

export interface ClockedInEmployees {
    clientDepartmentID: number;
    department: string;
    employeeID: number;
    employeeName: string;
    filter: number;
    lastPunch: string;
    punch1: string;
    punch2: string;
    punch3: string;
    punch4: string;
    startTime: Date;
    stopTime: Date;
    hoursScheduled: number;
    hoursWorked: number;
    overtime: number;
    supervisorName: string;
}