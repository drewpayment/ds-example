import { AccrualBalanceOption } from 'apps/ds-company/src/app/models/leave-management/accrual-balance-option';
import { ClientAccrualConstants } from 'apps/ds-company/src/app/models/leave-management/client-accrual-constants';
import { Moment } from 'moment';
import { ClientEarning } from '.';
import { ClientAccrualEarning } from './client-accrual-earning.model';
import { ClientAccrualFirstYearSchedule } from './client-accrual-first-year-schedule.model';
import { ClientAccrualSchedule } from './client-accrual-schedule.model';
import { EmployeeAccrual } from './employee-accrual.model';

export interface IClientAccrual {
    clientAccrualId: number;
    clientId: number;
    clientEarningId: number;
    description: string;
    employeeStatusId: number;
    employeeTypeId: number;
    isShowOnStub: boolean;
    accrualBalanceOptionId: number;
    serviceReferencePointId: number;
    planType: number;
    units: number;
    isShowPreviewMassages: boolean;
    notes: string;
    beforeAfterId: number;
    beforeAfterDate: Date | Moment | string;
    carryOverToId: number;
    isUseCheckDates: boolean;
    isLeaveManagment: boolean;
    leaveManagmentAdministrator?: number;
    requestMinimum: number;
    requestMaximum: number;
    hoursInDay: number;
    atmInterfaceCode: string;
    isPeriodEnd: boolean;
    isPeriodEndPlusOne: boolean;
    showAccrualBalanceStartingFrom: Date | Moment | string;
    isAccrueWhenPaid: boolean;
    accrualCarryOverOptionId: number;
    isEmailSupervisorRequests: boolean;
    isCombineByEarnings: boolean;
    isLeaveManagementUseBalanceOption: boolean;
    isRealTimeAccruals: boolean;
    isActive: boolean;
    accrualClearOptionId: number;
    isStopLastPay: boolean;
    carryOverToBalanceLimit: boolean;
    requestIncrement: number;
    policyLink: string;
    projectAmount: number;
    projectAmountType: number;
    isEmpEmailRequest: boolean;
    isEmpEmailApproval: boolean;
    isEmpEmailDecline: boolean;
    isSupEmailRequest: boolean;
    isSupEmailApproval: boolean;
    isSupEmailDecline: boolean;
    isCaEmailRequest: boolean;
    isCaEmailApproval: boolean;
    isCaEmailDecline: boolean;
    isLmaEmailRequest: boolean;
    isLmaEmailApproval: boolean;
    isLmaEmailDecline: boolean;
    balanceOptionAmount: number;
    lmMinAllowedBlanace: number;
    isDisplay4Decimals: boolean;
    allowAllDays: boolean;
    proratedWhenToAwardTypeId?: number;
    proratedServiceReferencePointOverrideId?: number;

    //optional
    waitingPeriodValue?: number;
    waitingPeriodTypeId?: number;
    waitingPeriodReferencePoint?: number;
    isPaidLeaveAct?: boolean;
    hoursPerWeekAct?: number;
    allowHoursRollOver?: boolean;
    modified?: Date | Moment | string;
    modifiedBy?: string;
    clientEarning?: ClientEarning[];
    clientAccrualEarnings?: ClientAccrualEarning[];
    clientAccrualSchedules?: ClientAccrualSchedule[];
    clientAccrualProratedSchedules?: ClientAccrualFirstYearSchedule[];
    // employeeAccruals?: EmployeeAccrual[];
    accrualBalanceOption?: AccrualBalanceOption[];
    autoApplyAccrualPolicyOptionId?: number;

}


export class ClientAccrual implements IClientAccrual {
  constructor(
    public clientAccrualId: number = null,
    public clientId: number = null,
    public clientEarningId: number = null,
    public description: string = null,
    public employeeStatusId: number = null,
    public employeeTypeId: number = null,
    public isShowOnStub: boolean = null,
    public accrualBalanceOptionId: number = null,
    public serviceReferencePointId: number = null,
    public planType: number = null,
    public units: number = null,
    public isShowPreviewMassages: boolean = null,
    public notes: string = null,
    public beforeAfterId: number = null,
    public beforeAfterDate: Date | Moment | string = null,
    public carryOverToId: number = null,
    public isUseCheckDates: boolean = null,
    public isLeaveManagment: boolean = null,
    public leaveManagmentAdministrator?: number,
    public requestMinimum: number = null,
    public requestMaximum: number = null,
    public hoursInDay: number = null,
    public atmInterfaceCode: string = null,
    public isPeriodEnd: boolean = null,
    public isPeriodEndPlusOne: boolean = null,
    public showAccrualBalanceStartingFrom: Date | Moment | string = null,
    public isAccrueWhenPaid: boolean = null,
    public accrualCarryOverOptionId: number = null,
    public isEmailSupervisorRequests: boolean = null,
    public isCombineByEarnings: boolean = null,
    public isLeaveManagementUseBalanceOption: boolean = null,
    public isRealTimeAccruals: boolean = null,
    public isActive: boolean = null,
    public accrualClearOptionId: number = null,
    public isStopLastPay: boolean = null,
    public carryOverToBalanceLimit: boolean = null,
    public requestIncrement: number = null,
    public policyLink: string = null,
    public projectAmount: number = null,
    public projectAmountType: number = null,
    public isEmpEmailRequest: boolean = null,
    public isEmpEmailApproval: boolean = null,
    public isEmpEmailDecline: boolean = null,
    public isSupEmailRequest: boolean = null,
    public isSupEmailApproval: boolean = null,
    public isSupEmailDecline: boolean = null,
    public isCaEmailRequest: boolean = null,
    public isCaEmailApproval: boolean = null,
    public isCaEmailDecline: boolean = null,
    public isLmaEmailRequest: boolean = null,
    public isLmaEmailApproval: boolean = null,
    public isLmaEmailDecline: boolean = null,
    public balanceOptionAmount: number = null,
    public lmMinAllowedBlanace: number = null,
    public isDisplay4Decimals: boolean = null,
    public allowAllDays: boolean = null,
    public proratedWhenToAwardTypeId?: number,
    public proratedServiceReferencePointOverrideId?: number,

    //optional
    public waitingPeriodValue?: number,
    public waitingPeriodTypeId?: number,
    public waitingPeriodReferencePoint?: number,
    public isPaidLeaveAct?: boolean,
    public hoursPerWeekAct?: number,
    public allowHoursRollOver?: boolean,
    public modified?: Date | Moment | string,
    public modifiedBy?: string,
    public clientEarning?: ClientEarning[],
    public clientAccrualEarnings?: ClientAccrualEarning[],
    public clientAccrualSchedules?: ClientAccrualSchedule[],
    public clientAccrualProratedSchedules?: ClientAccrualFirstYearSchedule[],
    // employeeAccruals?: EmployeeAccrual[],
    public accrualBalanceOption?: AccrualBalanceOption[],
    public autoApplyAccrualPolicyOptionId?: number,
  ) {}
}
