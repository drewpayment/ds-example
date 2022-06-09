import { List } from 'lodash';

export interface OvertimeSetup {
    employeeId: number;
    clockClientTimePolicyId: number;
    clockClientOvertimeId: Array<number>;
    clockOvertimeFrequencyId: Array<number>;
}