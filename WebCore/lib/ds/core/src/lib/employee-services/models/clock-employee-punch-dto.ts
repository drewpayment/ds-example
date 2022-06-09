import { Moment } from 'moment';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { ClockExceptionEnum } from '@ajs/labor/models/client-exception-detail.model';

export interface ClockEmployeePunchDto {
    clockEmployeePunchId: number;
    employeeId: number;
    employeeFirstName: string;
    employeeLastName: string;
    modifiedPunch: Moment|Date|string;
    rawPunch: Moment|Date|string;
    clientCostCenterId?: number;
    clientDivisionId?: number;
    clientDepartmentId?: number;
    clientShiftId?: number;
    rawPunchBy: number;
    clockEmployeePunchTypeId?: number;
    clockClientLunchId?: number;
    isPaid: boolean;
    comment: string;
    shiftDate?: Moment|Date|string;
    isManualShiftOverride?: boolean;
    timeZoneId?: number;
    clockName: string;
    transferOption?: number;
    isStopAutoLunch: boolean;
    clientId: number;
    clockEmployeeScheduleId?: number;
    scheduleNumber?: number;
    clientJobCostingAssignmentId1?: number;
    clientJobCostingAssignmentId2?: number;
    clientJobCostingAssignmentId3?: number;
    clientJobCostingAssignmentId4?: number;
    clientJobCostingAssignmentId5?: number;
    clientJobCostingAssignmentId6?: number;
    employeeComment: string;
    errorType: ClockExceptionEnum | null;
    // compare(that: ClockEmployeePunchDto): number;
    clockEmployeePunchLocationLat?: number;
    clockEmployeePunchLocationLng?: number;
    clockEmployeePunchLocationId: number | null;
    isChecked: boolean | null;
    isSelectable?: boolean;
}

export class ClockEmployeePunchDtoImpl implements ClockEmployeePunchDto {
    clockEmployeePunchId: number;
    employeeId: number;
    employeeFirstName: string;
    employeeLastName: string;
    modifiedPunch: Moment|Date|string;
    rawPunch: Moment|Date|string;
    clientCostCenterId?: number;
    clientDivisionId?: number;
    clientDepartmentId?: number;
    clientShiftId?: number;
    rawPunchBy: number;
    clockEmployeePunchTypeId?: number;
    clockClientLunchId?: number;
    isPaid: boolean;
    comment: string;
    shiftDate?: Moment|Date|string;
    isManualShiftOverride?: boolean;
    timeZoneId?: number;
    clockName: string;
    transferOption?: number;
    isStopAutoLunch: boolean;
    clientId: number;
    clockEmployeeScheduleId?: number;
    scheduleNumber?: number;
    clientJobCostingAssignmentId1?: number;
    clientJobCostingAssignmentId2?: number;
    clientJobCostingAssignmentId3?: number;
    clientJobCostingAssignmentId4?: number;
    clientJobCostingAssignmentId5?: number;
    clientJobCostingAssignmentId6?: number;
    employeeComment: string;
    errorType: ClockExceptionEnum | null;
    clockEmployeePunchLocationLat?: number;
    clockEmployeePunchLocationLng?: number;
    clockEmployeePunchLocationId: number | null;
    isChecked: boolean | null;
    isSelectable?: boolean;

    constructor(that: ClockEmployeePunchDto) {
        Object.getOwnPropertyNames(that).forEach(property => {
            this[property] = that[property];
        });
    }

    static punchExists(punch: ClockEmployeePunchDto): boolean {
      return typeof(punch) !== 'undefined';
    }

    static coalesceShiftDate(p: ClockEmployeePunchDto): Moment|Date|string {
        // Backend uses:
        // FilterBy(x => (x.ShiftDate ?? x.ModifiedPunch) >= startDate && (x.ShiftDate ?? x.ModifiedPunch) <= endDate);
        return p.shiftDate ? p.shiftDate : p.modifiedPunch;
    }

    // static comparePunches(a: Moment, b: Moment): number {
    static comparePunches(a: Moment|Date|string, b: Moment|Date|string): number {
        const aMoment = convertToMoment(a);
        const bMoment = convertToMoment(b);
        let result = 0;

        if (aMoment.isBefore(bMoment)) {
            result = -1;
        } else if (!aMoment.isSame(bMoment)) {
            result = 1;
        }

        return result;
    }

    compare(that: ClockEmployeePunchDto): number {
        return ClockEmployeePunchDtoImpl.comparePunches(this.rawPunch, that.rawPunch);
    }
}
