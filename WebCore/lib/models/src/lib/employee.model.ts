import { EmployeeStatusType } from '@ds/employees/models/employee-status.enum';


export interface EmployeeBasic {
  employeeId: number;
  employeeNumber: string;
  firstName: string;
  lastName: string;
  middleInitial?: string;
  clientId?: number;
  clientDivisionId?: number;
  clientDepartmentId?: number;
  clientCostCenterId?: number;
  employeeStatusType?: EmployeeStatusType;
}
