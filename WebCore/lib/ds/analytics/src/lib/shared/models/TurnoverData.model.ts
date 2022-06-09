export interface TurnoverData {
    totalCount: number;
    turnoverRate: string;
    terminatedEmployees: EmployeeTermination[];
}

export interface EmployeeTermination {
    active: boolean;
    costCenter: string;
    department: string;
    employeeId: number;
    employeeStatus: string;
    fullName: string;
    hireDate: Date;
    lastName: string;
    modified: Date;
    separationDate: Date;
    terminationDate: Date;
}