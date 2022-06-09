// in the lib/ds/core/src/lib/employees/shared folder
// add employee-actions.ts file

const EMPLOYEE   = "Employee";

export const EMPLOYEE_ACTIONS = {
    Employee: {
        ReadHourlyEmployeeInfo:  `${EMPLOYEE}.ReadHourlyEmployeeInfo`,
        ReadSalaryEmployeeInfo: `${EMPLOYEE}.ReadSalaryEmployeeInfo`,
        EmployeeUpdate: `${EMPLOYEE}.EmployeeUpdate`,
        EmployeeEmergencyContactUpdate: `${EMPLOYEE}.EmployeeEmergencyContactUpdate`,
        EmployeeDependentUpdate: `${EMPLOYEE}.EmployeeDependentUpdate`,
    }
};
