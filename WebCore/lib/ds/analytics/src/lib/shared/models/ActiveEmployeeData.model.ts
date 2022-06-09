export interface ActiveEmployeeData {
    activeEmployees: ActiveEmployee[];
}

export interface ActiveEmployee {
    clientCostCenterId: number;
    clientCostCenterName: string;
    clientDepartmentId: number;
    clientDepartmentName: string;
    clientDivisionId: number;
    clientId: number;
    employeeId: number;
    employeeNumber: string;
    employeeStatus: string;
    firstName: string;
    isActive: boolean;
    lastName: string;
    middleInitial: string;
    separationDate: Date;
    hireDate: Date;
}