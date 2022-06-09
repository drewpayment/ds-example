import { ClientEarningCategory } from '../enums';
import { IClientAccrual } from './client-accrual.model';
import { ClientEarningCategoryDto } from './client-earning-category.model';

export interface ClientEarning {
    clientEarningId:number;
    clientId:number;
    description:string;
    code:string;
    percent:number;
    isShowOnStub:boolean;
    isShowYtdHours:boolean;
    isShowYtdDollars:boolean;
    calcShiftPremium:number;
    isTips:boolean;
    isDefault:boolean;
    earningCategoryId:ClientEarningCategory;
    earningCategory:ClientEarningCategoryDto|null;
    isIncludeInDeductions:boolean;
    isEic:boolean;
    additionalAmount:number|null;
    isIncludeInOvertimeCalcs:boolean;
    isActive:boolean;
    isBlockFromTimeClock:boolean;
    isIncludeInAvgRate:boolean;
    isShowOnlyIfCurrent:boolean;
    blockedSecurityUser:number;
    isUpMinWage:boolean;
    isShowLifetimeHours:boolean;
    isExcludeHrsInArpCalc:boolean;
    isExcludePayInArpCalc:boolean;
    isServiceChargeTips:boolean;
    isAcaWorkedHours:boolean;
    taxOptionId:number|null;
    isReimburseTaxes:boolean

    //optional
    modified?: Date;
    modifiedBy?: string;
    isBasePay?: boolean;
    emergencyLeave?: number;
    clientAccrual?: IClientAccrual[];
}
