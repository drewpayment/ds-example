import { Pipe, PipeTransform } from '@angular/core';
import { IEEOCEmployeeInfo } from '@ds/core/employees/shared/employee-eeoc.model';
import { isNullOrUndefined } from 'util';
import { IEEOCLocationDataPerMultiClient } from '@ajs/job-profiles/shared/models/eeoc-location-data-per-client.interface';

@Pipe({ name: 'locationFilter' })
export class LocationFilterPipe implements PipeTransform {
    transform(clientLocArray: IEEOCLocationDataPerMultiClient[], userRow: IEEOCEmployeeInfo): IEEOCLocationDataPerMultiClient[] {
        const returnLocations: IEEOCLocationDataPerMultiClient[] = [];
        if (!isNullOrUndefined(clientLocArray)) {
            clientLocArray.forEach(obj => {
                if (obj.clientId === userRow.clientId) returnLocations.push(obj);
            });
        }
        return returnLocations;
    }
}
