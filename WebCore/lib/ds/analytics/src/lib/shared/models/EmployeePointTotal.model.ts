export interface EmployeePointTotal {
    employeePointsId: number,
    amount: number,
    clientId: number,
    dateOccurred: Date,
    employeeId: number,
    hours: number,
    firstName: string,
    lastName: string,
    department: string,
    clientDepartmentId: number,
    employee: Employee,
    costCenter: string,
    directSupervisor: string,
    directSupervisorId: number,
    clientCostCenterId: number,
    supervisor: string
}

export interface Employee {
    employeeId: number,
    directSupervisorId: number,
    firstName: String,
    lastName: String,
    department: Department,
    costCenter: CostCenter,
    directSupervisor: Employee
}

export interface Department {
    clientDepartmentId: number,
    name: String,
    code: number
}

export interface CostCenter {
    clientCostCenterId: number,
    code: number,
    description: String,
    glClassName: String
}