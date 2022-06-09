import { Pipe, PipeTransform } from '@angular/core';
import { TimeOffUnit } from '../shared';
import { coerceNumberProperty } from '@angular/cdk/coercion';


@Pipe({
    name: 'timeOffUnitName'
})
export class TimeOffUnitPipe implements PipeTransform {
    timeOffUnitTypes: TimeOffUnit[] = [
        { timeOffUnitTypeId: 1, name: 'Hours' },
        { timeOffUnitTypeId: 2, name: 'Days' }
    ];
    transform(value: string, ...args: any[]) {
        const id = coerceNumberProperty(value);
        const unit = this.timeOffUnitTypes.find(u => u.timeOffUnitTypeId === id);
        return unit != null ? unit.name : '';
    }
}
