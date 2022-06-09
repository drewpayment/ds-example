import { Component, OnInit, ViewChild, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { IPayrollPayCheckList } from '../shared/index';
import { DsPopupService } from '@ajs/ui/popup/ds-popup.service';
import { PayrollService } from '../shared/payroll.service';
import { isNotUndefinedOrNull } from '@util/ds-common';
import { Observable, BehaviorSubject, iif, of } from 'rxjs';
import { IOpenPayrollDetail } from '@ajs/payroll/history/models';
import { tap, take, map, mergeMap, catchError } from 'rxjs/operators';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { PaycheckTableService } from './paycheck-table.service';
import { IPaycheckHistorySaveVoidChecksDto, PaycheckHistorySaveVoidChecksDto } from '../shared/paycheck-history-save-void-checks-dto.model';
import { WindowRef } from '@ds/core/shared/window-ref.provider';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
    selector: 'ds-paycheck-table',
    templateUrl: './paycheck-table.component.html',
    styleUrls: ['./paycheck-table.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PaycheckTableComponent implements OnInit {

    // Ugly hack: If using showVoidChecksButton, this will get parsed from hidden input element on PayrollAdjustmentsVoidChecks.aspx
    private _voidChecksSelectedPayrollIdInput: number;

    showVoidChecksButton$ = this.paycheckTableSvc.showVoidChecksButton$.asObservable();

    emptyStateMessage$ = this.paycheckTableSvc.emptyStateMessage$.asObservable();

    displaySummaryFooter$ = this.paycheckTableSvc.displaySummaryFooter$.asObservable();

    displayedColumns$ = this.paycheckTableSvc.displayedColumns$.asObservable();

    isVoidCheckColumnVisible$ = this.displayedColumns$.pipe(
        map(x => x.some(y => y === 'voidCheck'))
    );

    // DataSource for the Table.
    payrollPayCheckMatList$ = this.paycheckTableSvc.payrollPayCheckMatList$.asObservable();

    payrollPayCheckList: IPayrollPayCheckList[] | null;
    payrollPayCheckList$ = this.paycheckTableSvc.payrollPayCheckList$.asObservable().pipe(
        tap(payrollPayCheckList => {
            this.payrollPayCheckList = payrollPayCheckList;

            if (this.isArrayAndHasLength(payrollPayCheckList)) {
                const sortedCheckDates = payrollPayCheckList.map(x => x.checkDate).sort();
                this.checkDateStart = sortedCheckDates[0];
                this.checkDateEnd   = sortedCheckDates[sortedCheckDates.length - 1];
            }
        }),
    );

    areAnySelectedChecksToVoid: boolean;
    showVoidCheckboxesAsInvalid: boolean;

    private _isLoadingVoidChecksResponse: boolean;
    get isLoadingVoidChecksResponse() { return this._isLoadingVoidChecksResponse; }

    // First thing we check in the template for the loading cascade...
    private _isLoadingShowVoidChecksButton: boolean = true;
    get isLoadingShowVoidChecksButton() { return this._isLoadingShowVoidChecksButton; }

    private _showVoidChecksButton: boolean;
    get showVoidChecksButton() { return this._showVoidChecksButton; }

    private _isLoadingHasOpenPayrollResponse: boolean;
    get isLoadingHasOpenPayrollResponse() { return this._isLoadingHasOpenPayrollResponse; }

    private _hasOpenPayroll: boolean = true;
    get hasOpenPayroll() { return this._hasOpenPayroll; }

    private _openPayrollDetailSubject$ = new BehaviorSubject<IOpenPayrollDetail>(null);

    checkDateStart:     Date;
    checkDateEnd:       Date;

    totalGrossPay$ = this.paycheckTableSvc.totalGrossPay$.asObservable();
    totalNetPay$ = this.paycheckTableSvc.totalNetPay$.asObservable();
    totalCheckAmount$ = this.paycheckTableSvc.totalCheckAmount$.asObservable();

    private _checksToVoidDtos: IPaycheckHistorySaveVoidChecksDto[];
    private getChecksToVoidDtos(data: IPayrollPayCheckList[]): IPaycheckHistorySaveVoidChecksDto[] {
        const dtos = this.paycheckTableSvc.getChecksToVoid(data).map(x => {
            const dto = new PaycheckHistorySaveVoidChecksDto(x.genPaycheckHistoryId, x.employeeId);
            return dto;
        });
        return dtos;
    }

    @ViewChild(MatSort, {static: false}) set matSort(ms: MatSort) {
      this.paycheckTableSvc.sort$.next(ms);
    }
    @ViewChild(MatPaginator, {static: false}) set matPaginator(mp: MatPaginator) {
      this.paycheckTableSvc.paginator$.next(mp);
    }

    constructor(
        private DsPopup: DsPopupService,
        private payrollApiService: PayrollService,
        private msgSvc: DsMsgService,
        private paycheckTableSvc: PaycheckTableService,
        private changeDetectorRef: ChangeDetectorRef,
        private windowRef: WindowRef,
    ) {
        // Nothing.
    }

    ngOnInit() {
        this.showVoidChecksButton$.pipe(take(1)).subscribe(showVoidChecksButton => {
            // Progress to next part of template's loading cascade.
            this._showVoidChecksButton = showVoidChecksButton;
            this._isLoadingShowVoidChecksButton = false;

            // We only care about open payrolls if we're enabling the saveVoidChecks feature on this table.
            if (showVoidChecksButton) {
                this.setVoidChecksSelectedPayrollIdInputFromHiddenHtmlInput();
                this._isLoadingHasOpenPayrollResponse = true;
                // Pre-fetch some info, so that a subsequent saveVoidChecks button click gets its response quicker.
                this.payrollApiService.getOpenPayrollDetail()
                .pipe(
                    catchError((err, caught) => of(null as IOpenPayrollDetail)),
                )
                .subscribe(openPayrollDetail => {
                    this._hasOpenPayroll = isNotUndefinedOrNull(openPayrollDetail);
                    // Check whether there was a value returned from the api.
                    // If so, also check that the payrollId of the open payroll is the one currently set
                    // as the selected payroll for the session.
                    if (this._hasOpenPayroll && openPayrollDetail.payrollId !== this._voidChecksSelectedPayrollIdInput) {
                        // Triggers a postback to the aspx page, to change the selected payroll in the session.
                        this.setPayrollIdAsSelectedPayrollViaPostback(openPayrollDetail.payrollId, openPayrollDetail.checkDate);
                    } else {
                        // Either there is no open payroll currently,
                        // or there is and it is not the one currently set as the selected payroll for the session.
                        this._openPayrollDetailSubject$.next(openPayrollDetail);
                        this._isLoadingHasOpenPayrollResponse = false;
                        this.changeDetectorRef.detectChanges();
                    }
                });
            } else {
                this._isLoadingHasOpenPayrollResponse = false;
            }
        });
    }

    private getSaveVoidChecksDtoObservables(payrollPayCheckList: Array<IPayrollPayCheckList>)
    : Observable<Array<IPaycheckHistorySaveVoidChecksDto>> {
        const dtos = this.getChecksToVoidDtos(payrollPayCheckList);
        this._checksToVoidDtos = dtos;
        const obs$ = this.payrollApiService.postGenPaycheckVoidableChecks(dtos)
            .pipe(catchError((err, caught) => {
                return of([] as Array<IPaycheckHistorySaveVoidChecksDto>);
            }));
        return obs$;
    }

    // In order to void a check, the user must have an open payroll selected.
    // - If the payroll is not an open payroll the user is prompted to select
    //   the open payroll from the list on the payroll dashboard.
    checkForOpenPayrollAndVoidSelectedChecks() {
        // Manual validation
        this.showVoidCheckboxesAsInvalid = !this.areAnySelectedChecksToVoid;
        if (this.showVoidCheckboxesAsInvalid) {
            return;
        }

        // Set the loading message
        this._isLoadingVoidChecksResponse = true;
        this.msgSvc.loading(this._isLoadingVoidChecksResponse);

        // We don't need to push this subscription; it self completes due to the take(1).
        const sub = this._openPayrollDetailSubject$.pipe(
            map(openPayrollDetail => isNotUndefinedOrNull(openPayrollDetail)),
            // Still not wild about this tap... Not sure how else to make this accessible in the subscription though... :/
            tap(hasOpenPayroll => this._hasOpenPayroll = hasOpenPayroll),
            mergeMap(hasOpenPayroll => {
                return iif(() => hasOpenPayroll,
                    this.getSaveVoidChecksDtoObservables(this.payrollPayCheckList), // VoidSelectedChecks
                    of([] as Array<IPaycheckHistorySaveVoidChecksDto>)              // PromptToOpenPayroll
                );
            }),
            take(1),
        // Each of these results corresponds to a request to void one or more checks for a single employee.
        ).subscribe(unifiedSaveVoidChecksResults => {
            // Unset loading message.
            this._isLoadingVoidChecksResponse = false;
            this.msgSvc.loading(this._isLoadingVoidChecksResponse);

            const sameLength = (this._checksToVoidDtos.length === unifiedSaveVoidChecksResults.length);
            let allChecksWereVoided: boolean = sameLength;

            if (sameLength) {
                // Verify that all of the ones we wanted to void, came back as being voided.
                for (const x of this._checksToVoidDtos) {
                    const found = unifiedSaveVoidChecksResults.some(y => x.equals(y));
                    allChecksWereVoided = allChecksWereVoided && found;
                    if (!found) break;
                }
            }

            if (this._hasOpenPayroll && allChecksWereVoided) {
                const checks = (this._checksToVoidDtos.length > 1) ? 'Checks were' : 'Check was';
                const msg = `${checks} voided successfully.`;
                this.msgSvc.setTemporarySuccessMessage(msg);

                // idk... Give them a few seconds to take in the previous success message, before redirecting them.
                setTimeout(() => window.location.href = 'PayrollAdjustmentsGrid.aspx', 5000);
                // (AC#8): After the adjustments are created the user is navigated to the PayrollAdjustmentsGrid
            } else {
                // TODO: Ideally, this would tell you which check(s) were unable to be voided...
                const s = (this._checksToVoidDtos.length > 1) ? 's' : '';
                const msg = `Unable to void check${s}.`;
                this.msgSvc.setMessage(msg, MessageTypes.error);
            }
        });
    }

    processVoidChecksValidation(index: number) {
        const array = this.paycheckTableSvc.getChecksToVoid(this.payrollPayCheckList);
        const arrayHasLength = (array.length > 0);

        this.areAnySelectedChecksToVoid = arrayHasLength;
        // Only reset the validation state if one or more checks are selected to be voided.
        if (this.areAnySelectedChecksToVoid) {
            this.showVoidCheckboxesAsInvalid = false;
        }
    }

    payrollCheckClicked(element: IPayrollPayCheckList) {
        const w = window,
              d = document,
              e = d.documentElement,
              g = d.getElementsByTagName('body')[0],
              x = w.innerWidth  || e.clientWidth  || g.clientWidth,
              y = w.innerHeight || e.clientHeight || g.clientHeight;

        if (!element.isVendor) {
            const urlBuilder = `api/payroll/check-history/${element.ownerId}/check-report/${element.genPaycheckHistoryId}`;
            this.DsPopup.open(urlBuilder, '_blank', { height: y / 1.25, width: x / 1.25, top: 0, left: 0 });
        } else {
            const urlBuilder = `api/payroll/vendor-check-history/${element.vendorTypeId}/check-report/${element.genPaycheckHistoryId}`;
            this.DsPopup.open(urlBuilder, '_blank', { height: y / 1.25, width: x / 1.25, top: 0, left: 0 });
        }
    }

    isArrayAndHasLength(data: IPayrollPayCheckList[] | null) {
        return Array.isArray(data) && data.length > 0;
    }

    private setVoidChecksSelectedPayrollIdInputFromHiddenHtmlInput() {
        const id = 'VoidChecksSelectedPayrollIdInput';
        const element = <HTMLInputElement>document.getElementById(id);
        this._voidChecksSelectedPayrollIdInput = Number(element.value);
    }

    // Only matters when using void-checks feature via this.showVoidChecksButton
    private setPayrollIdAsSelectedPayrollViaPostback(payrollId: number, checkDate: Date, transferTo?: string) {
        // Making the input have ClientIDMode="Static" in the PayrollAdjustmentsVoidChecks.aspx
        const id = 'VoidChecksPayrollSelectorInput';
        const element = <HTMLInputElement>document.getElementById(id);
        element.value = JSON.stringify({ payrollId: payrollId, checkDate: checkDate}); // , transfer: transferTo });
        const name = element.getAttribute('name');
        if (this.windowRef.nativeWindow.__doPostBack) {
            this.windowRef.nativeWindow.__doPostBack(name, '');
        }
    }
}
