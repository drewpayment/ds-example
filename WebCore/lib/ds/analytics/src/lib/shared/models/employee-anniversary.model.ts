export interface IEmployeeAnniversary{
    clientId: number;
    employeeId: number;
    firstName: string;
    lastName: string;
    AnniversaryDate: Date | null;
    costCenterDescription: String;
    departmentName: String;
    yearsOfService: Number | null;
    hireDate: Date | null;
    ninetyDayAnniversaryDate: Date | null;
    clientDepartmentName: string;
    clientCostCenterName: string;
}