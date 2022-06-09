import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';
import { IOnboardingEmployee } from 'apps/ds-company/src/app/models/onboarding-employee.model';

@Pipe({
    name: 'hireDateFilter',
    pure: true
})
export class HireDateFilterPipe implements PipeTransform {
  
    transform(items: IOnboardingEmployee[], filters?:{dateFrom?:string, dateTo?: string }): IOnboardingEmployee[] {
      if(!items){
        return [];
      }
      
      return items.filter(item => {
        var result = true;
        if (filters.dateFrom) {
            result = result && (item.hireDate >= new Date(filters.dateFrom));
        }
        if (filters.dateTo) {
            result = result && (item.hireDate <= new Date(filters.dateTo));
        }
        return result;
      });
    }
}

@Pipe({
    name: 'onboardingEmployeeFilter',
    pure: true
})
export class OnboardingEmployeeFilterPipe implements PipeTransform {
  
    transform(items: IOnboardingEmployee[], filterFn?:()=> boolean): IOnboardingEmployee[] {
      if(!items){
        return [];
      }
      
      return items.filter(filterFn);
    }
}