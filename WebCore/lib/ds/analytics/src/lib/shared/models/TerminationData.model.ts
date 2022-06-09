export interface TerminationData {
    totalCount: number;
    monthStartDate: Date;
    turnoverRate: string;
    retentionRate: string;
    growthRate: string;
    endCount: number;
    startCount: number;
    termedCount: number;
    newCount: number;
    hiredAndTermedCount: number;
    bothCount: number;
    terminatedEmployees: EmployeeTermination[];
    newHires: EmployeeTermination[];
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
    terminationReason: string;
}