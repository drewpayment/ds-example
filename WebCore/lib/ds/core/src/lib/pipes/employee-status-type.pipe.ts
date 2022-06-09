import { Pipe, PipeTransform } from '@angular/core';
import { EmployeeStatusType } from '@ds/employees/models/employee-status.enum';

@Pipe({ name: 'employeeStatusType' })
export class EmployeeStatusTypePipe implements PipeTransform {
  transform(input: EmployeeStatusType) {
    switch (input) {
      case EmployeeStatusType.fullTime:
        return 'Full Time';
      case EmployeeStatusType.callIn:
        return 'Call In';
      case EmployeeStatusType.fullTimeTemp:
        return 'Full Time Temp';
      case EmployeeStatusType.lastPay:
        return 'Last Pay';
      case EmployeeStatusType.manager:
        return 'Manager';
      case EmployeeStatusType.militaryLeave:
        return 'Military Leave';
      case EmployeeStatusType.partTime:
        return 'Part Time';
      case EmployeeStatusType.partTimeTemp:
        return 'Part Time Temp';
      case EmployeeStatusType.seasonal:
        return 'Seasonal';
      case EmployeeStatusType.severance:
        return 'Severance';
      case EmployeeStatusType.special:
        return 'Special';
      case EmployeeStatusType.studentIntern:
        return 'Student Intern';
      case EmployeeStatusType.furlough:
        return 'Furlough';
      case EmployeeStatusType.layoff:
        return 'Layoff';
      case EmployeeStatusType.leaveOfAbsence:
        return 'Leave of Absence';
      case EmployeeStatusType.retired:
        return 'Retired';
      case EmployeeStatusType.terminated:
        return 'Terminated';
      default:
        return '';
    }
  }
}
