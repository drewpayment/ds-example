import { Pipe, PipeTransform } from '@angular/core';
import { IPayrollRequestItem } from '../../shared/payroll-request-item.model';

@Pipe({
  name: 'payrollRequestRequestType',
  pure: true
})
export class PayrollRequestRequestTypePipe implements PipeTransform {

  transform(data: IPayrollRequestItem[], requestType: number): any {
    return data.filter(item =>  (item.requestType == requestType) );
  }

}
