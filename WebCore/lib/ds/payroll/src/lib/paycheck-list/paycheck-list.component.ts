import { Component, OnInit, OnDestroy } from '@angular/core';
import { PayrollService } from '../shared/payroll.service';
import { IPayrollPayCheckList, IReportParameter, IStandardReport, IEmailReportOptions, IPayrollEmailLog } from '../shared/index';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { HttpParams, HttpRequest } from '@angular/common/http';
import { Observable, of, forkJoin, iif, merge, Subject, NEVER } from 'rxjs';
import { tap, switchMap, concatMap, map, exhaustMap, takeUntil, filter, take, shareReplay, startWith } from 'rxjs/operators';
import { MenuApiService } from '@ds/core/app-config/shared/menu-api.service';
import { UserInfo } from '@ds/core/shared';
import { AccountService, IActionNotAllowedRejection } from '@ds/core/account.service';
import { PaycheckTableService } from '../paycheck-table/paycheck-table.service';
import { MatSortable } from '@angular/material/sort';
import { PayrollReportsToEmail } from '../shared/payroll-reports-to-email.enum';
import { ActionTypes } from '@ds/core/constants/action-types';
import { PopupService } from '@ds/core/popup/popup.service';


/**
 * Higher-order function, used to pass data into another callback function.
 * @param input A function that takes an input of type U, and returns an Observable of type T.
 *              May be thought of as a callback, which the outer function may call to yield the final result.
 * @returns An Observable of type T.
 *          Most common/obvious usage would be to return the same Observable as returned by the input function.
 */
type passData<U> = <T>(input: (variable: U) => Observable<T>) => Observable<T>;

@Component({
    selector: 'ds-paycheck-list',
    templateUrl: './paycheck-list.component.html',
    styleUrls: ['./paycheck-list.component.scss']
})

export class PaycheckListComponent implements OnInit, OnDestroy {

    readonly displayedColumns = ['profileImage', 'name', 'checkDate', 'checkNumber', 'subCheck', 'grossPay', 'netPay', 'checkAmount'];
    readonly initialSortState: MatSortable = {id: 'name', start: 'asc', disableClear: false};
    readonly displaySummaryFooter = true;
    readonly showVoidChecksButton = false;
    readonly emptyStateMessage = 'There is no paycheck history.';

    user: UserInfo;
    isLoading = true;
    isLoadedAndExists: Boolean = false;
    payrollId = 0;
    payrollPayCheckList: IPayrollPayCheckList[];

    private _displayVendors = true;
    get displayVendors() { return this._displayVendors; }
    set displayVendors(bool: boolean) {
        this._displayVendors = bool;
        this.paycheckTableService.doDisplayVendors$.next(this.displayVendors);
    }

    totalPaychecks$ = this.paycheckTableService.totalPaychecks$.asObservable();
    resultingPaychecks$ = this.paycheckTableService.resultingPaychecks$.asObservable();

    PayrollReportsToEmail = PayrollReportsToEmail;

    reports: IStandardReport[];
    reportParams: IReportParameter;
    srLog: IPayrollEmailLog;
    psLog: IPayrollEmailLog;
    glLog: IPayrollEmailLog;
    pfLog: IPayrollEmailLog; // Payroll File
    comPsychLog: IPayrollEmailLog; // ComPsychExport File
    srSending = false;
    glSending = false;
    psSending = false;
    pfSending = false; // Payroll File
    comPsychSending = false; // ComPsychExport File
    showReports = false;
    showEmailPayStubs: Boolean = false;
    showEmailGeneralLedger: Boolean = false;


    private readonly _canPerformAction_PayrollSystemAdministrator$: Observable<boolean>
      = this.accountService
        .canPerformAction(ActionTypes.Payroll.PayrollSystemAdministrator)
        .pipe(startWith(false), shareReplay());

    private readonly _canPerformAction_ReadPayrollHistory$: Observable<boolean>
      = this.accountService
        .canPerformAction(ActionTypes.Payroll.ReadPayrollHistory)
        .pipe(startWith(false), shareReplay());

    readonly canAccessPayrollExportFile$: Observable<boolean>
      = this._canPerformAction_PayrollSystemAdministrator$;

    private readonly _clientIdForAGA2 = 1142;
    canAccessComPsychExportFile$: Observable<boolean>;

    data$: Observable<any>;
    generateReportHook: Subject<PayrollReportsToEmail> = new Subject();
    destroy = new Subject();


    passPayrollId: passData<number>;
    passCheckDate: passData<Date>;
    checkingPayrollStatus: boolean = true;
    payrollNotApplied: boolean;

    ngOnDestroy(): void {
        this.destroy.next();
    }

    /**
     * Workaround to get the check date to show on the menu wrapper top bar,
     * and sets the active menu state when the user is using menu wrapper.
     */
    private getMenuActiveStateObservable() {
        return this.accountService.hasMenuWrapperFeature()
        .pipe(
            switchMap(hasMenu => {
                if (hasMenu) return this.router.events;
                return NEVER;
            }),
            filter(event => event instanceof NavigationEnd),
            tap(event => {
                this.menuApi.setViewPermissions(null, true);

            }),
            takeUntil(this.destroy),
        );
    }

    private setMenuActiveState() {
        const obs$ = this.getMenuActiveStateObservable();
        obs$.subscribe();
    }

    ngOnInit() {
        this.setMenuActiveState();
        this.paycheckTableService.doDisplayVendors$.next(this.displayVendors);
    }

    constructor(
        private payrollApi: PayrollService,
        private accountService: AccountService,
        private route: ActivatedRoute,
        private DsPopup: PopupService,
        private router: Router,
        private menuApi: MenuApiService,
        private paycheckTableService: PaycheckTableService,
    ) {
        paycheckTableService.displayedColumns$.next(this.displayedColumns);
        paycheckTableService.initialSortState$.next(this.initialSortState);
        paycheckTableService.showVoidChecksButton$.next(this.showVoidChecksButton);
        paycheckTableService.displaySummaryFooter$.next(this.displaySummaryFooter);
        paycheckTableService.emptyStateMessage$.next(this.emptyStateMessage);

        const payrollId$ = this.route.params.pipe(
            map(x => +x['payrollId']),
            tap(x => this.payrollId = x)
        );

        this.passPayrollId = (x => payrollId$.pipe(switchMap(id => x(id))));

        const basicInfo$ = payrollId$.pipe(
          switchMap(id => this.payrollApi.getBasicPayryollHistoryByPayrollId(id)),
          tap(x => {
              this.payrollNotApplied = x.isOpen;
              this.checkingPayrollStatus = false;
          })
        );

        const checkDate$ = basicInfo$.pipe(map(x => x.checkDate));

        this.passCheckDate = (x => checkDate$.pipe(switchMap(id => x(id))));

        const userInfo$ = this.getUserInfo();

        const loadData$ = merge(
            userInfo$,
            this.passPayrollId(this.getPayrollCheckList),
            this.getReports(),
            this.passPayrollId(payrollId => this.getPayrollEmailLogs(payrollId, PayrollReportsToEmail.StandardReports, (x) => this.srLog = x)),
            this.passPayrollId(payrollId => this.getPayrollEmailLogs(payrollId, PayrollReportsToEmail.PayrollExportFile, (x) => this.pfLog = x)),
            this.passPayrollId(payrollId => this.getPayrollEmailLogs(payrollId, PayrollReportsToEmail.ComPsychExportFile, (x) => this.comPsychLog = x)),
            this.setUpEmailStuff(),
            this.createGenerateReport(this.generateReportHook)
        );

        this.data$ = basicInfo$.pipe(switchMap(x => iif(() => x.isOpen == false, loadData$, of(null) )));
        
        const isClientCodeAGA2$ = userInfo$.pipe(
            // startWith({clientId: null} as UserInfo),
            map(x => { return {clientId: x.clientId, clientCode: null} }),
            map(x => x.clientId === this._clientIdForAGA2)
        );
        this.canAccessComPsychExportFile$ = isClientCodeAGA2$.pipe(
            startWith(false),
            switchMap(isClientCodeAGA2 => iif(
                () => isClientCodeAGA2,
                this._canPerformAction_ReadPayrollHistory$,
                of(isClientCodeAGA2),
            ))
        );
    }

    private getPayrollCheckList = (payrollId: number) => {
        return this.payrollApi.getPayrollPaycheckHistory(payrollId).pipe(
          tap(paycheckHistories => {
            this.payrollPayCheckList = paycheckHistories || [];

            this.isLoadedAndExists = true;

            this.paycheckTableService.payrollPayCheckList$.next(this.payrollPayCheckList);
        }));
    }

    applyFilter(filterValue: string) {
        this.paycheckTableService.filterValue$.next(filterValue);
    }

    private getReports() {
        return this.payrollApi.getPayrollHistoryReportList().pipe(tap(reports => {
            this.reports = reports;
            this.showReports = (this.reports.length > 0);
        }));
    }

    private getPayrollEmailLogs = (
      payrollId: number,
      logType: PayrollReportsToEmail,
      payrollEmailLogSetter: (x: IPayrollEmailLog) => void
    ) => {
        return this.payrollApi.getPayrollEmailLog(payrollId, logType)
        .pipe(tap((payrollEmailLog: IPayrollEmailLog) => {
          this.mutateThisEmailLog(payrollEmailLog);
          payrollEmailLogSetter(payrollEmailLog);
        }));
    }

    private getUserInfo() {
        return this.accountService.getUserInfo().pipe(tap(x => {
            this.user = x;
        }));
    }

    private setUpEmailStuff() {
        return this.passPayrollId(payrollId => {
            return this.payrollApi.getEmailReportOptions().pipe(
                tap((options: IEmailReportOptions) => {
                    this.showEmailPayStubs      = options.showEmailPayStubs;
                    this.showEmailGeneralLedger = options.showEmailGeneralLedger;
                }),
                concatMap((options: IEmailReportOptions) =>
                    forkJoin(
                        iif(
                            () => options.showEmailPayStubs,
                            this.getPayrollEmailLogs(payrollId, PayrollReportsToEmail.PayStubs, (x) => this.psLog = x),
                            of(null) as Observable<IPayrollEmailLog>
                        ),
                        iif(
                            () => options.showEmailGeneralLedger,
                            this.getPayrollEmailLogs(payrollId, PayrollReportsToEmail.GlInterfaces, (x) => this.glLog = x),
                            of(null) as Observable<IPayrollEmailLog>
                        )
                    ) as Observable<[IPayrollEmailLog, IPayrollEmailLog]>
                ));
        });
    }

    // Mutates thisLog
    private mutateThisEmailLog(thisLog: IPayrollEmailLog) {
        if (thisLog != null) {
            if (thisLog.endDate != null && !thisLog.hasError) {
                thisLog.buttonClass = 'btn btn-action done';
                thisLog.icon        = 'done';
            } else if (thisLog.endDate == null && !thisLog.hasError) {
                thisLog.buttonClass = 'btn btn-action done';
                thisLog.icon        = 'timer';
            } else if (thisLog.hasError) {
                thisLog.buttonClass = 'btn btn-action';
                thisLog.icon        = 'priority_high';
            }
        } else {
            // this.thatLog.buttonClass = 'btn btn-success'
        }
    }

    openReport(reportId: number) {
        const payrollId$ = this.passPayrollId(payrollId => {
            return of(payrollId).pipe(
                take(1),
                tap(id => {
                    const w = window,
                    d = document,
                    e = d.documentElement,
                    g = d.getElementsByTagName('body')[0],
                    x = w.innerWidth || e.clientWidth || g.clientWidth,
                    y = w.innerHeight|| e.clientHeight|| g.clientHeight;

                    //this.reportParams.PayrollId = this.payrollId;
                    const urlBuilder            = `api/payroll/reports/standard/${reportId}`;
                    let params                  = new HttpParams();
                    params                      = params.append('payrollId', id.toString());
                    const request               = new HttpRequest("GET", urlBuilder, {params: params}).urlWithParams;
                    this.DsPopup.open(request, '_blank', { height: y / 1.25, width: x / 1.25, top: 0, left: 0 });
                })
            );
        }).subscribe(x => {
            // Need to subscribe so that this doesn't just sit there.
        });
    }


    generateReport(reportTypeId: PayrollReportsToEmail, payrollId: number, checkDate: Date) {
        const absoluteUrl = window.location.href;
        return this.payrollApi.generateReport(payrollId, reportTypeId, checkDate.toString(), absoluteUrl)
        .pipe(tap(data => {
            if (reportTypeId === PayrollReportsToEmail.StandardReports) {
                this.mutateThisEmailLogAnother(this.srLog, (x) => this.srSending = x);
            } else if (reportTypeId === PayrollReportsToEmail.GlInterfaces) {
                this.mutateThisEmailLogAnother(this.glLog, (x) => this.glSending = x);
            } else if (reportTypeId === PayrollReportsToEmail.PayStubs) {
                this.mutateThisEmailLogAnother(this.psLog, (x) => this.psSending = x);
            } else if (reportTypeId === PayrollReportsToEmail.PayrollExportFile) {
                this.mutateThisEmailLogAnother(this.pfLog, (x) => this.pfSending = x);
            } else if (reportTypeId === PayrollReportsToEmail.ComPsychExportFile) {
                this.mutateThisEmailLogAnother(this.comPsychLog, (x) => this.comPsychSending = x);
            }
        }));
    }

    generateReportHookHook(reportId: PayrollReportsToEmail) {
        this.generateReportHook.next(reportId);
        if (reportId === PayrollReportsToEmail.StandardReports && this.srLog === null) {
            this.srLog = this.createTempLog();
            this.mutateThisEmailLog(this.srLog);
        } else if (reportId === PayrollReportsToEmail.GlInterfaces && this.glLog === null) {
            this.glLog = this.createTempLog();
            this.mutateThisEmailLog(this.glLog);
        } else if (reportId === PayrollReportsToEmail.PayStubs && this.psLog === null) {
            this.psLog = this.createTempLog();
            this.mutateThisEmailLog(this.psLog);
        } else if (reportId === PayrollReportsToEmail.PayrollExportFile && this.pfLog === null) {
            this.pfLog = this.createTempLog();
            this.mutateThisEmailLog(this.pfLog);
        } else if (reportId === PayrollReportsToEmail.ComPsychExportFile && this.comPsychLog === null) {
            this.comPsychLog = this.createTempLog();
            this.mutateThisEmailLog(this.comPsychLog);
        }
    }

    private mutateThisEmailLogAnother(thisLog: IPayrollEmailLog, sendingSetter: (x: boolean) => void) {
        if (thisLog != null) {
            thisLog.buttonClass = 'btn btn-action done';
            thisLog.icon        = 'timer';
            thisLog.endDate     = null;
            thisLog.hasError    = false;
        } else {
            sendingSetter(true);
        }
    }

    private createGenerateReport(hook: Subject<PayrollReportsToEmail>) {
        return this.passPayrollId(payrollId => {
            return this.passCheckDate(checkDate => {
                return hook.pipe(exhaustMap(reportId => this.generateReport(reportId, payrollId, checkDate)));
            });
        });
    }

    // creates a temporary log file that can be passed to srLog, glLog, or psLog to be mutated through mutateThisEmailLog - PAY-1419
    private createTempLog(){
        const log: IPayrollEmailLog = {
            payrollEmailLogId : null,
            clientId          : this.user.clientId,
            payrollId         : this.payrollId,
            logType           : null,
            startDate         : null,
            endDate           : null,
            modifiedBy        : this.user.userId,
            hasError          : false,
            buttonClass       : 'btn btn-action',
            icon              : 'timer'
        }
        return log;
    }

}
