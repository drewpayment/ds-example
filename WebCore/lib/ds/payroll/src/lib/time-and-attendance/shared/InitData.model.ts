import { PayPeriod } from './PayPeriod.model';
import { ApproveHourOption } from './ApproveHourOption.model';
import { ClockFilter } from './ClockFilter.model';
import { ApproveHourSettings } from './ApproveHourSettings.model';
import { JobCostingInfo } from './JobCostingInfo.model';

export interface InitData {
    clockPayrollList?: PayPeriod[];
    clockFilterCategory?: ClockFilter[];
    clockEmployeeApproveHoursSettings?: ApproveHourSettings[];
    clockEmployeeApproveHoursOptions?: ApproveHourOption[];
    clientJobCostingInfoResults?: JobCostingInfo;
    clientNotes?: GetClockClientNoteListResultDto[];
}

export interface MappedInitData extends InitData {

    clockFilterCategory1?: ClockFilter[];
    clockFilterCategory2?: ClockFilter[];
}

export interface GetClockClientNoteListResultDto {
    note: string;
    clockClientNoteID: number;
    isActive: boolean;
}
