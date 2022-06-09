import { Component, OnInit } from '@angular/core';
import { date, checkboxComponent } from '@ajs/applicantTracking/application/inputComponents';
import { IPayrollHistory } from '@ajs/payroll/history/models';
import { IEmployeeStatus } from '@ajs/employee/models';
import { IContact, IContactSearchResult } from '@ds/core/contacts';
import { zip, forkJoin } from 'rxjs';
import { map, startWith, distinctUntilChanged, filter } from 'rxjs/operators';
import { EmployeeApiService } from '@ds/core/employees/shared/employee-api.service';
import { DsApiCommonProvider } from '@ajs/core/api/ds-api-common.provider';
import * as moment from 'moment';
import { DsPopupService } from '@ajs/ui/popup/ds-popup.service';
import { HttpParams, HttpRequest } from '@angular/common/http';
import { FormControl } from '@angular/forms';
import { ReportsService } from '../../shared/reports.service';
import { IEmployeePayType, IScheduledReportFileFormat, IEmployeeChangeList,
    IEmployeeChangeInfo, IEmployeeChangeParameters } from '../../shared';
import { PayrollService } from '@ds/payroll/shared/payroll.service';
import { IReportParameter, IBasicPayrollHistory, IPayrollRunType } from '@ds/payroll/shared';
import { IEmployeeSearchResultResponseData, IEmployeeSearchResult } from '@ajs/employee/search/shared/models';
import { AccountService } from '@ds/core/account.service';
import { EMPLOYEE_ACTIONS } from '@ds/core/employees';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';


@Component({
  selector: 'ds-report-runner',
  templateUrl: './report-runner.component.html',
  styleUrls: ['./report-runner.component.scss']
})
export class ReportRunnerComponent implements OnInit {
  isLoading = true;
  isDisabled = false;
  isFlitering = false;
  searchResults: IEmployeeSearchResultResponseData;
  employees: IEmployeeSearchResult[];
  filteredEmployees: IEmployeeSearchResult[];
  selectedEmployee: IEmployeeSearchResult;
  selectedEmployeeId: number;
  payrolls: IBasicPayrollHistory[];
  selectedPayroll: IBasicPayrollHistory;
  selectedPayrollId: number;
  selectedStartDate: Date;
  selectedEndDate: Date;
  employeeStatuses: IEmployeeStatus[];
  selectedEmployeeStatus: IEmployeeStatus;
  selectedEmployeeStatusId: number;
  employeePayTypes: IEmployeePayType[];
  selectedPayType: IEmployeePayType;
  payrollRunTypes: IPayrollRunType[];
  selectedPayrollRunType: IPayrollRunType;
  selectedPayTypeId: number;
  fileFormats: IScheduledReportFileFormat[];
  selectedFormat: IScheduledReportFileFormat;
  selectedFormatId: number;
  employeeChanges: IEmployeeChangeList[];
  filteredChanges: IEmployeeChangeList[];
  selectedEmployeeChanges: number;
  filteredEmployeeChanges: number;
  selectedEmployeeChange: IEmployeeChangeInfo;
  selectedEmployeeChangeId: string;
  selectedSequenceId: number;
  checkEverything: boolean = false;
  searchControl                   = new FormControl();
  firstLoad = false;
  hasHourlyAccess: boolean = false;
  hasSalaryAccess: boolean = false;

  constructor(
    private reportApiService: ReportsService,
    private payrollApiService: PayrollService,
    private employeeApiService: EmployeeApiService,
    private DsPopup: DsPopupService,
    private accountSvc: AccountService
  ) { }

  ngOnInit() {
    this.selectedEmployee = null;
    this.selectedEmployeeId = 0;
    this.selectedPayrollId = null;
    this.selectedEmployeeStatusId = 0;
    this.selectedPayTypeId = null;
    this.selectedFormatId = null;
    this.selectedPayroll = <IBasicPayrollHistory>{};
    this.selectedEmployeeStatus = <IEmployeeStatus>{};
    this.selectedPayType = <IEmployeePayType>{};
    this.selectedSequenceId = 1;
    this.checkEverything = false;
    this.loadDropDowns();
    this.selectedStartDate = new Date();
    this.selectedEndDate = new Date();

    this.searchControl.valueChanges.subscribe(search => this.applySearch(search));
  }

  loadDropDowns() {
    forkJoin(
      this.reportApiService.getReportSetupContacts(),
      this.payrollApiService.getBasicPayrollHistory(),
      this.payrollApiService.getPayType(),
      this.employeeApiService.getEmployeeStatusList(),
      this.reportApiService.getFileFormats(),
      this.accountSvc
      .canPerformActions(EMPLOYEE_ACTIONS.Employee.ReadHourlyEmployeeInfo),

      this.accountSvc
      .canPerformActions(EMPLOYEE_ACTIONS.Employee.ReadSalaryEmployeeInfo),
      this.payrollApiService.getPayrollRun()
    )
    .subscribe((results) => {
      this.searchResults = results[0];
      this.employees = this.searchResults.results.filter(item => item.employeeId !== null)
      .sort((v1, v2) => {
        if (v1.lastName === v2.lastName) {
          return v1.firstName > v2.firstName ? 1 : -1;
        }
        return v1.lastName > v2.lastName ? 1 : -1;
      });
      this.employees.unshift(<any>{
        employeeId: 0,
        firstName: 'All',
        lastName: '',
        isActive: true
      });
      this.employees.forEach(item => {
        if (!item.isActive) {
          item.firstName = item.firstName + ' - T'
        }
      });

      this.selectedEmployee = this.employees[0];
      this.payrolls = results[1].sort((v1, v2) => {
        return v1.checkDate < v2.checkDate ? 1 : -1;
      });
      this.employeePayTypes = results[2].sort((v1, v2) => {
        return v1.payTypeId < v2.payTypeId ? 1 : -1;
      });
      this.payrollRunTypes = results[7];
      this.hasHourlyAccess = (results[5] === true);
      this.hasSalaryAccess = (results[6] === true);

      if (!this.hasHourlyAccess) {
        this.selectedPayTypeId = 2;
      } else if (!this.hasSalaryAccess) {
        this.selectedPayTypeId = 1;
      } else {
        this.selectedPayTypeId = 3;
      }

      this.employeeStatuses = results[3].sort((v1, v2) => {
        if (v1.isActive === true && v2.isActive === true) {
          return v1.description > v2.description ? 1 : -1;
        } else if (v1.isActive === false && v2.isActive === false) {
          return v1.description > v2.description ? 1 : -1;
        }
        return v1.isActive < v2.isActive ? 1 : -1;
      });

      // remove unneeded file formats from selections
      results[4].splice(1, 2);
      this.fileFormats = results[4];

      this.selectedFormatId = 1;
      this.isLoading = false;
    });
  }

  payrollChange() {
    this.selectedPayroll = this.payrolls.find(p => p.payrollId === this.selectedPayrollId) || <IBasicPayrollHistory>{};
    if (this.selectedPayroll.periodStart === undefined) {
      this.selectedStartDate = new Date();
      this.selectedEndDate = new Date();

    } else {
      this.selectedStartDate = this.selectedPayroll.periodStart;
      this.selectedEndDate = this.selectedPayroll.periodEnded;
    }
  }

  filterClick() {
    this.isFlitering = true;
    const payStart = moment(this.selectedStartDate).format(DsApiCommonProvider.TimeFormat.DATE_ONLY);
    const payEnd = moment(this.selectedEndDate).format(DsApiCommonProvider.TimeFormat.DATE_ONLY);
    const employeeChangeParams: IEmployeeChangeParameters = {
      employeeId : this.selectedEmployee.employeeId,
      employeeStatusId : this.selectedEmployeeStatusId,
      startDate : payStart,
      endDate : payEnd,
      sequenceId : this.selectedSequenceId,
      payType : this.selectedPayTypeId,
      returnFilterOnly : true,
      csvChangeLogIds : ''
    };
    let count = 0;
    const employeeChanges$ = this.reportApiService.getEmployeeChanges(employeeChangeParams);

    zip(employeeChanges$)
    .pipe(map(results => ({ employeeChanges: results[0]})))
    .subscribe(results => {
      this.employeeChanges = results.employeeChanges;

      this.employeeChanges.forEach(function(element) {
        count += element.changedItem.length;
        element.changedItem.sort((v1, v2) => {
          return v1.friendlyDesc < v2.friendlyDesc ? -1 : 1;
        });
      });

      this.employeeChanges.forEach(function(element) {
        if (element.tFriendlyDesc == 'Employee' || element.tFriendlyDesc == 'Pay' || element.tFriendlyDesc == 'Rates' || element.tFriendlyDesc == 'Emergency Contact'  || element.tFriendlyDesc == 'Benefit Info') {
          element.smColWidth = true;
        }
      });
      this.filteredChanges = this.employeeChanges;
      this.searchControl.setValue('');
      this.selectedEmployeeChanges = count;
      this.filteredEmployeeChanges = count;
      this.isFlitering = false;
      this.firstLoad = true;
    });
}

  submitClick() {
    this.isDisabled = true;
    let csvString = '';

    if (!this.checkEverything) {
      this.filteredChanges.forEach(function(element) {
        element.filtered.forEach(function(element) {
          if (element.selected) {
            csvString += element.changeLogIds;
            csvString += ',';
          }
        });
      });

      csvString = csvString.substring(0, csvString.length - 1);

      if (csvString == '') { csvString = ","; }
    }


    this.selectedFormat = this.fileFormats.find(item => item.scheduledReportFileFormatId === this.selectedFormatId);

    const employeeChangeParams = <IReportParameter> {};
    employeeChangeParams.EmployeeId = this.selectedEmployee.employeeId;
    employeeChangeParams.EmployeeStatus = this.selectedEmployeeStatusId.toString();

    employeeChangeParams.StartDate = this.selectedStartDate;
    employeeChangeParams.EndDate = this.selectedEndDate;
    employeeChangeParams.SequenceId = this.selectedSequenceId;
    employeeChangeParams.EmployeeTypeId = this.selectedPayTypeId.toString();
    employeeChangeParams.ReturnFilterOnly = false;
    employeeChangeParams.CsvChangeLogIds = csvString;
    employeeChangeParams.fileFormatId = this.selectedFormat.scheduledReportFileFormat;
    employeeChangeParams.fileTypeId = -88;

    if (this.selectedPayrollId == null) {
      const startDateString = convertToMoment(this.selectedStartDate).startOf('day').format('YYYY-MM-DDTHH:mm:ss');
      const endDateString = convertToMoment(this.selectedEndDate).startOf('day').format('YYYY-MM-DDTHH:mm:ss');
      employeeChangeParams.StartDate = startDateString;
      employeeChangeParams.EndDate = endDateString;
      employeeChangeParams.EndDate = convertToMoment(employeeChangeParams.EndDate).add(23, 'hours').add(59, 'minutes').add(59, 'seconds').format('YYYY-MM-DDTHH:mm:ss');
    }

    this.reportApiService.generateReports(employeeChangeParams)
    .subscribe(reportDownload => {

        const w = window,
        d = document,
        e = d.documentElement,
        g = d.getElementsByTagName('body')[0],
        x = w.innerWidth || e.clientWidth || g.clientWidth,
        y = w.innerHeight || e.clientHeight || g.clientHeight;
        let reportPostParams = new HttpParams();
        reportPostParams = reportPostParams.append('f', reportDownload.fileName.toString());
        reportPostParams = reportPostParams.append('t', encodeURIComponent(reportDownload.token.toString()));
        const request = new HttpRequest('GET', `${ReportsService.EMPLOYEECHANGE_API_BASE}/EmployeeChanges`, {params: reportPostParams}).urlWithParams;
        this.DsPopup.open(request, '_blank', { height: y / 1.25, width: x / 1.25, top: 0, left: 0 });
        this.isDisabled = false;
      });
  }

  selectAll(checked: Boolean, description?: string) {
    if (description != null && checked) {
      const changes = this.filteredChanges.find(item => item.tFriendlyDesc === description);
      changes.changedItem.forEach(function(element) {
        element.selected = true;
      });
    } else if (checked) {
      this.filteredChanges.forEach(function(element) {
        if (element.filtered.length !== 0) {
          element.isChecked = true;
          element.filtered.forEach(function(element) {
            element.selected = true;
          });
        }
      });
    }
    if (description != null && !checked) {
      this.checkEverything = false;
      const changes = this.filteredChanges.find(item => item.tFriendlyDesc === description);
      changes.changedItem.forEach(function(element) {
        element.selected = false;
      });
    } else if (!checked) {
      this.filteredChanges.forEach(function(element) {
        element.isChecked = false;
        element.changedItem.forEach(function(element) {
          element.selected = false;
        });
      });
    }
  }

  applySearch(searchValue: string) {
    let count = 0;
    this.filteredChanges.forEach(thing => (<any>thing).filtered = thing.changedItem.
    filter(x => x.friendlyDesc.toLowerCase().indexOf(searchValue.toLowerCase()) > -1));
    this.filteredChanges.forEach(function(item) {
      count += item.filtered.length;

      // if(item.filtered.length === 0 && item.isChecked) {
      //   item.isChecked = false;
      // }
    });
    this.filteredEmployeeChanges = count;
  }

  toggleSelect(description: string, checked: Boolean) {
    if (description != null && !checked) {
      this.checkEverything = false;
      const changes = this.employeeChanges.find(item => item.tFriendlyDesc === description);
      changes.isChecked = checked;
    }
  }

  payTypeChange() {
    // this.filteredEmployees = this.employees.reduce(item => item.)
  }

}
