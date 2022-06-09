import { Component, OnDestroy, OnInit } from '@angular/core';
import { IPayrollPayCheckList } from '@ds/payroll/shared';
import { IPaycheckHistory, mapIPaycheckHistoryToIPayrollPayCheckList } from '@ajs/payroll/employee/shared';
import { PayrollService } from '@ds/payroll/shared/payroll.service';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { Observable, of as observableOf, Subject } from 'rxjs';
import { catchError, switchMap, tap, map, takeUntil } from 'rxjs/operators';
import { PaycheckTableService } from '@ds/payroll/paycheck-table/paycheck-table.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MatSortable } from '@angular/material/sort';

@Component({
  selector: 'ds-void-checks',
  templateUrl: './void-checks.component.html',
  styleUrls: ['./void-checks.component.scss']
})
export class VoidChecksComponent implements OnInit, OnDestroy {

  readonly displayedColumns = ['voidCheck', 'name', 'checkNumber', 'subCheck', 'checkDate', 'grossPay', 'netPay', 'checkAmount'];
  readonly initialSortState: MatSortable = {id: 'checkDate', start: 'desc', disableClear: false};
  readonly showVoidChecksButton = true;
  readonly displaySummaryFooter = false;
  readonly emptyStateMessage = 'There are no voidable paychecks in the paycheck history for this employee.';

  userInfo$: Observable<UserInfo>;
  userInfo: UserInfo;
  employeeId: number;
  clientId: number;
  userTypeId: number;

  private _destroy$ = new Subject();

  constructor(
      private payrollApiService: PayrollService,
      private accountService: AccountService,
      private paycheckTableService: PaycheckTableService,
      private msgSvc: DsMsgService,
  ) {
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
    this.accountService.getUserInfo()
    .pipe(
        tap(x => {
            this.userInfo = x;
            this.employeeId = x.selectedEmployeeId();
            this.clientId = x.selectedClientId();
        }),
        catchError(() => {
          const msg = 'Error getting the UserInfo.';
          this.msgSvc.setMessage(msg, MessageTypes.error);
          return observableOf({} as UserInfo);
        }),
        switchMap(userInfo => this.getPayrollPayCheckList(this.employeeId, this.clientId)),
        catchError(() => {
          const msg = 'Error getting the paychecks for this employee.';
          this.msgSvc.setMessage(msg, MessageTypes.error);
          return observableOf(Array<IPayrollPayCheckList>());
        }),
        takeUntil(this._destroy$),
    ).subscribe(payrollPayCheckList => {
        this.paycheckTableService.payrollPayCheckList$.next(payrollPayCheckList);
    });
  }

  private getPaycheckHistory(employeeId: number, clientId: number, startDate?: Date, endDate?: Date)
  : Observable<IPaycheckHistory[]> {
      return this.payrollApiService.getGenPaycheckVoidableChecks(employeeId);
  }

  private getPayrollPayCheckList(employeeId: number, clientId: number, startDate?: Date, endDate?: Date)
  : Observable<IPayrollPayCheckList[]> {
      return this.getPaycheckHistory(employeeId, clientId, startDate, endDate)
        .pipe(
            map(x => x.flatMap(y => mapIPaycheckHistoryToIPayrollPayCheckList(y, employeeId)))
        );
  }

}
