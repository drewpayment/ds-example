import { Pipe, PipeTransform } from '@angular/core';
import { IClientData } from '@ajs/onboarding/shared/models';
import { isNullOrUndefined } from 'util';

@Pipe({ name: 'payrollFilter' })
export class PayrollFilterPipe implements PipeTransform {
    transform(clientPayrollData: any[], currentClient: IClientData): number[] {
        let returnPayPeriods: number[] = [];
        if (!isNullOrUndefined(clientPayrollData)) {
            clientPayrollData.forEach(obj => {
                if (obj && obj.clientId == currentClient.clientId) returnPayPeriods = returnPayPeriods.concat(obj.payPeriods);
            });
        }
        return returnPayPeriods;
    }
}
