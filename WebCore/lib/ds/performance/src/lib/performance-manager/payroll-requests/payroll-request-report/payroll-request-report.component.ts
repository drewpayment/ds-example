import { Component, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { PayrollRequestReportArgsStore } from './payroll-request-report-args.store';
import { PayrollRequestService } from '../payroll-request.service';
import { switchMap, map, tap, catchError } from 'rxjs/operators';
import { Observable, forkJoin, of, throwError } from 'rxjs';
import { IPayrollRequest } from '../../shared/payroll-request.model';
import { PayrollRequestToEmpSectionConverterService } from './payroll-request-to-emp-section-converter.service';
import { PayrollRequestReportData, EmpRequestSection } from './shared/report-display-data.model';
import { PayTypeEnum } from '@ajs/employee/hiring/shared/models';
import { ApprovalStatus } from '@ds/performance/evaluations/shared/approval-status.enum';
import { Maybe } from '@ds/core/shared/Maybe';
import { MonthlyCostCalculator, HourlyStrategy, SalaryStrategy } from './monthly-cost-calculator.service';
import { IPayrollRequestItem } from '../../shared/payroll-request-item.model';
import { ClientSideFilters } from './payroll-request-report-args.model';
import { PayrollRequestFilterFactory } from './payroll-request-filter.service';
import { IReviewStatusSearchOptions } from '../../shared/review-search-options.model';
import { EmployeeApiService } from '@ds/core/employees';
import { IEmployeeSearchFilter } from '@ds/employees/search/shared/models/employee-search-filter';
import { EmployeeFilter } from './shared/employee-filter.model';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';

@Component({
  selector: 'ds-payroll-request-report',
  templateUrl: './payroll-request-report.component.html',
  styleUrls: ['./payroll-request-report.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PayrollRequestReportComponent {



readonly data$: Observable<PayrollRequestReportData>;

  constructor(argsStore: PayrollRequestReportArgsStore,
    payrollRequestSvc: PayrollRequestService,
    changeTrackerSvc: ChangeDetectorRef,
    private employeeService:EmployeeApiService,
    private msg: DsMsgService,
    perfSvc: PerformanceReviewsService) {

      this.data$ = argsStore.reportParams$.pipe(switchMap(data => {
        return forkJoin(
        payrollRequestSvc.getPayrollRequests(data.searchOptions), 
        of(data.clientSideFilters), 
        of(data.searchOptions), 
        employeeService.getEmployeeSearchFilters(),
        perfSvc.isScoringEnabledForReviewTemplate(data.searchOptions.reviewTemplateId));
      }),
      map(data => this.mapper(new Maybe(data[0]), data[1], data[2], data[3], new Maybe(data[4]).map(x => x.data).valueOr(false))),
      tap(() => {
        setTimeout(() => changeTrackerSvc.detach()); // view doesn't change after first emission, no need for change detection
        setTimeout(() => window.print(), 1000)
      }),
      catchError(err => {
        msg.showErrorMsg('Failed to retrieve report data.');
        return of({} as PayrollRequestReportData);
      }))
   }

  mapper(
    request: Maybe<IPayrollRequest>, 
    clientSideFilters: ClientSideFilters, 
    searchOptions: IReviewStatusSearchOptions, 
    filterInfo: IEmployeeSearchFilter[],
    isScoringEnabled: boolean): PayrollRequestReportData {
    const filterFactory = new PayrollRequestFilterFactory();
    const payrollRequestConverter = new PayrollRequestToEmpSectionConverterService();
    const filter = filterFactory.getFilter(clientSideFilters);

    const empSections = payrollRequestConverter.ConvertPayrollRequestsToEmpSections(request, filter);

    const monthlyCost = payrollRequestConverter.findMonthyCost(empSections);

    return {
      headerData: {
        startDate: searchOptions.startDate,
        endDate: searchOptions.endDate,
        filtersApplied: this.convertFiltersIntoDisplayableFilter(searchOptions, filterInfo),
        reviewTemplateName: request.map(x => x.reviewTemplateName).value(),
        noEmps: empSections.map(x => x.length).value(),
        monthlyCostFrom: monthlyCost.map(x => x.from).value(),
        monthlyCostTo: monthlyCost.map(x => x.to).value(),
        noMeritIncreasesAwarded: this.findMeritsAwarded(empSections).value(),
        noHourlyEmployeesMerit: this.findHourlyEmployeesWithMerit(empSections).value(),
        noSalaryEmployeesMerit: this.findSalaryEmployeesWithMerit(empSections).value(),
        noHourlyEmployeesBonus: this.findHourlyEmployeesWithBonus(empSections).value(),
        noSalaryEmployeesBonus: this.findSalaryEmployeesWithBonus(empSections).value(),
        additionalEarningsPayout: this.findAdditionalEarningPayout(empSections).value(),
        additionalEarningsAwarded: this.findBonusesAwarded(empSections).value(),
      },
      empSections: empSections.value(),
      isScoringEnabled: isScoringEnabled
    }
  }

  private convertFiltersIntoDisplayableFilter(searchOptions: IReviewStatusSearchOptions, employeeSearchFilterItems: IEmployeeSearchFilter[]): EmployeeFilter[] {
    const result: EmployeeFilter[] = [];
    this.addFilter(result, searchOptions.isActiveOnly, "Active Only");
    this.addFilter(result, searchOptions.isExcludeTemps, "Temps Excluded");
    this.addFilter(result, !!searchOptions.searchText, "Search Text: " + searchOptions.searchText)
    if(searchOptions.filters){
      const filtered = employeeSearchFilterItems // ported from the manager-header.component.ts ngoninit
      .filter(x => !!searchOptions.filters[x.filterType])
      .map(x => x.filterOptions)
      .reduce((prev, curr) => prev.concat(curr), [])
      .filter(option => +searchOptions.filters[option.filterType] == option.id)
      .map(filter => {
        const newFilter = new EmployeeFilter();
        newFilter.filterName = filter.name;
        return newFilter;
      });
      filtered.forEach(item => {
        result.push(item);
      });

    }
    return result;
  }

  private addFilter(list: EmployeeFilter[], condition: boolean, name: string): EmployeeFilter[] {
    if(condition){
      list.push({
        employeeFilterType: null,
        filterName: name,
        specialFilterType: null
      })
    }
    return list;
  }

  

   private findMeritsAwarded(empSections: Maybe<EmpRequestSection[]>): Maybe<number> {
     return empSections.map(x => x.map(section => section.meritRequestItems).reduce((a, b) => a.concat(b), []).filter(item => item.approvalStatusId === ApprovalStatus.Approved).length);
   }

   private findBonusesAwarded(empSections: Maybe<EmpRequestSection[]>): Maybe<number> {
    return empSections.map(x => x.map(section => section.oneTimeItems).reduce((a, b) => a.concat(b), []).filter(item => item.approvalStatusId === ApprovalStatus.Approved).length)
   }

   private findHourlyEmployeesWithMerit(empSections: Maybe<EmpRequestSection[]>): Maybe<number> {
    return empSections.map(x => x.filter(section => section.meritRequestItems.some(item => item.approvalStatusId === ApprovalStatus.Approved && item.employeePayType === PayTypeEnum.Hourly)).length)
   }

   private findSalaryEmployeesWithMerit(empSections: Maybe<EmpRequestSection[]>): Maybe<number> {
    return empSections.map(x => x.filter(section => section.meritRequestItems.some(item => item.approvalStatusId === ApprovalStatus.Approved && item.employeePayType === PayTypeEnum.Salary)).length)
   }

   private findHourlyEmployeesWithBonus(empSections: Maybe<EmpRequestSection[]>): Maybe<number> {
    return empSections.map(x => x.filter(section => section.oneTimeItems.some(item => item.approvalStatusId === ApprovalStatus.Approved && item.employeePayType === PayTypeEnum.Hourly)).length)
   }

   private findSalaryEmployeesWithBonus(empSections: Maybe<EmpRequestSection[]>): Maybe<number> {
    return empSections.map(x => x.filter(section => section.oneTimeItems.some(item => item.approvalStatusId === ApprovalStatus.Approved && item.employeePayType === PayTypeEnum.Salary)).length)
   }

   private findAdditionalEarningPayout(empSections: Maybe<EmpRequestSection[]>): Maybe<number> {
     return empSections.map(x => x.map(section => section.oneTimeItems).reduce((a, b) => a.concat(b), []).reduce((a, b) => a + b.payoutTo, 0));
   }


}

