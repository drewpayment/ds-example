import { Injectable } from '@angular/core';
import { Observable, of, Subject, defer, ReplaySubject } from 'rxjs';
import { InitData } from './shared/InitData.model';
import { StoreBuilder, StoreAction } from '@ds/core/shared/store-builder';
import { HttpClient } from '@angular/common/http';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { ComponentData } from './shared/ComponentData.model';
import { DataRow } from './shared/DataRow.model';
import { map, concatMap, filter, withLatestFrom, tap, switchMap, catchError, defaultIfEmpty, } from 'rxjs/operators';
import { UserType, UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { InsertClockEmployeeApproveDateArgsDto } from './shared/InsertClockEmployeeApproveArgsDto.model';
import { WhatJustinPassesMe } from './shared/getDaatDto.model';
import { TimeCardAuthorizationDataArgs } from './shared/TimeCardAuthorizationDataArgs.model';
import { DaysFilterType } from './shared/DaysFilterType.enum';
import { FilterOption } from './shared/filter-option.model';
import { Maybe } from '@ds/core/shared/Maybe';
import { applyMsgSvcToErrorHandler } from '@ds/core/shared/shared-api-fn';
import {
    applyMsgSvcToSaveHandler, applyMsgSvcToSaveHandlerNoSuccessMsg,
    AutoSaveThrottleStrategyFactory
} from '@ds/core/shared/save-handler-factory';
import * as moment from 'moment';
import { TimeCardAuthorizationService } from './time-card-authorization.service';
import { TimeCardAuthorizationSearchOptions } from '../shared/time-card-authorization-search-options.model';
import { ApproveHourSettings } from './shared/ApproveHourSettings.model';
import { coerceNumberProperty } from '@angular/cdk/coercion';
import { IContact } from '@ds/core/contacts';
import { IClockClientHardware } from '@ds/payroll/time-and-attendance/shared/ClockClientHardware.model';
import { MessageTypes } from "@ajs/core/msg/ds-msg-msgTypes.enumeration";

const baseUrl = 'api/time-card-authorization';

interface RowForm {
    Date: string;
    ddlClockClientNoteOrig: string;
    ddlClockClientNote: string;
    ID: string;
    employeeId: string;
    eventDate: string;
    isApproved: boolean;
    isApprovedVisible: boolean;
    clientCostCenterID: string;
    clockclientNoteID: string;
    clockclientNoteIDOrig: string;
    chkPayToSchedule: string;
    selectHoursDisabled: boolean;
    setClockClientNoteDisabled: boolean;
}

interface TimeCardAuthorizationSaveArgs {
    clientId?: number;
    userId?: number;
    modifiedBy?: number;
    dataEntries?: DataEntry[];
    employeeIds?: number[];
    startDate?: Date;
    endDate?: Date;
    isRecalcActivity?: boolean;
}

interface DataEntry {
    clockEmployeeApproveDate?: InsertClockEmployeeApproveDateArgsDto;
    doRecalcPoints?: boolean;
}



@Injectable({
    providedIn: 'root'
})
export class TimeAndAttendanceService {
    data$: Observable<ComponentData>;
    // make sure something is emitted from the withlatestfrom in our save storeaction effect
    private readonly tchApi = 'api/time-clock-hardware';
    private readonly searchEmps$: Subject<any> = new ReplaySubject(1);
    private readonly save$: Subject<any> = new Subject();
    private readonly loadFilterOptions$: Subject<any> = new Subject();
    private readonly loadFilter2Options$: Subject<any> = new Subject();
    private readonly saveDisplaySettings$: Subject<ApproveHourSettings> = new Subject();
    searchEmpsInFlight = false;
    constructor(
        private http: HttpClient,
        private msgSvc: DsMsgService,
        private accountService: AccountService,
        private tcaService: TimeCardAuthorizationService
    ) {
        const builder = new StoreBuilder<ComponentData>();
        const errorHandler = applyMsgSvcToErrorHandler(this.msgSvc);
        const requestHandlerNoSuccessMsg = applyMsgSvcToSaveHandlerNoSuccessMsg(this.msgSvc);
        builder.setDataSource(
            accountService.PassUserInfoToRequest(
                userInfo => this.http.post<InitData>(baseUrl + '/init-page', <TimeCardAuthorizationDataArgs>{
                    clientId: userInfo.selectedClientId(),
                    controlId: DaysFilterType.ControlID,
                    categoryString: userInfo.userTypeId === UserType.supervisor ? '1,2,3,4,8,13,14,15' : '1,2,3,4,6,8,13,14,15',
                    hideCustomDateRange: false,
                    payrollRunId: 1,
                    userId: userInfo.userId
                }
            )).pipe(map<InitData, ComponentData>(x => {
                return {
                    initData: x,
                    tableData: null,
                    filterValues: {}
                };
            })), errorHandler, 'Get Time Card Authorization Data');

        const searchEmps = builder.scaffoldAction<any, any>();
        searchEmps.dispatcher$ = this.searchEmps$;
        searchEmps.effect = (a, b) => this.accountService.getUserInfo().pipe(
            switchMap(user => of(null)),
            tap(() => this.searchEmpsInFlight = true),
            concatMap(() => accountService.PassUserInfoToRequest(userInfo => this.http.post<DataRow[]>(`${baseUrl}/time-card-approvals`,
                <WhatJustinPassesMe>{
                    approvalStatusDropdownSelectedValue: b.approvalStatus,
                    payPeriodDropdownSelectedValue: b.payPeriod.payrollId,
                    payPeriodDropdownSelectedItemText: b.payPeriod.payPeriod,
                    category1Dropdown: {
                        visible: b.category1Visible,
                        value: b.category1Value
                    },
                    category2Dropdown: {
                        visible: b.category2Visible,
                        value: b.category2Value
                    },
                    daysDropdownSelectedValue: b.persistedSearchSettings.days,
                    endDateFieldText: b.endDate,
                    startDateFieldText: b.startDate,
                    filter1Dropdown: {
                        value: b.filter1Value,
                        visible: b.filter1Visible
                    },
                    filter2Dropdown: {
                        value: b.filter2Value,
                        visible: b.filter2Visible
                    },
                    clientId: userInfo.selectedClientId(),
                    currentPage: b.currentPage,
                    pageSize: b.persistedSearchSettings.empsPerPage
                }))),
            tap(() => this.searchEmpsInFlight = false));
        searchEmps.normalizeSaveHandler = requestHandlerNoSuccessMsg;
        searchEmps.operation = 'Get Employee Data';
        searchEmps.setupFn = builder.nullSetupFn();
        searchEmps.updateState = (result: any, oldState: ComponentData) => {
            oldState.tableData = result;
            return oldState;
        };

        const save = builder.scaffoldAction<any, { form: RowForm, dataRows: DataRow[], startDate: string, endDate: string }>();
        save.dispatcher$ = this.save$;
        save.effect = (a, b): Observable<any> => {
            return this.http.put<Observable<any>>(`${baseUrl}/save`, a)
                .pipe(
                    withLatestFrom(this.searchEmps$),
                    tap(x => setTimeout(() => this.searchEmps(x[1]), 10))
                );
        }; // make sure we set the message to loading after the save message has been set
        save.normalizeSaveHandler = applyMsgSvcToSaveHandler(msgSvc);
        save.operation = 'Save Approvals';
        save.setupFn = x => x.pipe(concatMap(y => accountService.PassUserInfoToRequest(userInfo => defer(() => {
            const result = {
                item: null,
                idOrNewValue: null,
                currentValue: null
            };
            const allItems: { form: RowForm, row: DataRow }[] = [];
            const empIds: number[] = [];

            /**
             * form is a dictionary with row id as the key for each rowForm object inside of it
             * form: { 1: rowForm, 2: rowForm }
             */
            const formList = Object.keys(y.idOrNewValue.form);
            formList.forEach(z => {
                if (isNaN(+z)) return;
                const rowKey = coerceNumberProperty(z);
                const currentRow = y.idOrNewValue.dataRows.find(r => r.id === rowKey);
                const current = y.idOrNewValue.form[rowKey];

                // if the row needs to be approved, but hasn't been approved yet, including the employee id.
                if (current.isApprovedVisible && !current.isApproved)
                    empIds.push(current.employeeId);

                allItems.push({ form: current, row: currentRow });
            });

            // removed in TA-918 to enable saving notes on locked timecards.
            // const itemsToSave = allItems.filter(
            //     i => !i.form.disabled &&
            //     (i.form.isApprovedVisible || i.form.clockclientNoteID !== i.form.clockclientNoteIDOrig)
            // );

            const itemsToSave = allItems.filter(i => {
                const updatingIsApproved = (
                    i.form.isApprovedVisible && i.form.isApproved !== i.row.isApproved && !i.form.selectHoursDisabled &&
                    i.form.isApproved !== undefined && i.form.isApproved !== null
                );

                const updatingClockClientNote = (
                    i.form.clockclientNoteID !== i.form.clockclientNoteIDOrig && !i.form.setClockClientNoteDisabled
                );

                return updatingIsApproved || updatingClockClientNote;
            });

            let resultingDate = '';
            let clockclientNoteID = null;
            let origClockClientNoteID = null;
            // let bolSelectHours = false;
            let isApproved = null;
            let clientCostCenterID = null;

            result.item = <TimeCardAuthorizationSaveArgs>{
                clientId: userInfo.selectedClientId(),
                startDate: y.idOrNewValue.startDate as any,
                endDate: y.idOrNewValue.endDate as any,
                // in original code isRecalcActivity was true when pay to schedule was visible but pay to schedule was never visible
                isRecalcActivity: false,
                dataEntries: itemsToSave.map(s => {
                    const itemLocation = allItems.indexOf(s);
                    for (let i = 0; i < itemLocation; i++) {
                        const current = allItems[i];
                        if (moment(current.form.Date, 'MM/DD/YYYY', true).isValid() ||
                        moment(current.form.Date, 'M/DD/YYYY', true).isValid() ||
                        moment(current.form.Date, 'MM/D/YYYY', true).isValid() ||
                        moment(current.form.Date, 'M/D/YYYY', true).isValid()) {
                            resultingDate = current.form.Date;
                        }
                    }
                    if (moment(s.form.Date, 'MM/DD/YYYY', true).isValid() ||
                        moment(s.form.Date, 'M/DD/YYYY', true).isValid() ||
                        moment(s.form.Date, 'MM/D/YYYY', true).isValid() ||
                        moment(s.form.Date, 'M/D/YYYY', true).isValid()) {
                        resultingDate = s.form.Date;
                    }
                    if ((!('HEADER'.toLowerCase() === s.row.header) && !s.row.isTotalRow && s.row.schedule)) {
                        clockclientNoteID = s.form.clockclientNoteID;
                        origClockClientNoteID = s.form.ddlClockClientNoteOrig;
                    }

                    // if (s.form.isApprovedVisible || clockclientNoteID !== origClockClientNoteID) {
                    //     if (s.form.isApprovedVisible) {
                    //         bolSelectHours = !!s.form.isApproved;
                    //     } else {
                    //         bolSelectHours = false;
                    //     }
                    // }

                    if (s.row.isApproved !== !!s.form.isApproved) {
                        if (!s.row.clientCostCenterID) {
                            resultingDate = null;
                            if (s.form.Date.indexOf('/') >= 0) {
                                resultingDate = s.form.Date;
                            }
                        }

                        if (s.form.clientCostCenterID) {
                            clientCostCenterID = s.form.clientCostCenterID;
                        }

                    }

                    // set isApproved to form value, or row value if undefined or null (no change)
                    isApproved = (s.form.isApproved === null || s.form.isApproved === undefined) ?
                        s.row.isApproved : s.form.isApproved;

                    return <DataEntry>{
                        doRecalcPoints: s.form.clockclientNoteID !== s.form.clockclientNoteIDOrig,
                        clockEmployeeApproveDate: {
                            clientCostCenterID: <any>s.form.clientCostCenterID,
                            clockClientNoteID: <any>s.form.clockclientNoteID,
                            employeeID: <any>s.form.employeeId,
                            eventDate: <any>resultingDate,
                            isApproved: <any>isApproved,
                            modifiedBy: <any>null,
                            payToSchedule: <any>s.form.chkPayToSchedule,
                        }
                    };
                }),

            };
            result.currentValue = y.currentValue;
            result.idOrNewValue = y.idOrNewValue;
            return of(result);
        }))));
        save.updateState = (result, oldState) => {
            // result.forEach(x => {
            //   oldState[x.ID] = x;
            // })
            return oldState;
        };

        const loadFilter1Ids = builder.scaffoldAction<{ [id: number]: FilterOption[] }, any>();
        loadFilter1Ids.dispatcher$ = this.loadFilterOptions$.pipe(filter(x => !(+x.newVal === 0 || +x.newVal === -2)));
        loadFilter1Ids.setupFn = x => x.pipe(map(y => {
            return {
                item: y.currentValue.filterValues,
                idOrNewValue: y.idOrNewValue,
                currentValue: y.currentValue
            };
        }));
        loadFilter1Ids.effect = (item, id: any) => {
            if (item[id.newVal] !== undefined)
                return of(item);

            return accountService.PassUserInfoToRequest(userInfo => {
                const request = +id.newVal === 7 ?
                    this.http
                        .post<FilterOption[]>(
                            `${baseUrl}/get-dropdown-data-employee/filterCategory/` +
                            `${id.newVal}/noOfPayPeriodOptions/${id.form.totalPayPeriodOptions}/payPeriod/` +
                            `${id.form.payPeriod.payrollId}/clientId/${userInfo.selectedClientId()}`,
                            { data: id.form.payPeriod.payPeriod }
                        ) :
                    this.http
                        .get<FilterOption[]>(
                            `${baseUrl}/get-dropdown-data-dynamic/filterCategory/` +
                            `${id.newVal}/clientId/${userInfo.selectedClientId()}`
                        );

                return request.pipe(map(x => {
                    const result = {} as { [key: number]: FilterOption[] };
                    result[id.newVal] = x;
                    return result;
                }));
            });
        };
        loadFilter1Ids.normalizeSaveHandler = requestHandlerNoSuccessMsg;
        loadFilter1Ids.operation = 'Get Filter Values';
        loadFilter1Ids.updateState = (result, staleCache) => {
            Object.assign(staleCache.filterValues, result);
            return staleCache;
        };

        const loadFilter2Ids = builder.scaffoldAction<{ [id: number]: FilterOption[] }, any>();
        loadFilter2Ids.dispatcher$ = this.loadFilter2Options$;
        loadFilter2Ids.setupFn = x => x.pipe(map(y => {
            return {
                item: y.currentValue.filterValues,
                idOrNewValue: y.idOrNewValue,
                currentValue: y.currentValue
            };
        }));
        loadFilter2Ids.effect = (item, id: any) => {
            if (item[id.newVal] !== undefined)
                return of(item);

            return accountService.PassUserInfoToRequest(userInfo => {

                return this.http
                    .get<FilterOption[]>(
                        `${baseUrl}/get-dropdown-data-dynamic/filterCategory/` +
                        `${id.newVal}/clientId/${userInfo.selectedClientId()}`
                    )
                    .pipe(map(x => {
                        const result = {};
                        result[id.newVal] = x;
                        return result;
                    }));

            });
        };
        loadFilter2Ids.normalizeSaveHandler = requestHandlerNoSuccessMsg;
        loadFilter2Ids.operation = 'Get Filter Values';
        loadFilter2Ids.updateState = (result, staleCache) => {
            Object.assign(staleCache.filterValues, result);
            return staleCache;
        };

        const saveDisplaySettingsAction = this.createSaveDisplaySettingsAction(builder, this.saveDisplaySettings$);


        this.data$ = builder
            .addAction(save)
            .addAction(searchEmps)
            .addAction(loadFilter1Ids)
            .addAction(loadFilter2Ids)
            .addAction(saveDisplaySettingsAction)
            .Build();
    }

    private createSaveDisplaySettingsAction(
        builder: StoreBuilder<ComponentData>,
        dispatcher$: Subject<ApproveHourSettings>
    ): StoreAction<any, ComponentData, any> {
        const action = builder.scaffoldAction<any, any>();
        action.dispatcher$ = dispatcher$;

        action.normalizeSaveHandler = AutoSaveThrottleStrategyFactory(
            this.msgSvc,
            (newVal: {
                item: any;
                idOrNewValue: PersistedFormType;
                currentValue: any;
            }, oldVal: {
                item: any;
                idOrNewValue: PersistedFormType;
                currentValue: any;
            }) => {
                if (!oldVal || !newVal.idOrNewValue || !oldVal.idOrNewValue) return false;
                const newThing = newVal.idOrNewValue;
                const oldThing = oldVal.idOrNewValue;
                let areEqual = true;

                for (const p in newThing) {
                    if (!areEqual) return areEqual;
                    areEqual = newThing[p] === oldThing[p];
                }

                return areEqual;
            });
        action.operation = 'Save Display Settings';
        action.setupFn = x => x.pipe(map(val => ({
            item: null,
            idOrNewValue: val.idOrNewValue,
            currentValue: val.currentValue
        })));
        action.effect = (item, newVal) => this.saveDisplaySettingsWithUserContext(newVal);
        action.updateState = (result: ApproveHourSettings, oldState) => {
            const staleVal = new Maybe(oldState.initData.clockEmployeeApproveHoursSettings).valueOr([]);
            if (staleVal.length === 0) {
                staleVal.push(result);
            } else {
                staleVal[0] = result;
            }
            oldState.initData.clockEmployeeApproveHoursSettings = staleVal;
            return oldState;
        };
        return action;
    }

    private saveDisplaySettingsWithUserContext(newVal: ApproveHourSettings): Observable<ApproveHourSettings> {
        return this.http.post<ApproveHourSettings>(`${baseUrl}/display-settings`, newVal);
    }

    searchEmps(formState: any): void {
        this.searchEmps$.next(formState);
    }

    save(formVal: any): void {
        this.save$.next(formVal);
    }

    loadFilterOptions(value: any): void {
        this.loadFilterOptions$.next(value);
    }

    loadFilter2Options(value: any): void {
        this.loadFilter2Options$.next(value);
    }

    saveDisplaySettings(value: ApproveHourSettings): void {
        this.saveDisplaySettings$.next(value);
    }

    getClockClientHardwares(clientId: number):Observable<IClockClientHardware[]> {
        const url = `${this.tchApi}/time-clock-hardwares/clients/${clientId}`;
        return this.http.get<IClockClientHardware[]>(url)
        .pipe(
            catchError(this.httpError('getClockClientHardwares', <IClockClientHardware[]>[]))
        );
    }

    updateClockClientHardware(model: IClockClientHardware, clientId: number): Observable<IClockClientHardware> {
        const url = `${this.tchApi}/clients/${clientId}/time-clock-hardware`;
        return this.http.post<IClockClientHardware>(url, model)
            .pipe(
                catchError(this.httpError('updateClockClientHardware', <IClockClientHardware>{}))
            );
    }

    deleteClockClientHardware(timeClockHardwareId: number):Observable<IClockClientHardware> {
        const url = `${this.tchApi}/time-clock-hardwares/${timeClockHardwareId}`;
        return this.http.delete<IClockClientHardware>(url)
        .pipe(
            catchError(this.httpError('deleteClockClientHardware', <IClockClientHardware>{}))
        );
    }


    private httpError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            let errorMsg = error.error.errors != null && error.error.errors.length
                ? error.error.errors[0].msg
                : error.message;

            this.accountService.log(error, `${operation} failed: ${errorMsg}`);

            // TODO: better job of transforming error for user consumption
            this.msgSvc.setTemporaryMessage(`Sorry, this operation failed: ${errorMsg}`, MessageTypes.error, 6000);

            // let app continue by return empty result
            return of(result as T);
        }
    }
}

interface PersistedFormType {
    employeesPerPage: number;
    clientID: number;
    clockEmployeeApproveHoursSettingsID: number;
    defaultDaysFilter: number;
    hideActivity: boolean;
    hideDailyTotals: boolean;
    hideWeeklyTotals: boolean;
    hideGrandTotals: boolean;
    hideNoActivity: boolean;
    showAllDays: boolean;
    userID: number;
}
