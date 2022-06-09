import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { UserInfo, UserType, MOMENT_FORMATS } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { tap, startWith, distinctUntilChanged, map, switchMap, skipWhile } from 'rxjs/operators';
import { LastPayrollDates, EmployeeLeaveManagementInfo, TimeOffRequest } from '@ds/core/employee-services/models';
import * as moment from 'moment';
import { Moment } from 'moment';
import { DatePipe } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LeaveManagementService } from '@ds/core/employee-services/leave-management.service';
import { environment } from '../../environments/environment';
import { TimeOffPolicy } from 'apps/ds-ess/ajs/leave/time-off/shared/time-off-policy.model';
import { TimeOffPolicySummary } from '../../models';
import { coerceBooleanProperty } from '@angular/cdk/coercion';

enum TimeOffType {
    partial,
    single,
    multiple
}

@Component({
    selector: 'ds-time-off',
    templateUrl: './time-off.component.html',
    styleUrls: ['./time-off.component.scss'],
    providers: [DatePipe]
})
export class TimeOffComponent implements OnInit {
    // this must be assigned before this.createForm() is called
    timeOffType = TimeOffType.partial;
    form: FormGroup = this.createForm();
    user: UserInfo;
    lastPayrollDates: LastPayrollDates;
    lmInfo: EmployeeLeaveManagementInfo[];
    hoursBalanceLabel: string;

    oldStartTime: Moment = moment();
    pageLoading = true;
    requestDisabled = false;

    policies: TimeOffPolicy[];
    selectedPolicy: TimeOffPolicy;
    timeOffPolicySummaries: TimeOffPolicySummary[];

    constructor(
        private fb: FormBuilder,
        private leaveManagementService: LeaveManagementService,
        private accountService: AccountService,
        private sb: MatSnackBar
    ) { }

    ngOnInit() {
        this.initializeFormControlObservers();

        this.accountService.getUserInfo()
            .pipe(
                switchMap(user => {
                    this.user = user;
                    return this.leaveManagementService.getActiveTimeOffEvents(this.user.employeeId);
                }),
                switchMap(policiesWithActiveEvents => {
                    this.policies = policiesWithActiveEvents;
                    if (this.policies && this.policies.length) this.selectedPolicy = this.policies[0];
                    return this.leaveManagementService.getTimeOffPolicies(this.user.clientId, this.user.employeeId);
                }),
                switchMap(policiesSummaries => {
                    this.timeOffPolicySummaries = policiesSummaries;
                    return this.pageLoadResources();
                }),
                tap(() => this.pageLoading = false)
            )
            .subscribe(resources => {
                this.lastPayrollDates = resources.lastPayrollDates;
                this.lmInfo = resources.leaveManagementInfo;

                this.renderResources();
            }, err => {
                if (err && err.lastPayrollDates) {
                    const resources = err as PageLoadResources;
                    this.lastPayrollDates = resources.lastPayrollDates;
                    this.lmInfo = resources.leaveManagementInfo;
                    this.renderResources();
                }
            });
    }

    submitRequest() {
        this.validateFormControls();
        if (!this.form.valid) return;
        this.requestDisabled = true;
        const model = this.prepareModel();

        this.leaveManagementService.saveRequestTimeOff(this.user.lastClientId, model)
            .pipe(
                switchMap(result => {
                    this.sb.open(`Request submitted.`, `dismiss`, { duration: 2500 });
                    this.requestDisabled = false;
                    return this.leaveManagementService.getActiveTimeOffEvents(this.user.employeeId);
                })
            )
            .subscribe(policies => {
                this.policies = policies;

                if (this.policies && this.policies.length) {
                    const policy = this.policies.find(x => x.policyId == this.form.getRawValue().accrual);
                    if (policy) this.selectedPolicy = policy;
                }
                this.requestDisabled = false;
                this.resetForm();
            }, err => {
                this.requestDisabled = false;
                if (err.error && err.error.errors && err.error.errors.length) {
                    this.sb.open(err.error.errors[0].msg, 'dismiss', { duration: 5000 });
                } else {
                    this.sb.open(err.message, 'dismiss', { duration: 5000 });
                }
            });
    }

    private validateFormControls() {
        switch(+this.form.value.type) {
            case TimeOffType.multiple:
                this.validateMultipleDayTimeOffRequest();
                break;
            case TimeOffType.partial:
                this.validatePartialTimeOffRequest();
                break;
            case TimeOffType.single:
            default:
                this.validateSingleDayTimeOffRequest();
                break;
        }
        this.form.markAllAsTouched();
    }

    private validatePartialTimeOffRequest() {
        const f = this.form.value;
        if (!f.hours) {
            this.form.get('hours').setErrors({ required: true });
        } else if (f.hours < 1) {
            this.form.get('hours').setErrors({ invalidHours: true });
        }
        if (!f.startTime) this.form.get('startTime').setErrors({ required: true });
    }

    private validateSingleDayTimeOffRequest() {}

    private validateMultipleDayTimeOffRequest() {
        if (!this.form.value.endDate) this.form.get('endDate').setErrors({ required: true });

        const start = moment(this.form.value.startDate);
        const end = moment(this.form.value.endDate);

        if (end.isBefore(start, 'd')) {
            this.form.get('endDate').setErrors({ minDate: true });
        } else if (end.isSame(start, 'd')) {
            this.form.get('endDate').setErrors({ sameDate: true });
        }
    }

    private renderResources() {
        const hasTimeOffPolicies = this.timeOffPolicySummaries && this.timeOffPolicySummaries.length;
        if (hasTimeOffPolicies) this.form.get('accrual').enable({ emitEvent: false });
        const now = moment();

        if(this.lmInfo) {
            const balanceInfo = this.lmInfo.find(l => l.requestTimeOffId < 0);
            if (balanceInfo)
                this.hoursBalanceLabel = balanceInfo.requestHoursAfter.toString();
        } else {
            this.requestDisabled = true;
        }

        // if there is 0 - 1 accrual options, disable the dropdown

        if (hasTimeOffPolicies && this.timeOffPolicySummaries.length < 2
            && this.user.userTypeId >= UserType.employee) this.form.get('accrual').disable({ emitEvent: false });

        this.patchForm({
            accrual: hasTimeOffPolicies ? this.timeOffPolicySummaries[0].policyId : null,
            startDate: now,
            endDate: null,
            hours: 0,
            note: null,
            startTime: now.format('hh:mm a')
        });
    }

    private pageLoadResources(): Observable<PageLoadResources> {
        const res = {} as PageLoadResources;
        return this.leaveManagementService.getEmployeeLastPayrollDates(this.user.clientId, this.user.userEmployeeId)
            .pipe(
                switchMap(lastPayrollDates => {
                    res.lastPayrollDates = lastPayrollDates;
                    return this.leaveManagementService.getEmployeeLeaveManagementInfo(this.selectedPolicy.policyId, this.user.clientId,
                        this.user.userId, this.user.userEmployeeId, moment(lastPayrollDates.lastPayrollDate));
                }),
                switchMap(info => {
                    res.leaveManagementInfo = info;
                    return of(res);
                })
            );
    }

    private prepareModel(): TimeOffRequest {
        const startDate = moment(this.form.value.startDate).format('YYYY-MM-DD');
        const reqFrom = moment(`${startDate} ${this.form.value.startTime}`, 'YYYY-MM-DD hh:mm a');
        const endDate = this.form.value.endDate
            ? moment(this.form.value.endDate).format('YYYY-MM-DD')
            : moment(startDate).clone().format('YYYY-MM-DD');
        const reqUntil = moment(`${endDate} ${this.form.value.startTime}`, 'YYYY-MM-DD hh:mm a');

        if (this.form.value.type == TimeOffType.partial) reqUntil.add(this.form.value.hours, 'hours');
        if (this.form.value.type == TimeOffType.single) {
            this.form.controls['hours'].setValue(this.selectedPolicy.unitsPerDay, { emitEvent: false });
            reqUntil.add(this.selectedPolicy.unitsPerDay, 'hours');
        }
        if (this.form.value.type == TimeOffType.multiple) {
            // add additional day, to account for "today" being one of the days it counts...
            // reqUntil.add(1, 'days');
            this.form.controls['hours'].setValue(this.selectedPolicy.unitsPerDay, { emitEvent: false });
        }

        const result = {
            clientAccrualId: this.form.getRawValue().accrual,
            employeeId: this.user.userEmployeeId,
            requestFrom: reqFrom.format(MOMENT_FORMATS.DATETIME),
            requestUntil: reqUntil.format(MOMENT_FORMATS.DATETIME),
            modifiedBy: this.user.userId,
            amountInOneDay: this.form.value.hours,
            requesterNotes: this.form.value.note,
            originalRequestDate: moment().format(MOMENT_FORMATS.API),
        } as TimeOffRequest;

        const info = this.lmInfo.find(l => l.clientAccrualId === result.clientAccrualId);
        if (info && info.requestTimeOffId > 0) result.timeOffRequestId = info.requestTimeOffId;

        return result;
    }

    private initializeFormControlObservers() {
        this.form.get('accrual').valueChanges
            .pipe(
                skipWhile(v => !coerceBooleanProperty(v)),
                distinctUntilChanged()
            )
            .subscribe(value => {
                this.selectedPolicy = this.policies.find(p => p.policyId == value);
                this.hoursBalanceLabel = `${this.selectedPolicy.unitsAvailable}`;
            });

        this.form.get('startTime').valueChanges
            .pipe(
                startWith(''),
                distinctUntilChanged(),
                map(value => {
                    const m = moment(value, 'hh:mm a');

                    if (m.isValid()) {
                        this.oldStartTime = m;
                        return m.format('hh:mm a');
                    } else {
                        return this.oldStartTime.format('hh:mm a');
                    }
                })
            ).subscribe(v => {
                if (!environment.production) console.log(v);
                this.form.get('startTime').setValue(v, { emitEvent: false });
            });

        this.form.get('type').valueChanges
            .pipe(
                distinctUntilChanged()
            )
            .subscribe((value: TimeOffType) => {
                this.timeOffType = value;
                if (value == TimeOffType.single) {
                    this.form.setValidators(null);
                    this.form.get('startDate').setValidators([Validators.required]);

                    this.form.updateValueAndValidity();
                }
            });
    }

    private patchForm(v: { accrual: number, startDate: Moment, endDate: Moment, hours: number, note: string, startTime: string }) {
        this.form.patchValue({
            accrual: v.accrual,
            startDate: v.startDate,
            endDate: v.endDate,
            note: v.note,
            startTime: v.startTime,
            hours: v.hours > 0 ? v.hours : ''
        });
    }

    private resetForm() {
        this.form.reset({
            accrual: this.selectedPolicy.policyId,
            type: this.timeOffType.toString(),
            startDate: moment(),
            startTime: moment().format('hh:mm a')
        }, { emitEvent: true });
    }

    private createForm(): FormGroup {
        return this.fb.group({
            accrual: this.fb.control('', [Validators.required]),
            type: this.fb.control(this.timeOffType.toString()),
            hours: this.fb.control(''),
            startDate: this.fb.control('', [Validators.required]),
            endDate: this.fb.control(''),
            startTime: this.fb.control('', { updateOn: 'blur' }),
            note: this.fb.control('')
        });
    }

}

interface PageLoadResources {
    lastPayrollDates: LastPayrollDates;
    leaveManagementInfo: EmployeeLeaveManagementInfo[];
}
