import { PunchOptionType, DayOfWeek } from '../enums';
import { Moment } from 'moment';

export interface EmployeeClockConfiguration {
    clientId: number;
    employeeId: number;
    employeeNumber: string;
    firstName: string;
    lastName: string;
    middleInitial: string;
    homeDivisionId?: number;
    homeDepartmentId?: number;

    /** RELATIONSHIPS */
    clockEmployee?: EmployeeClockSetupConfiguration;
}

export interface EmployeeClockSetupConfiguration {
    badgeNumber: string;
    timePolicy: EmployeeTimePolicyConfiguration;
    punches: EmployeeRecentPunchInfo[];
}

export interface EmployeeTimePolicyConfiguration {
    timePolicyId: number;
    name: string;

    /** RELATIONSHIPS */
    rules?: EmployeeTimePolicyRuleConfiguration;
    lunches?: EmployeeTimePolicyLunchConfiguration[];
}

export interface EmployeeTimePolicyRuleConfiguration {
    ruleId: number;
    name: string;
    punchOption?: PunchOptionType;
    allowInputPunches: boolean;
    isHideCostCenter: boolean;
    isHideDepartment: boolean;
    isHideEmployeeNotes: boolean;
    isHideJobCosting: boolean;
    isHidePunchType: boolean;
    isHideShift: boolean;
    startDayOfWeek: DayOfWeek;
}

export interface EmployeeTimePolicyLunchConfiguration {
    clockClientLunchId?: number;
    name: string;
    clientCostCenterId?: number;
    isSunday: boolean;
    isMonday: boolean;
    isTuesday: boolean;
    isWednesday: boolean;
    isThursday: boolean;
    isFriday: boolean;
    isSaturday: boolean;
    startTime: Moment | Date | string;
    stopTime: Moment | Date | string;
}

export interface EmployeeRecentPunchInfo {
    clockEmployeePunchId: number;
    modifiedPunch: Moment | Date | string;
    rawPunch: Moment | Date | string;
    shiftDate: Moment | Date | string;
    clientCostCenterId: number | null;
    clientDepartmentId: number | null;
    clientDivisionId: number | null;
    clockClientLunchId: number | null;
    transferOption: number | null;
    clientJobCostingAssignmentId1: number | null;
    clientJobCostingAssignmentId2: number | null;
    clientJobCostingAssignmentId3: number | null;
    clientJobCostingAssignmentId4: number | null;
    clientJobCostingAssignmentId5: number | null;
    clientJobCostingAssignmentId6: number | null;
}
