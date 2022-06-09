import { Injectable } from '@angular/core';
import { IPayrollRequestItem } from '../../shared/payroll-request-item.model';
import { EmpRequestSection, EmpSectionSortItem, meritIncreaseRequestType as MeritIncreaseRequestType, bonusIncreaseRequestType as BonusIncreaseRequestType } from './shared/report-display-data.model';
import { ApprovalStatus } from '@ds/performance/evaluations/shared/approval-status.enum';
import { Maybe } from '@ds/core/shared/Maybe';
import { IPayrollRequest } from '../../shared/payroll-request.model';
import { ClientSideFilters } from './payroll-request-report-args.model';
import { PayrollRequestFilter } from './payroll-request-filter.service';
import { MonthlyCostCalculator, SalaryStrategy, HourlyStrategy } from './monthly-cost-calculator.service';

export class PayrollRequestToEmpSectionConverterService {

  private readonly HourlyCalculator: MonthlyCostCalculator = new HourlyStrategy();
  private readonly SalaryCalculator: MonthlyCostCalculator = new SalaryStrategy();

  ConvertPayrollRequestsToEmpSections(items: Maybe<IPayrollRequest>, filterService: PayrollRequestFilter): Maybe<EmpRequestSection[]> {
    const filtered = this.FilterItems(items, filterService);
    const grouped = this.GroupByEmployeeNumber(filtered);
    const sections = this.BuildSections(grouped);
    const result = this.SortSections(sections);
    

    return result;
  }

  private FilterItems(items: Maybe<IPayrollRequest>, filterService: PayrollRequestFilter): Maybe<IPayrollRequestItem[]> {
    return filterService.filter(items)
    .map(x => x.filter(x => x.approvalStatusId === ApprovalStatus.Approved || x.approvalStatusId === ApprovalStatus.Pending));
  }

  private BuildSections(grouped: Maybe<{[id:string]:IPayrollRequestItem[]}>): Maybe<EmpRequestSection[]> {

    return grouped.map(x => Object.values(x).map<EmpRequestSection>(empGroup => {
      const section = empGroup.reduce<EmpRequestSection>((prev, curr) => {
        prev.empNumber = curr.employeeNumber;
        prev.empFirstName = curr.employeeFirstName;
        prev.empLastName = curr.employeeLastName;
        prev.jobTitle = curr.employeeJobTitle;
        prev.department = curr.department;
        prev.division = curr.division;
        prev.meritRecommendation = curr.percent;
        prev.score = curr.score;

        if(curr.requestType === MeritIncreaseRequestType){
          prev.meritRequestItems.push(curr);
        }

        if(curr.requestType === BonusIncreaseRequestType){
          prev.oneTimeItems.push(curr);
        }
        return prev;
      }, {meritRequestItems: [], oneTimeItems: []} as EmpRequestSection);
      return section;
    }));

  }

  /**
   * Sorts by the emp last name, then first name, then by emp number
   */
  private SortSections(sections: Maybe<EmpRequestSection[]>): Maybe<EmpRequestSection[]> {
    return sections.map(items => items
    .map(x => this.convertToSortItem(x))
    .sort((a, b) => this.SortItems(a, b))
    .map((val => val.item)));
  }

  private convertToSortItem(section: EmpRequestSection): EmpSectionSortItem {
    return {
      firstName: this.normalizeString(section.empFirstName),
      lastName: this.normalizeString(section.empLastName),
      number: +section.empNumber,
      item: section
    }
  }

  private SortItems(first: EmpSectionSortItem, second: EmpSectionSortItem): number {
    return first.lastName.localeCompare(second.lastName) ||
    first.firstName.localeCompare(second.firstName) || 
    first.number - second.number;
  }

  private GroupByEmployeeNumber(items: Maybe<IPayrollRequestItem[]>) : Maybe<{[id:string]:IPayrollRequestItem[]}> {
    return items.map(x => x.reduce<{[id:string]:IPayrollRequestItem[]}>((prev, curr) => {
      if(prev[curr.employeeNumber] !== undefined){
        prev[curr.employeeNumber].push(curr);
      } else {
        prev[curr.employeeNumber] = [curr];
      }
      return prev;
    }, {}));
  }

  private normalizeString(s: string): string {
    if(s != null){
      s = s.trim().toLowerCase();
    }
    return s;
  }

  public findMonthyCost(empSections: Maybe<EmpRequestSection[]>): Maybe<PayoutData> {

    const startVal: PayoutData = {
      from: 0,
      to: 0
    }

    return empSections.map(x => x.map(section => section.meritRequestItems).reduce((a, b) => a.concat(b), []).reduce((result, nextItem) => {

      const hourlyResult = this.CalculatePayouts(nextItem, this.HourlyCalculator);
      const salaryResult = this.CalculatePayouts(nextItem, this.SalaryCalculator);

      result.from += hourlyResult.from + salaryResult.from;
      result.to += hourlyResult.to + salaryResult.to;

      return result;
    }, startVal))
  }

  public CalculatePayouts(item: IPayrollRequestItem, service: MonthlyCostCalculator): PayoutData {
    const result: PayoutData = {
      from: 0,
      to: 0
    }

    if (service.DoesCalcApply(item)) {

      result.from += service.ApplyCalculationFrom(item)
      result.to += service.ApplyCalculationTo(item);

    }

    return result;
  }
}

interface PayoutData {
  from: number;
  to: number;
}