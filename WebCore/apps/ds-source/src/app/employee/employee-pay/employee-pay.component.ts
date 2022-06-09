import { Component, OnInit, Input, ElementRef, OnDestroy } from '@angular/core';
import { PayrollService } from '@ds/payroll/shared/payroll.service';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { IPayrollPayCheckList } from '@ds/payroll/shared';
import {
  IPaycheckHistory,
  mapIPaycheckHistoryToIPayrollPayCheckList,
} from '@ajs/payroll/employee/shared';
import * as moment from 'moment';
import { map, tap, catchError, switchMap, takeUntil } from 'rxjs/operators';
import { Observable, of as observableOf, Subject } from 'rxjs';
import {
  FormGroup,
  FormControl,
  FormBuilder,
  AbstractControl,
  Validators,
} from '@angular/forms';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { PaycheckTableService } from '@ds/payroll/paycheck-table/paycheck-table.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { MatSortable } from '@angular/material/sort';
import { coerceBooleanProperty } from '@angular/cdk/coercion';

@Component({
  selector: 'ds-employee-pay',
  templateUrl: './employee-pay.component.html',
  styleUrls: ['./employee-pay.component.scss'],
})
export class EmployeePayComponent implements OnInit, OnDestroy {
  // Columns originally shown on EmployeePay.aspx:
  // Check Date	Sub Check	Gross Pay	Total Taxes	Deductions	Net Pay	Check Amount	Check Number
  readonly displayedColumns = [
    'name',
    'checkNumber',
    'subCheck',
    'checkDate',
    'grossPay',
    'netPay',
    'checkAmount',
  ];
  readonly initialSortState: MatSortable = {
    id: 'checkDate',
    start: 'desc',
    disableClear: false,
  };
  readonly showVoidChecksButton = false;
  readonly emptyStateMessage =
    'There is no history to display. Please use filters to search pay history.';
  readonly displaySummaryFooter = false;

  payrollPayCheckList: IPayrollPayCheckList[];
  displayVendors = false;
  filterValue = '';

  private _isCurrentUserEmployeeId = false;
  set isCurrentUserEmployeeId(value: string) {
    this._isCurrentUserEmployeeId = coerceBooleanProperty(value);
  }

  userInfo: UserInfo;
  employeeId: number;
  clientId: number;
  userTypeId: number;

  startDate: IDateValidation = {
    getControl: () =>
      this.employeePaycheckHistoryForm.get('startDateInput') as FormControl,
    getDateValue: () => this.getDateValue(this.startDate.getControl()),
    hasError: () => this.dateHasError(this.startDate.getControl()),
    errorMessage: () => '',
    minDate: () => {},
    maxDate: () => this.endDate.getDateValue(),
  };

  endDate: IDateValidation = {
    getControl: () =>
      this.employeePaycheckHistoryForm.get('endDateInput') as FormControl,
    getDateValue: () => this.getDateValue(this.endDate.getControl()),
    hasError: () => this.dateHasError(this.endDate.getControl()),
    errorMessage: () => '',
    minDate: () => this.startDate.getDateValue(),
    maxDate: () => {},
  };

  employeePaycheckHistoryForm = this.fb.group({
    startDateInput: ['', Validators.required],
    endDateInput: ['', Validators.required],
  });

  private _destroy$ = new Subject();

  constructor(
    private payrollApiService: PayrollService,
    private accountService: AccountService,
    private fb: FormBuilder,
    private paycheckTableService: PaycheckTableService,
    private element: ElementRef,
    private msgSvc: DsMsgService
  ) {
    // Enables us to pass strings directly from the aspx page this is included in.
    this.isCurrentUserEmployeeId = this.element.nativeElement.getAttribute(
      'isCurrentUserEmployeeId'
    );

    paycheckTableService.displayedColumns$.next(this.displayedColumns);
    paycheckTableService.initialSortState$.next(this.initialSortState);
    paycheckTableService.showVoidChecksButton$.next(this.showVoidChecksButton);
    paycheckTableService.displaySummaryFooter$.next(this.displaySummaryFooter);
    paycheckTableService.emptyStateMessage$.next(this.emptyStateMessage);
  }

  ngOnDestroy() {
    this._destroy$.next();
  }

  ngOnInit() {
    this.employeePaycheckHistoryForm.patchValue({
      startDateInput: moment().subtract(3, 'months').toDate(),
      endDateInput: moment().toDate(),
    });

    this.accountService
      .getUserInfo()
      .pipe(
        tap((x) => {
          this.userInfo = x;
          this.clientId = x.selectedClientId();
          if (this._isCurrentUserEmployeeId) {
            this.employeeId = x.userEmployeeId;
          } else {
            this.employeeId = x.selectedEmployeeId();
          }
        }),
        catchError(() => {
          const msg = 'Error getting the UserInfo.';
          this.msgSvc.setMessage(msg, MessageTypes.error);
          return observableOf({} as UserInfo);
        }),
        switchMap((userInfo) =>
          this.getPayrollPayCheckList(
            this.employeeId,
            this.clientId,
            this.startDate.getDateValue(),
            this.endDate.getDateValue()
          )
        ),
        catchError(() => {
          const msg = 'Error getting the paychecks for this employee.';
          this.msgSvc.setMessage(msg, MessageTypes.error);
          return observableOf(Array<IPayrollPayCheckList>());
        }),
        takeUntil(this._destroy$)
      )
      .subscribe((payrollPayCheckList) => {
        this.payrollPayCheckList = payrollPayCheckList;
        this.paycheckTableService.payrollPayCheckList$.next(
          this.payrollPayCheckList
        );
      });
  }

  updatePayrollPayCheckList() {
    this.getPayrollPayCheckList(
      this.employeeId,
      this.clientId,
      this.startDate.getDateValue(),
      this.endDate.getDateValue()
    ).subscribe((payrollPayCheckList) => {
      this.payrollPayCheckList = payrollPayCheckList;
      this.paycheckTableService.payrollPayCheckList$.next(
        this.payrollPayCheckList
      );
    });
  }

  private getPaycheckHistory(
    employeeId: number,
    clientId: number,
    startDate: Date,
    endDate: Date
  ): Observable<IPaycheckHistory[]> {
    const AUTO_SUTA_CATCH_UP = 'Auto SUTA Tax Catch Up';

    return this.payrollApiService
      .getEmployeePayHistoryByEmployeeId(
        employeeId,
        clientId,
        startDate,
        endDate
      )
      .pipe(map((x) => x.filter((o) => o.subCheck !== AUTO_SUTA_CATCH_UP)));
  }

  private getPayrollPayCheckList(
    employeeId: number,
    clientId: number,
    startDate: Date,
    endDate: Date
  ): Observable<IPayrollPayCheckList[]> {
    return this.getPaycheckHistory(
      employeeId,
      clientId,
      startDate,
      endDate,
    ).pipe(
      map((x) =>
        x.flatMap((y) =>
          mapIPaycheckHistoryToIPayrollPayCheckList(y, employeeId)
        )
      )
    );
  }

  /**
   * Get boolean representing the error state of a given MatDatepicker.
   *
   * @param fc A formControl, which is assumed to be a MatDatepicker.
   */
  private dateHasError(
    fc: AbstractControl,
    isRequired: boolean = true
  ): boolean {
    const hasErrorMin = fc.hasError('matDatepickerMin');
    const hasErrorMax = fc.hasError('matDatepickerMax');
    const hasErrorParse = fc.hasError('matDatepickerParse');
    const isRequiredAndIsBlank = isRequired && fc.hasError('required');
    return hasErrorMin || hasErrorMax || hasErrorParse || isRequiredAndIsBlank;
  }

  private getDateValue(fc: AbstractControl) {
    return moment(fc.value).toDate();
  }

  dateToFormattedString(date: Date) {
    return convertToMoment(date).format('MM/DD/YYYY');
  }
}

interface IDateValidation {
  getControl(): FormControl;
  getDateValue(): Date;
  hasError(): boolean;
  errorMessage(): string;
  minDate(): any;
  maxDate(): any;
}
