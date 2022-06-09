import { ClientEarning } from "@ds/core/employee-services/models";
// import { EmployeePayType } from "@ds/core/employee-services/models/employee-pay-type.model";
import { EmployeePayType } from "./employee-pay-type";
import { User } from "@ds/core/employee-services/models/user.model";
import { AccrualBalanceOption } from "./accrual-balance-option";
import { AccrualCarryOverOption } from "./client-accrual-carry-over-option";
import { ClientAccrualEmployeeStatus } from "./client-accrual-employee-status";
import { ServiceBeforeAfter } from "./service-before-after";
import { ServiceCarryOverFrequency } from "./service-carry-over-frequency";
import { ServiceCarryOverTillFrequency } from "./service-carry-over-till-frequency";
import { ServiceCarryOverWhenFrequency } from "./service-carry-over-when-frequency";
import { ServiceFrequency } from "./service-frequency";
import { ServicePlanType } from "./service-plan-type";
import { ServiceReferencePointFrequency } from "./service-reference-point-frequency";
import { ServiceRenewFrequency } from "./service-renew-frequency";
import { ServiceRewardFrequency } from "./service-reward-frequency";
import { ServiceStartEndFrequency } from "./service-start-end-frequency";
import { ServiceUnit } from "./service-unit";
import { AutoApplyAccrualPolicy } from "./auto-apply-accrual-policy";
import { ServiceType } from "./service-type";
import { IPayrollHistoryInfo } from "@ds/payroll/shared";


export class ClientAccrualDropdownsDto {
    serviceFrequencies: ServiceFrequency[];
    serviceRewardFrequencies: ServiceRewardFrequency[];
    serviceRenewFrequencies: ServiceRenewFrequency[];
    serviceCarryOverFrequencies: ServiceCarryOverFrequency[];
    serviceCarryOverTillFrequencies: ServiceCarryOverTillFrequency[];
    serviceCarryOverWhenFrequencies: ServiceCarryOverWhenFrequency[];
    serviceReferencePointFrequencies: ServiceReferencePointFrequency[];
    serviceStartEndFrequencies: ServiceStartEndFrequency[];
    servicePlanTypes: ServicePlanType[];
    serviceTypes: ServiceType[];
    serviceUnits: ServiceUnit[];
    accrualBalanceOptions: AccrualBalanceOption[];
    clientAccrualEmployeeStatuses: ClientAccrualEmployeeStatus[];
    clientEarnings: ClientEarning[];
    clientAccrualCarryOverOptions: AccrualCarryOverOption[];
    clientAccrualClearOptions: AccrualCarryOverOption[];
    employeePayTypes: EmployeePayType[];
    serviceBeforeAfters: ServiceBeforeAfter[];
    companyAdmins: User[];
    autoApplyAccrualPolicyOptions: AutoApplyAccrualPolicy[];
}

export interface IClientCalendar {
    calendarFrequencyAltWeekId: boolean;
    calendarFrequencyBiWeeklyId: boolean;
    calendarFrequencyMonthlyId: boolean;
    calendarFrequencyQuarterlyId: boolean;
    calendarFrequencySemiMonthlyId: boolean;
    calendarFrequencyWeeklyId: boolean;
}
