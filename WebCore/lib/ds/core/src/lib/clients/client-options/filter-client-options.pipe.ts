import { Pipe, PipeTransform } from '@angular/core';
import { IAccountOptionInfoWithClientSelection, ClientOptionCategory } from '../shared';

@Pipe({
  name: 'filterClientOptions'
})
export class FilterClientOptionsPipe implements PipeTransform {

  transform(value: IAccountOptionInfoWithClientSelection[], args?: string): any {
    if (!Array.isArray(value)) {
      return null;
    }

    if(args == "payroll") {
      return value.filter(opt => {
        return opt.category == ClientOptionCategory.payroll;
      });
    }
    if(args == "reporting") {
      return value.filter(opt => {
        return opt.category == ClientOptionCategory.reporting;
      });
    }

    return value;

  }

}
