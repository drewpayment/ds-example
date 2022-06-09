import { Pipe, PipeTransform } from '@angular/core';
import { EmployeeStatusType } from '@ds/employees/models/employee-status.enum';


@Pipe({
  name: 'empStatus',
})
export class EmployeeHeaderEmployeeStatusPipe implements PipeTransform {

  transform(value: number, ...args: any[]) {
    switch (value) {
      case EmployeeStatusType.fullTime:
        return 'Full Time';
      case EmployeeStatusType.partTime:
        return 'Part Time';
      case EmployeeStatusType.callIn:
        return 'Call In';
      case EmployeeStatusType.special:
        return 'Special';
      case EmployeeStatusType.layoff:
        return 'Layoff';
      case EmployeeStatusType.terminated:
        return 'Terminated';
      case EmployeeStatusType.manager:
        return 'Manager';
      case EmployeeStatusType.lastPay:
        return 'Last Pay';
      case EmployeeStatusType.fullTimeTemp:
        return 'Full Time Temp';
      case EmployeeStatusType.militaryLeave:
        return 'Military Leave';
      case EmployeeStatusType.leaveOfAbsence:
        return 'Leave of Absence';
      case EmployeeStatusType.studentIntern:
        return 'Student Intern';
      case EmployeeStatusType.retired:
        return 'Retired';
      case EmployeeStatusType.seasonal:
        return 'Seasonal';
      case EmployeeStatusType.partTimeTemp:
        return 'Part Time Temp';
      case EmployeeStatusType.severance:
        return 'Severance';
      case EmployeeStatusType.furlough:
        return 'Furlough';
      case EmployeeStatusType.unknown:
      default:
        return 'Unknown';
    }
  }

}
