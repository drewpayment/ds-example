import { Moment } from 'moment';
import { PunchOptionType } from '../enums';
import { ClockEmployeePayPeriodEnded, EmployeeClockConfiguration } from '.';


export interface CheckPunchTypeResult {
    lastPunchTime?: Moment | Date | string;
    isFirstPunchOfDay: boolean;
    isOutPunch: boolean;
    shouldDisablePunchTypeSelection: boolean;
    shouldHideCostCenter: boolean;
    isCostCenterSelectionRequired: boolean;
    shouldHideDepartment: boolean;
    shouldHideJobCosting: boolean;
    shouldHideEmployeeNotes: boolean;
    costCenterId?: number;
    lastOutCostCenterId?: number;
    lunchCostCenterId?: number;
    departmentId?: number;
    divisionId?: number;
    punchTypeId?: PunchOptionType;
    homeCostCenterId?: number;
    homeDepartmentId?: number;
    clientJobCostingAssignmentId1?: number;
    clientJobCostingAssignmentId2?: number;
    clientJobCostingAssignmentId3?: number;
    clientJobCostingAssignmentId4?: number;
    clientJobCostingAssignmentId5?: number;
    clientJobCostingAssignmentId6?: number;
    payPeriodEnded: ClockEmployeePayPeriodEnded;
    ipAddress: string;
    canPunchFromIp: boolean | null;
    allowInputPunches: boolean;
    hasMobilePunching: boolean;
    punchOption: PunchOptionType;
    employeeClockConfiguration: EmployeeClockConfiguration;
}
