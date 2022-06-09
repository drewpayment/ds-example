import { IPayrollRequestItem } from '@ds/performance/performance-manager/shared/payroll-request-item.model';
import { EmployeeFilter } from './employee-filter.model';
import { Moment } from 'moment';

export interface EmpSectionSortItem {
    firstName: string;
    lastName: string;
    number: number;
    item: EmpRequestSection;
  }
  
export interface PayrollRequestReportData {
    headerData: PayrollRequestHeaderData;
    empSections: EmpRequestSection[];
    isScoringEnabled: boolean;
  }

  export interface PayrollRequestHeaderData{
    reviewTemplateName: string;
    noEmps: number;
    monthlyCostFrom: number;
    monthlyCostTo: number;
    noMeritIncreasesAwarded: number;
    noHourlyEmployeesMerit: number;
    noSalaryEmployeesMerit: number;
    noHourlyEmployeesBonus: number;
    noSalaryEmployeesBonus: number;
    additionalEarningsPayout: number;
    additionalEarningsAwarded: number;
    startDate: string | Date | Moment;
    endDate: string | Date | Moment;
    filtersApplied: EmployeeFilter[]
  }
  
export interface EmpRequestSection {
    empNumber: string;
    empFirstName: string;
    empLastName: string;
    jobTitle: string;
    department: string;
    division: string;
    meritRecommendation: number;
    score: number;
    meritRequestItems: IPayrollRequestItem[];
    oneTimeItems: IPayrollRequestItem[];
  }

  export const meritIncreaseRequestType = 1;
 export const bonusIncreaseRequestType = 2;