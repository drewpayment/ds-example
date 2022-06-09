import { Pipe, PipeTransform } from '@angular/core';
import { CurrencyPipe } from '@angular/common';

@Pipe({
  name: 'payrollRequestReportCurrency'
})
export class PayrollRequestReportCurrencyPipe implements PipeTransform {
  constructor(
    private ngCurrencyPipe: CurrencyPipe
  ) {
  }

  transform(value: any): any {
    var digitsInfo: string;

    const properNumber = +value;
    if(isNaN(properNumber)){
      throw new TypeError('The provided value: ' + JSON.stringify(value) + ' cannot be converted into a number')
    }

    if(Number.isInteger(value)){
      digitsInfo = '1.0-0';
    } else {
      digitsInfo = '1.2-4'
    }

    return this.ngCurrencyPipe.transform(value, 'USD', 'symbol', digitsInfo)

  }

}
