import { Moment } from "moment";

export interface ClientAccrualSchedule {
    clientAccrualScheduleId: number;
    clientAccrualId: number;
    serviceStart: number;
    serviceStartFrequencyId: number;
    serviceFrequencyId: number;
    serviceEnd: number;
    serviceEndFrequencyId: number;
    accrualBalanceLimit: number;
    balanceLimit: number;
    reward: number;
    serviceRewardFrequencyId: number;
    serviceRenewFrequencyId: number;
    renewEnd: number;
    carryOver: number;
    serviceCarryOverFrequencyId: number;
    serviceCarryOverWhenFrequencyId: number;
    serviceCarryOverTill: number;
    serviceCarryOverTillFrequencyId: number;
    rateCarryOverMax: number;
    modified: Date | Moment | string;
    modifiedBy: number;
}

export enum ProratedWhenToAwardType {
  Days = 1,
  Date
}

export enum ClientAccrualSchedulesEditIconType {
  notEditing = 'edit',
  editing = 'check'
}

export function mapIsEditModeToClientAccrualSchedulesEditIconType(
  isEditMode: boolean
  ): ClientAccrualSchedulesEditIconType {
  return isEditMode
    ? ClientAccrualSchedulesEditIconType.editing
    : ClientAccrualSchedulesEditIconType.notEditing;
}

export enum ServiceStartEndFrequencyType {
  Days = 1,
  Months,
  Years,
  Hours
}

export enum ServiceRewardFrequencyType {
  "Rate Max" = 1,
  Flat,
  "Rate Min"
}

export enum ServiceCarryOverFrequencyType {
  Rate = 1,
  Flat,
  "Balance Limit"
}

export enum ServiceCarryOverWhenFrequencyType {
  "Anniversary Year" = 1,
  "Fiscal Year",
  "Calendar Year",
  "First of Month Annv Year",
  "Each Payroll"
}

export enum ServiceCarryOverUntilFrequencyType {
  Days = 1,
  Months
}

export enum ServiceFrequencyType {
  "One Time" = 1,
  "Completed" = 2,
  "Up To" = 3,
  "First Payroll" = 4,
  "LifeTime" = 5,
}
