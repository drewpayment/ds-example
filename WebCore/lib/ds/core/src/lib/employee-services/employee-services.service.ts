import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Moment } from 'moment';
import * as moment from 'moment';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { ClockEmployeePunchDto } from '@ds/core/employee-services/models/clock-employee-punch-dto';
import { ClockEmployeeSetup } from '@ajs/labor/clock-employee/models';
import { CheckPunchTypeResultDto, ScheduledHoursWorkedResult } from './models';
import { convertToMoment } from '../shared/convert-to-moment.func';
import { switchMap, tap, shareReplay, skipWhile } from 'rxjs/operators';
import { AccountService } from '../account.service';
import { UserInfo } from '../shared/user-info.model';

@Injectable({
    providedIn: 'root'
})
export class EmployeeServicesService {

    static readonly EMPLOYEE_LABOR_API_BASE = 'api/labor/setup';
    static readonly CLOCK_API_BASE = 'api/Clock';
    static readonly MOBILE_API_BASE = 'api/mobile';
    private _startDate: Moment;
    private _endDate: Moment;
    private _user: UserInfo;

    private _fetchHoursWorked$: Observable<ScheduledHoursWorkedResult>;

    get startDate(): Moment {
        return this._startDate;
    }
    get endDate(): Moment {
        return this._endDate;
    }
    
    private _clockSetup$ = new BehaviorSubject<ClockEmployeeSetup>(null);
    
    employeeLaborSetup$: Observable<ClockEmployeeSetup> = this._clockSetup$
        .pipe(
            skipWhile(s => {
                if (!s) this.refreshClockSetup();
                return !s;
            }),
            switchMap(setup => {
                return this.accountService.getUserInfo();    
            }),
            switchMap(user => {
                this._user = user;
                const api = `${EmployeeServicesService.EMPLOYEE_LABOR_API_BASE}/employees/${this._user.employeeId}`;
                return this.http.get<ClockEmployeeSetup>(api);
            }),
            shareReplay(1)
        );

    constructor(
        private http: HttpClient,
        private accountService: AccountService
    ) {}
    
    refreshClockSetup() {
        this.getEmployeeLaborSetup().subscribe(setup => this._clockSetup$.next(setup));
    }

    private setFetchHoursWorkedObservable(): Observable<ScheduledHoursWorkedResult> {
        if (!this._fetchHoursWorked$) {
            this._fetchHoursWorked$ = this.accountService.getUserInfo()
                .pipe(
                    switchMap(u => {
                        this._user = u;
                        const start = this._startDate.format('YYYY-MM-DD') || moment().startOf('week').format('YYYY-MM-DD');
                        const end   = this._endDate.format('YYYY-MM-DD')   || moment().endOf('week').format('YYYY-MM-DD');
                        const apiUri = `${EmployeeServicesService.CLOCK_API_BASE}/weekly-hours-worked/clients/`
                            + `${this._user.selectedClientId()}/employees/${this._user.employeeId}?start=${start}&end=${end}`;

                        return this.http.get<ScheduledHoursWorkedResult>(apiUri);
                    }),
                    shareReplay()
                );
        }
        return this._fetchHoursWorked$;
    }

    getWeeklyHoursWorked(
        startDate?: Moment | Date | string,
        endDate?: Moment | Date | string
    ): Observable<ScheduledHoursWorkedResult> {
        if (startDate && endDate || !this._user || !this._fetchHoursWorked$) {
            this._startDate = convertToMoment(startDate);
            this._endDate   = convertToMoment(endDate);
            this.setFetchHoursWorkedObservable();
        }
        return this._fetchHoursWorked$;
    }

    getEmployeeLaborSetup(): Observable<ClockEmployeeSetup> {
        return this.accountService.getUserInfo()
            .pipe(switchMap(user => {
                this._user = user;
                const apiUri = `${EmployeeServicesService.EMPLOYEE_LABOR_API_BASE}/employees/${this._user.employeeId}`;
                return this.http.get<ClockEmployeeSetup>(apiUri);
            }));
    }

    getNextPunchDetail(employeeId: number): Observable<CheckPunchTypeResultDto> {
        const apiUri = `${EmployeeServicesService.CLOCK_API_BASE}/punchDetail/${employeeId}?config=true`;

        const response = this.http.get<CheckPunchTypeResultDto>(apiUri);

        return response;
    }

    getEmployeeShiftPunches(
        employeeId: number,
        shiftDate: Moment | Date | string = moment()
    ): Observable<ClockEmployeePunchDto[]> {
        const shiftDateString = convertToMoment(shiftDate).format('YYYY-MM-DD');

        const apiUri = `${EmployeeServicesService.CLOCK_API_BASE}/get/employee/punches`
            + `?id=${employeeId}&shiftDate=${shiftDateString}`;

        const response = this.http.get<ClockEmployeePunchDto[]>(apiUri);

        return response;
    }

    getEmployeePunches(
        employeeId: number,
        startDate: Moment | Date | string = moment().startOf('week'),
        endDate: Moment | Date | string = moment().endOf('week')
    ): Observable<ClockEmployeePunchDto[]> {
        const startDateString = convertToMoment(startDate).format('YYYY-MM-DD');
        const endDateString = convertToMoment(endDate).format('YYYY-MM-DD');

        const apiUri = `${EmployeeServicesService.CLOCK_API_BASE}/get/employee/punches`
            + `?id=${employeeId}&startDate=${startDateString}&endDate=${endDateString}`;

        const response = this.http.get<ClockEmployeePunchDto[]>(apiUri);

        return response;
    }


    //   // Probably don't need this anymore...
    //   getClockClientRulesSummary(employeeId: number) {
    //     const apiUri = `${EmployeeServicesService.MOBILE_API_BASE}/clock/clientRulesSummary/${employeeId}`;

    //     const response = this.http.get<ClockClientRulesSummary>(apiUri);

    //     return response;
    //   }

}
