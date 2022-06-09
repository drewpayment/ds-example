

export interface EmployeeNavigatorEmpRequiredFields {
  employeeId: number;
  firstName: string;
  lastName: string;
  isActive: boolean;
  separationDate: Date;
  missingFields: string[];
}
