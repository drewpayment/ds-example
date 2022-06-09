export interface ExceptionData {
    activeEmployees: EmployeeException[];
}

export interface EmployeeException {
    firstName: string;
    lastName: string;
    dateOccured: Date;
    minutes: number;
    fullName: string;
    eventDate: Date;
    hours: number;
    department: string;
    employeePunch: string;
    employeeSchedule: string;
}