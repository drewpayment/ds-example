import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DashboardFilterOption, UserPerformanceDashboardEmployee, UserPerformanceDashboardResult } from '@ds/analytics/models';
import { AnalyticsApiService } from './analytics-api.service';
import { switchMap, takeUntil, tap } from 'rxjs/operators';
import { DashboardApiService } from 'apps/ds-source/src/app/reports/shared/services/dashboard-api.service';
import { Moment } from 'moment';
import * as moment from 'moment';
import { MOMENT_FORMATS } from '@ds/core/shared';


@Injectable({
    providedIn: 'root',
})
export class AnalyticsService {

    private destroy$ = new Subject();
    private employees$ = new BehaviorSubject<UserPerformanceDashboardEmployee[]>(null);
    private users$ = new BehaviorSubject<UserPerformanceDashboardEmployee[]>(null);
    private userPerfDashboardResult$ = new BehaviorSubject<UserPerformanceDashboardResult>(null);
    private isUserPerfDashboardReqInFlight = false;

    constructor(private api: AnalyticsApiService) {
        this.destroy$.subscribe(() => this.employees$.next(null));
    }

    getUserPerformanceDashboard(clientId: number, startDate: Date|Moment, endDate: Date|Moment, employeeIds: number[], refresh: boolean = false): Observable<UserPerformanceDashboardResult> {
        return this.userPerfDashboardResult$
            .pipe(switchMap(res => {
                if (res != null)
                    return of({...res, isInFlight: this.isUserPerfDashboardReqInFlight} as UserPerformanceDashboardResult);

                this.isUserPerfDashboardReqInFlight = true;
                return this.api.getUserPerformanceDashboard(clientId, moment(startDate).format(MOMENT_FORMATS.API), moment(endDate).format(MOMENT_FORMATS.API), employeeIds)
                    .pipe(tap(res => {
                        this.isUserPerfDashboardReqInFlight = false;
                        this.userPerfDashboardResult$.next({...res, isInFlight: this.isUserPerfDashboardReqInFlight} as UserPerformanceDashboardResult);
                    }));
            }));
    }

    getEmployeeInfo(clientId: number, startDate: Date|Moment, endDate: Date|Moment, employeeIds: number[], refresh: boolean = false): Observable<UserPerformanceDashboardEmployee[]> {
        return this.employees$
            .pipe(
                takeUntil(this.destroy$),
                switchMap(employees => {
                    if (employees && Array.isArray(employees) && employees.length) {
                        return of(employees);
                    }
                    return this.api.getEmployeeInfo(clientId, moment(startDate).format(MOMENT_FORMATS.API), moment(endDate).format(MOMENT_FORMATS.API), employeeIds)
                        .pipe(tap(employees => this.employees$.next(employees)));
                })
            );
    }

    getUserInfo(clientId: number, startDate: Date|Moment, endDate: Date|Moment, employeeIds: number[], refresh: boolean = false): Observable<UserPerformanceDashboardEmployee[]> {
        return this.users$
            .pipe(
                takeUntil(this.destroy$),
                switchMap(users => {
                    if (users && Array.isArray(users) && users.length) {
                        return of(users);
                    }
                    return this.api.getUserInfo(clientId, moment(startDate).format(MOMENT_FORMATS.API), moment(endDate).format(MOMENT_FORMATS.API), employeeIds)
                        .pipe(tap(users => this.users$.next(users)));
                })
            );
    }

    destroy() {
        this.destroy$.next();
    }
}
