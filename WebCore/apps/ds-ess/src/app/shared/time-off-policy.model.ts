import { Moment } from 'moment';
import { TimeOffStatusType } from '@ds/core/employee-services/models';

export interface TimeOffPolicy {
    policyId: number;
    policyName: string;
    unitsAvailable: number;
    pendingUnits: number;
    pendingRequest: number;
    nextAwardDate: Date | string | Moment;
    activity: TimeOffEvent[];
    manager: any;
    startingUnits: number;
    startingUnitsAsOf: Date | string | Moment;
    timeOffUnitType: number;
    display4Decimals: boolean;
    unitsPerDay: number;
}

export interface TimeOffEvent {
    amount: number;
    balanceAfter: number;
    endDate: Date | string | Moment;
    startDate: Date | string | Moment;
    requestDate: Date | string | Moment;
    requestTimeOffId: number;
    timeOffStatus: TimeOffStatusType;
    timeOffAward: number;
    policy: any;
    manager: any;
    notes: any;
}
