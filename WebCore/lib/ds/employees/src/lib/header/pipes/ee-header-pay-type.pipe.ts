import { Pipe, PipeTransform } from '@angular/core';
import { PayType } from '@ds/employees/models/pay-type.enum';


@Pipe({
  name: 'payType',
})
export class EmployeeHeaderPayTypePipe implements PipeTransform {

  transform(value: number, ...args: any[]) {
    switch (value) {
      case PayType.hourly:
        return 'Hourly';
      case PayType.salary:
        return 'Salary';
      case PayType.noneSelected:
      default:
        return 'N/A';
    }
  }

}
