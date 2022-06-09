import { Pipe, PipeTransform } from '@angular/core';
import { Maybe } from '@ds/core/shared/Maybe';

@Pipe({
  name: 'isExceptionRow'
})
export class IsExceptionRowPipe implements PipeTransform {
// add memoization

  transform = isExceptionRow;

}

function normalizeString(val: string): string {
    return new Maybe(val)
    .map(x => x.toLowerCase())
    .map(x => x.trim())
    .valueOr('');
}

const exceptionCauses = [
    'Arrived',
    'Pending',
    'Missing',
    'Tardy',
    'Left',
    'Location',
].map(x => normalizeString(x));

function isException(input: string): boolean {
    return exceptionCauses.some(x => input.indexOf(x) !== -1);
}

export function isExceptionRow(punch: string, ClockClientLunchID: any, exceptionStyle: string,
     tooltip: string, exceptions: string, idx: any, classUsed: string): string {
    const normalizedPunch = normalizeString(punch);
    const approved = normalizeString('approved');
    const arrived = normalizeString('arrived');
    const tardy = normalizeString('tardy');
    const exception = normalizeString('exception');
    const location = normalizeString('location');
    const normalizedTooltip = normalizeString(tooltip);
    const classApproved  = normalizeString(classUsed).indexOf(approved) !== -1;
    const canBeInSuccess = ((normalizeString(exceptions).indexOf(arrived) !== -1 ||
    normalizeString(exceptions).indexOf(tardy) !== -1 || normalizeString(classUsed).indexOf(exception) !== -1)
    || classApproved) && idx == 1;
    const canBeOutSuccess = (normalizeString(classUsed).indexOf(exception) !== -1) || classApproved
        && idx % 2 == 0;
    
    const isInExceptionRow = isException(normalizedPunch) || isException(normalizedTooltip) || canBeInSuccess;
    const isOutExceptionRow = isException(normalizedPunch) || isException(normalizedTooltip) || canBeOutSuccess;
    const isLocationException = normalizeString(exceptions).indexOf(location) !== -1;

    const hasLunch = ClockClientLunchID != null;

    const isApproved = normalizeString(exceptionStyle).indexOf(approved) !== -1;

    if (isApproved && (canBeInSuccess || canBeOutSuccess)) {
        return 'text-success';
    }  else if (isInExceptionRow) {
        return 'text-danger';
    } else if (isOutExceptionRow) {
        return 'text-danger';
    } else if (hasLunch) {
        return 'text-info';
    } else if (isLocationException) {
        return 'text-danger';
    } else {
        return null;
    }
}
