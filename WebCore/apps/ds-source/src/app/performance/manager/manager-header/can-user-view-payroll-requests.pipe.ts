import { Pipe, PipeTransform } from '@angular/core';
import { CanUserViewPayrollRequests } from '../guard/payroll-request.guard';

/**
 * Controls whether the Payroll Request tab is viewable.  @see manager-header.component.html
 */
@Pipe({
  name: 'canUserViewPayrollRequests'
})
export class CanUserViewPayrollRequestsPipe implements PipeTransform {

  readonly transform = CanUserViewPayrollRequests;

}
