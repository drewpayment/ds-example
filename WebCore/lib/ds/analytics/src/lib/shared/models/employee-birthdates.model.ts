export interface IEmployeeBirthdate{
    clientId: number;
    employeeId: number;
    firstName: string;
    lastName: string;
    dateOfBirth: Date | null;
    costCenterDescription: String;
    departmentName: String;
    age: number | null;
    clientDepartmentName: string;
    clientCostCenterName: string;
}