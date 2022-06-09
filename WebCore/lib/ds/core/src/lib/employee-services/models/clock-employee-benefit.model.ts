import { Moment } from 'moment';

export interface ClockEmployeeBenefit {
    clockEmployeeBenefitId: number;
    employeeId: number;
    clientEarningId: number;
    hours: number | null;
    eventDate: Moment | Date | string;
    clientCostCenterId?: number | null;
    clientDivisionId?: number | null;
    clientDepartmentId?: number | null;
    clientShiftId?: number | null;
    isApproved?: boolean;
    comment?: string;
    clockClientHolidayDetailId?: number | null;
    isWorkedHours: boolean;
    requestTimeOffDetailId?: number | null;
    clientId: number;
    subcheck?: string;
    clientJobCostingAssignmentId1?: number | null;
    clientJobCostingAssignmentId2?: number | null;
    clientJobCostingAssignmentId3?: number | null;
    clientJobCostingAssignmentId4?: number | null;
    clientJobCostingAssignmentId5?: number | null;
    clientJobCostingAssignmentId6?: number | null;
    employeeComment?: string;
    employeeClientRateId?: number | null;
    employeeBenefitPay?: number | null;
    approvedBy?: number | null;
}
