import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Moment } from 'moment';
import { Observable, BehaviorSubject, ReplaySubject, Subject, of } from 'rxjs';
import { CheckPunchTypeResult, JobCostingAssignment, JobCostingAssignmentSelected, JobCosting, JobCostingWithAssociations,
    ClientDivision, ScheduledHoursWorkedResult, PunchTypeItemResult, ClientDepartment, ClientEarning, ClientCostCenter,
    InputHoursPunchRequest, InputHoursPunchRequestResult, RealTimePunchPairRequest, RealTimePunchRequest, RealTimePunchResult,
    RealTimePunchPairResult } from './models';
import { multicast, refCount, tap, map, shareReplay, switchMap } from 'rxjs/operators';
import { PunchOptionType } from './enums';
import * as moment from 'moment';
import { ClientJobCostingCustom } from './models/client-job-costing-custom.model';
import { JobCostingIdAssignments } from 'apps/ds-source/src/app/employee/shared/models/job-costing-aassignments.model';
import { ClockEmployeePunchDto } from './models/clock-employee-punch-dto';
import { IGeofenceEmployeeInfo, ISortedGeofenceEmployees } from 'apps/ds-source/src/app/models';
import { List } from 'lodash';
import { WeeklySchedule } from './models/weekly-schedule.model';
import { TimecardParams } from '@ds/core/employees/shared/timecard-params.model';

@Injectable({
    providedIn: 'root'
})
export class ClockService {

    constructor(private http: HttpClient) {
        // this.getData$ = merge(
        //     this.http.get<JobCostingAssignment[]>(url, { params: params }),
        //     this.hook.pipe(concatMap(x => {
        //         return this.http.get<JobCostingAssignment[]>(url, { params: params })
        //     }))
        // );
    }

    private hook: Subject<any> = new Subject();

    /** hours-worked.component subscribes here to keep updated with time-landing.component */
    _nextPunchDetail = new BehaviorSubject<CheckPunchTypeResult>(null);

    private getNextPunchDetail$: Observable<CheckPunchTypeResult>;

    private clientJobCostingAssignments = new BehaviorSubject<JobCostingAssignment[]>(null);
    private clientJobCostingAssignmentsSelectedList = new BehaviorSubject<JobCostingAssignmentSelected[]>(null);

    private clientDivisions = new BehaviorSubject<ClientDivision[]>(null);
    private periodParams$ = new BehaviorSubject<TimecardParams>(null);

    private weeklyHoursWorked$: Observable<ScheduledHoursWorkedResult>;
    private clientJobCostingList$: Observable<JobCosting[]>;
    private employeeJobCostAssignmentList$: Observable<JobCostingAssignment[]>;
    private getPunchTypeItems$: Observable<PunchTypeItemResult>;
    private getEmployeeDepartments$: Observable<ClientDepartment[]>;
    private getEmployeeCostCenters$: Observable<ClientCostCenter[]>;
    private getEmployeeClientEarnings$: Observable<ClientEarning[]>;
    private getClientJobCostingAssignments$: Observable<JobCostingAssignment[]>;
    private getClientJobCostingAssignmentSelectedList$: Observable<JobCostingAssignmentSelected[]>;
    private getClientDivisions$: Observable<ClientDivision[]>;

    getData$: Observable<any>;

    selectedRow = 0;
    modalClosing = false;

    private apiRoot = 'api/clock';
    private clockSyncRoot = 'api/ClockSync';

    /**
     * API endpoint - /api/clock/punchdetail/{employeeId}
     *
     * @param employeeId
     */
    getNextPunchDetail(employeeId: number): Observable<CheckPunchTypeResult> {
        const url = `api/clock/punchdetail/${employeeId}`;
        const params = new HttpParams()
            .append('ip', 'true')
            .append('config', 'true');

        if (this.getNextPunchDetail$ == null) {
            return this.http.get<CheckPunchTypeResult>(url, { params: params })
                .pipe(
                    shareReplay(1),
                    tap<CheckPunchTypeResult>(detail => this._nextPunchDetail.next(detail))
                );
        }
        return this.getNextPunchDetail$;
    }

    /**
     * /api/clock/jobcosting/{clientId}
     *
     * @param clientId
     */
    getClientJobCostingList(clientId: number): Observable<JobCosting[]> {
        const url = `api/clock/jobcosting/${clientId}`;
        if (this.clientJobCostingList$ == null) {
            this.clientJobCostingList$ = this.http.get<JobCosting[]>(url)
                .pipe(shareReplay(1));
        }
        return this.clientJobCostingList$;
    }

    getPunch(punchId: number, employeeId: number): Observable<number> {
        const url = `${this.apiRoot}/employees/${employeeId}/punches/${punchId}`;
        return this.http.get<ClockEmployeePunchDto>(url)
            .pipe(switchMap(punch => of((punch && punch.clockEmployeePunchId) ? punch.clockEmployeePunchId : null)));
    }

    getPunchesByIdList(punchIdList: number[], employeeId: number): Observable<ClockEmployeePunchDto[]> {
        const url = `${this.apiRoot}/employees/${employeeId}/punches`;
        let params = new HttpParams();
        const uniquePunchIdList = [];

        punchIdList.forEach(punchId => {
            if (!uniquePunchIdList.includes(punchId) && punchId > 0) {
                uniquePunchIdList.push(punchId);
            }
        });

        return this.http.post<ClockEmployeePunchDto[]>(url, uniquePunchIdList);
    }

    punchClicked(): void {


        this.hook.next(true);
    }

    getEmployeeJobCostingAssignmentList(clientId: number, employeeId: number, clientJobCostingId: number, parentJobCostingIds: any[] = null,
        parentJobCostingAssignmentIds: any[] = null, searchText: string = null): Observable<JobCostingAssignment[]> {
        const url = `api/clock/jobCosting/${clientId}/employee/${employeeId}/job/${clientJobCostingId}/list`;
        let params = new HttpParams();
        params = params.append('parentJobCostingIds', parentJobCostingIds == null ? '' : parentJobCostingIds.join(','));
        params = params.append('parentJobCostingAssignmentIds', parentJobCostingAssignmentIds == null ? ''
            : parentJobCostingAssignmentIds.join(','));
        params = params.append('searchText', searchText == null ? '' : searchText);
        if (this.employeeJobCostAssignmentList$ == null) {
            this.employeeJobCostAssignmentList$ = this.http.get<JobCostingAssignment[]>(url, { params: params })
                .pipe(
                    multicast(() => new ReplaySubject(1)),
                    refCount<JobCostingAssignment[]>()
                );
        }
        return this.employeeJobCostAssignmentList$;
    }

    getEmployeeJobCostingAssignmentLists(clientId: number, employeeId: number,
        associatedJobCostings: JobCostingWithAssociations[]): Observable<JobCostingIdAssignments[]> {
        const url = `api/clock/jobcosting/${clientId}/employee/${employeeId}/jobs/list`;
        return this.http.post<JobCostingIdAssignments[]>(url, associatedJobCostings);
    }

    getEmployeeJobCostingAssignmentListsLazy(clientId: number, employeeId: number,
        associatedJobCostings: ClientJobCostingCustom): Observable<JobCostingIdAssignments> {
        const url = `api/clock/jobcosting/${clientId}/employee/${employeeId}/jobs/list/lazy`;
        const associateArray = [associatedJobCostings];
        return this.http.post<JobCostingIdAssignments>(url, associatedJobCostings);
    }

    getPunchTypeItems(employeeId: number): Observable<PunchTypeItemResult> {
        const url = `api/clock/punch-types/${employeeId}`;
        if (this.getPunchTypeItems$ == null) {
            this.getPunchTypeItems$ = this.http.get<PunchTypeItemResult>(url)
                .pipe(
                    multicast(() => new ReplaySubject(1)),
                    refCount<PunchTypeItemResult>(),
                    map(res => {
                        res.items.forEach((v, i, a) => {
                            if (v.id == null) {
                                a[i].id = -1;
                            }
                        });
                        return res;
                    })
                );
        }
        return this.getPunchTypeItems$;
    }

    processRealTimePunch(request: RealTimePunchRequest): Observable<RealTimePunchResult> {
        const url = `api/clock/realtimepunch`;
        return this.http.post<RealTimePunchResult>(url, request);
    }

    getWeeklyHoursWorked(clientId: number, employeeId: number,
        startDate: Moment|Date|string, endDate: Moment|Date|string,
        punchOptionType: PunchOptionType = null, forceRefresh = false): Observable<ScheduledHoursWorkedResult> {
        const url = `api/clock/weekly-hours-worked/clients/${clientId}/employees/${employeeId}`;

        startDate = moment(startDate).format('YYYY-MM-DDT00:00:00');
        endDate = moment(endDate).format('YYYY-MM-DDT11:59:59');
        let params = new HttpParams();
        params = params.append('start', <string>startDate);
        params = params.append('end', <string>endDate);
        if (punchOptionType) { params = params.append('punchOptionType', punchOptionType.toString()); }

        if (this.weeklyHoursWorked$ == null || forceRefresh) {
            this.weeklyHoursWorked$ = this.http.get<ScheduledHoursWorkedResult>(url, { params: params })
                .pipe(shareReplay(1));
        }

        return this.weeklyHoursWorked$;
    }

    getEmployeeDepartments(clientId: number, employeeId: number): Observable<ClientDepartment[]> {
        const url = `api/clock/departments`;
        let params = new HttpParams();
        params = params.append('clientId', clientId.toString());
        params = params.append('employeeId', employeeId.toString());
        if (this.getEmployeeDepartments$ == null) {
            this.getEmployeeDepartments$ = this.http.get<ClientDepartment[]>(url, { params: params })
                .pipe(
                    multicast(() => new ReplaySubject(1)),
                    refCount<ClientDepartment[]>()
                );
        }
        return this.getEmployeeDepartments$;
    }

    processInputHoursPunchRequest(request: InputHoursPunchRequest): Observable<InputHoursPunchRequestResult> {
        const url = `api/clock/input-hours-punch-request`;
        return this.http.post<InputHoursPunchRequestResult>(url, request);
    }

    processRealTimePunchPair(request: RealTimePunchPairRequest): Observable<RealTimePunchPairResult> {
        const url = `api/clock/realtimepunchpair`;
        return this.http.post<RealTimePunchPairResult>(url, request);
    }

    /**
     * CLOCKSYNC CONTROLLER ENDPOINTS
     */

    /**
     * API endpoint - /api/clocksync/costcenter/{clientId?}
     *
     * @param clientId
     */
    getEmployeeCostCenters(clientId: number): Observable<ClientCostCenter[]> {
        const url = `api/clocksync/costcenter/${clientId}`;
        if (this.getEmployeeCostCenters$ == null) {
            this.getEmployeeCostCenters$ = this.http.get<ClientCostCenter[]>(url)
                .pipe(
                    multicast(() => new ReplaySubject(1)),
                    refCount<ClientCostCenter[]>()
                );
        }
        return this.getEmployeeCostCenters$;
    }

    getEmployeeClientEarnings(clientId: number): Observable<ClientEarning[]> {
        const url = `api/clocksync/clients/${clientId}/client-earnings`;
        if (this.getEmployeeClientEarnings$ == null) {
            this.getEmployeeClientEarnings$ = this.http.get<ClientEarning[]>(url)
                .pipe(
                    multicast(() => new ReplaySubject(1)),
                    refCount<ClientEarning[]>()
                );
        }
        return this.getEmployeeClientEarnings$;
    }

    getClientJobCostingAssignments(clientId: number, forceReload: boolean = false): Observable<JobCostingAssignment[]> {
        const url = `api/clocksync/clientjobcostingassignments/${clientId}`;

        if (this.getClientJobCostingAssignments$ == null) {
            this.getClientJobCostingAssignments$ = this.http.get<JobCostingAssignment[]>(url)
                .pipe(
                    multicast(() => new ReplaySubject(1)),
                    refCount(),
                    tap<JobCostingAssignment[]>(res => this.clientJobCostingAssignments.next(res))
                );
        }
        return this.getClientJobCostingAssignments$;
    }

    getClientJobCostingAssignmentSelectedList(clientId: number, forceReload: boolean = false): Observable<JobCostingAssignmentSelected[]> {
        const url = `api/clocksync/clientjobcostingassignmentselecteds/${clientId}`;

        if (this.getClientJobCostingAssignmentSelectedList$ == null) {
            this.getClientJobCostingAssignmentSelectedList$ = this.http.get<JobCostingAssignmentSelected[]>(url)
                .pipe(
                    multicast(() => new ReplaySubject(1)),
                    refCount(),
                    tap<JobCostingAssignmentSelected[]>(res => this.clientJobCostingAssignmentsSelectedList.next(res))
                );
        }
        return this.getClientJobCostingAssignmentSelectedList$;
    }

    getClientDivisions(clientId: number): Observable<ClientDivision[]> {
        const url = `api/clocksync/clientdivisions/${clientId}`;
        if (this.getClientDivisions$ == null) {
            this.getClientDivisions$ = this.http.get<ClientDivision[]>(url)
                .pipe(
                    multicast(() => new ReplaySubject(1)),
                    refCount(),
                    tap<ClientDivision[]>(res => this.clientDivisions.next(res))
                );
        }
        return this.getClientDivisions$;
    }

    getEmployeesByTimePolicy(timePolicyIds: number[]): Observable<ISortedGeofenceEmployees[]> {
        const url = `${this.clockSyncRoot}/time-policy/employees`;

        return this.http.post<ISortedGeofenceEmployees[]>(url, timePolicyIds);
    }

    updateEmployeesGeofence(employees: IGeofenceEmployeeInfo[]): Observable<IGeofenceEmployeeInfo[]> {
        const url = `${this.clockSyncRoot}/time-policy/update/employees`;

        return this.http.post<IGeofenceEmployeeInfo[]>(url, employees);
    }

    getWeeklyScheduleByEmployeeId(userEmployeeId: number): Observable<WeeklySchedule> {
        const url = `api/labor-management/company-rules/getweeklyschedule/${userEmployeeId}`;
        return this.http.get<WeeklySchedule>(url);
    }

    setPeriodFilterParams(payrollId: number,isCustom: boolean, periodStart: Date, periodEnd: Date){
        this.periodParams$.next(
            <TimecardParams>{
                payrollId: payrollId,
                isCustom: isCustom,
                periodStart: periodStart,
                periodEnd: periodEnd
            });
    }
    getPeriodFilterParams(): Observable<TimecardParams>{
        return this.periodParams$;
    }
}
