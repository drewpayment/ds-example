import { Pipe, PipeTransform } from '@angular/core';
import { convertToMoment } from '../shared/convert-to-moment.func';

@Pipe({ name: 'convertToMoment' })
export class ConvertToMomentPipe implements PipeTransform {
    readonly transform = convertToMoment;
}
