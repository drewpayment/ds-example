import { Injectable } from '@angular/core';
import { ClockService } from '../employee-services/clock.service';
import { AccountService } from '../account.service';
import { UserInfo } from '../shared';
import { RealTimePunchRequest, CheckPunchTypeResult, JobCosting,
    RealTimePunchPairRequest, InputHoursPunchRequest, EmployeeTimePolicyLunchConfiguration } from '../employee-services/models';
import { JobCostingType, DayOfWeek } from '../employee-services/enums';
import { FormGroup } from '@angular/forms';
import { skipWhile, switchMap, catchError } from 'rxjs/operators';
import * as moment from 'moment';
import { Observable, throwError } from 'rxjs';
import { Moment } from 'moment';

@Injectable()
export class PunchService {
    private punchDetail: CheckPunchTypeResult;
    private jobCostingList: JobCosting[];

    user: UserInfo;
    formTypes = { normal: 'normal', punches: 'punches', hours: 'hours' };
    clockName = '';

    constructor(private service: ClockService, private accountService: AccountService) {
        this.accountService.getUserInfo().subscribe(u => this.user = u);
        this.service._nextPunchDetail.pipe(skipWhile(p => !p)).subscribe(detail => this.punchDetail = detail);
    }

    saveInputHours(form: FormGroup, jobCostingList?: JobCosting[], clockName?: string): Observable<CheckPunchTypeResult> {
        this.validateInputHoursForm(form);
        if (form.invalid)
            return throwError(new Error('Please confirm all required fields complete.'));

        if (clockName !== null)
            this.clockName = clockName;

        const req = this.buildInputHoursRequest(form, jobCostingList);

        return this.service.processInputHoursPunchRequest(req)
            .pipe(
                catchError((err) => {
                    return throwError(err);
                }),
                switchMap(result => {
                    return this.service.getNextPunchDetail(this.user.employeeId);
                })
            );
    }

    saveInputPunches(form: FormGroup, jobCostingList?: JobCosting[], clockName?: string): Observable<CheckPunchTypeResult> {
        form.markAllAsTouched();
        if (form.invalid) return throwError(new Error('Form Invalid'));

        if(clockName !== null)
            this.clockName = clockName;

        const req = this.buildInputPunchesRequest(form, jobCostingList);
        return this.service.processRealTimePunchPair(req).pipe(
            catchError(err => {
                return throwError(err);
            }),
            switchMap(result => {
                if (!result.succeeded) return throwError(result);

                return this.service.getNextPunchDetail(this.user.employeeId);
            })
        );
    }

    saveNormalPunch(form: FormGroup, jobCostingList?: JobCosting[], clockName?: string): Observable<CheckPunchTypeResult> {
        form.markAllAsTouched();
        if (form.invalid) {
            return throwError(new Error('Please fill out all required fields.'));
        }

        // if a clockName is passed in, we set the class property to reflect that value
        if (clockName !== null)
            this.clockName = clockName;

        const req = this.buildNormalPunchRequest(form, jobCostingList);

        return this.service.processRealTimePunch(req).pipe(
            catchError(err => {
                return throwError(err);
            }),
            switchMap(result => {
                if (!result.succeeded) {
                    return throwError(result);
                }

                return this.service.getNextPunchDetail(this.user.userEmployeeId);
            })
        );
    }

    qualifyLunchPunchType(detail: CheckPunchTypeResult): number {
        const punches = detail && detail.employeeClockConfiguration && detail.employeeClockConfiguration.clockEmployee
            && detail.employeeClockConfiguration.clockEmployee
            ? detail.employeeClockConfiguration.clockEmployee.punches : [];

        // If there are two lunch punches already, we don't set the punch type to a lunch punch
        const existingLunchPunches = punches.filter(p => p.clockClientLunchId && p.clockClientLunchId > 0);
        const hasTwoLunchPunches = existingLunchPunches && existingLunchPunches.length && existingLunchPunches.length % 2 === 0;
        if (hasTwoLunchPunches) return null;

        // If there are no punches, the first punch cannot be a lunch punch
        if (!punches.length) return null;

        const lunches = detail && detail.employeeClockConfiguration && detail.employeeClockConfiguration.clockEmployee
            && detail.employeeClockConfiguration.clockEmployee.timePolicy
            ? detail.employeeClockConfiguration.clockEmployee.timePolicy.lunches : [];
        const lunch = lunches && lunches.length ? lunches[0] : null;
        if (!lunch) return null;

        const today = moment();
        const rightNow = moment(today.format('HH:mm'), 'HH:mm');
        const startTimeArr = (<string>lunch.startTime).split('T');
        const stopTimeArr = (<string>lunch.stopTime).split('T');
        const startTime = moment(startTimeArr[startTimeArr.length - 1], 'HH:mm:ss');
        const stopTime = moment(stopTimeArr[stopTimeArr.length - 1], 'HH:mm:ss');
        if (startTime.isSameOrBefore(rightNow) && stopTime.isSameOrAfter(rightNow) && this.checkMomentIsInLunchSchedule(today, lunch)) {
            return lunch.clockClientLunchId;
        } else {
            return null;
        }
    }

    private checkMomentIsInLunchSchedule(m: Moment, lunch: EmployeeTimePolicyLunchConfiguration): boolean {
        let result = false;
        switch (m.weekday()) {
            case DayOfWeek.Sunday:
                result = lunch.isSunday;
                break;
            case DayOfWeek.Monday:
                result = lunch.isMonday;
                break;
            case DayOfWeek.Tuesday:
                result = lunch.isTuesday;
                break;
            case DayOfWeek.Wednesday:
                result = lunch.isWednesday;
                break;
            case DayOfWeek.Thursday:
                result = lunch.isThursday;
                break;
            case DayOfWeek.Friday:
                result = lunch.isFriday;
                break;
            case DayOfWeek.Saturday:
                result = lunch.isSaturday;
                break;
            default:
                result = false;
                break;
        }
        return result;
    }

    private buildNormalPunchRequest(form: FormGroup, jobCostingList: JobCosting[]): RealTimePunchRequest {
        this.jobCostingList = jobCostingList;
        const formValue = form.getRawValue();
        const punchType = formValue.punchType;

        const result = {
            clientId: this.user.selectedClientId(),
            employeeId: this.user.userEmployeeId,
            isOutPunch: this.punchDetail.isOutPunch,
            clockName: this.clockName,
            costCenterId: this.resolveCostCenterId(formValue.costCenter),
            departmentId: formValue.department && formValue.department > 0 ? formValue.department : null,
            divisionId: formValue.division && formValue.division > 0 ? formValue.division : null,
            employeeComment: formValue.employeeNote,
            jobCostingAssignmentId1: formValue.clientJobCostingAssignmentId1,
            jobCostingAssignmentId2: formValue.clientJobCostingAssignmentId2,
            jobCostingAssignmentId3: formValue.clientJobCostingAssignmentId3,
            jobCostingAssignmentId4: formValue.clientJobCostingAssignmentId4,
            jobCostingAssignmentId5: formValue.clientJobCostingAssignmentId5,
            jobCostingAssignmentId6: formValue.clientJobCostingAssignmentId6,
            punchTypeId: punchType && punchType > 0 ? punchType : null
        } as RealTimePunchRequest;
        return this.addJobCostingToModel(form, result, jobCostingList);
    }

    private destructJobCostingFormObjects(form: FormGroup, jcList: JobCosting[]) {
        const f = JSON.parse(JSON.stringify(form.getRawValue()));
        const result = {} as any;

        for (let i = 1; i < 7; i++) {
            result[`clientJobCostingAssignmentId${i}`] = f[`clientJobCostingAssignmentId${i}`]
                ? f[`clientJobCostingAssignmentId${i}`].clientJobCostingAssignmentId
                : null;
        }

        jcList.forEach(jc => {
            switch (jc.jobCostingTypeId) {
                case JobCostingType.CostCenter:
                    result.jobCostingCostCenter = f.jobCostingCostCenter ? f.jobCostingCostCenter.clientJobCostingAssignmentId : null;
                    break;
                case JobCostingType.Department:
                    result.jobCostingDepartment = f.jobCostingDepartment ? f.jobCostingDepartment.clientJobCostingAssignmentId : null;
                    break;
                case JobCostingType.Division:
                    result.jobCostingDivision = f.jobCostingDivision ? f.jobCostingDivision.clientJobCostingAssignmentId : null;
                    break;
                case JobCostingType.Employee:
                    result.jobCostingEmployee = f.jobCostingEmployee ? f.jobCostingEmployee.clientJobCostingAssignmentId : null;
                    break;
            }
        });

        return result;
    }

    // set cost center to the value from the form
    // however, if we are going to send the user's "home cost center", the API prefers to accept
    // NULL as the value of the cost center so that the legacy time clock calcs do not consider it to
    // be an overriden cost center type
    private resolveCostCenterId(value: number | null): number | null {
        let costCenterId = value && value > 0 ? value : null;
        if (costCenterId) costCenterId = this.punchDetail.homeCostCenterId == costCenterId ? null : costCenterId;
        return costCenterId;
    }

    private buildInputPunchesRequest(form: FormGroup, jobCostingList: JobCosting[]): RealTimePunchPairRequest {
        const formValue = form.getRawValue();
        const request = {} as RealTimePunchPairRequest;
        const start = form.value.startDate.format('YYYY-MM-DD')
            + 'T' + moment(form.value.startTime, 'hh:mm:ss a').format('HH:mm:ss');
        const punchType = formValue.punchType;

        const firstResult: RealTimePunchRequest = {
            clientId: this.user.selectedClientId(),
            employeeId: this.user.userEmployeeId,
            isOutPunch: false,
            clockName: this.clockName,
            employeeComment: formValue.startPunchNote,
            overridePunchTime: start,
            costCenterId: this.resolveCostCenterId(formValue.costCenter),
            departmentId: formValue.department && formValue.department > 0 ? formValue.department : null,
            divisionId: formValue.division && formValue.division > 0 ? formValue.division : null,
            jobCostingAssignmentId1: formValue.clientJobCostingAssignmentId1,
            jobCostingAssignmentId2: formValue.clientJobCostingAssignmentId2,
            jobCostingAssignmentId3: formValue.clientJobCostingAssignmentId3,
            jobCostingAssignmentId4: formValue.clientJobCostingAssignmentId4,
            jobCostingAssignmentId5: formValue.clientJobCostingAssignmentId5,
            jobCostingAssignmentId6: formValue.clientJobCostingAssignmentId6,
            punchTypeId: punchType && punchType > 0 ? punchType : null
        };

        // set first punch request
        request.first = this.addJobCostingToModel(form, firstResult, jobCostingList);

        const end = formValue.endDate.format('YYYY-MM-DD')
            + 'T' + moment(formValue.endTime, 'hh:mm:ss a').format('HH:mm:ss');
        const result: RealTimePunchRequest = {
            clientId: this.user.selectedClientId(),
            employeeId: this.user.userEmployeeId,
            isOutPunch: true,
            clockName: this.clockName,
            employeeComment: formValue.endPunchNote,
            overridePunchTime: end,
            costCenterId: this.resolveCostCenterId(formValue.costCenter),
            departmentId: formValue.department && formValue.department > 0 ? formValue.department : null,
            divisionId: formValue.division && formValue.division > 0 ? formValue.division : null,
            jobCostingAssignmentId1: formValue.clientJobCostingAssignmentId1,
            jobCostingAssignmentId2: formValue.clientJobCostingAssignmentId2,
            jobCostingAssignmentId3: formValue.clientJobCostingAssignmentId3,
            jobCostingAssignmentId4: formValue.clientJobCostingAssignmentId4,
            jobCostingAssignmentId5: formValue.clientJobCostingAssignmentId5,
            jobCostingAssignmentId6: formValue.clientJobCostingAssignmentId6,
            punchTypeId: punchType && punchType > 0 ? punchType : null
        };

        request.second = this.addJobCostingToModel(form, result, jobCostingList);

        return request;
    }

    private buildInputHoursRequest(form: FormGroup, jobCostingList: JobCosting[]): InputHoursPunchRequest {
        const clientEarningId = form.getRawValue().clientEarning;
        const eventDate = form.value.startDate.clone().format('YYYY-MM-DD');
        const formValue = form.getRawValue();

        const result: InputHoursPunchRequest = {
            data: {
                clockEmployeeBenefitId: null,
                clientId: this.user.selectedClientId(),
                clientEarningId: clientEarningId,
                employeeId: this.user.userEmployeeId,
                eventDate: eventDate,
                hours: formValue.hours && formValue.hours > 0 ? formValue.hours : null,
                employeeComment: formValue.employeeNote,
                isWorkedHours: true,
                clientCostCenterId: this.resolveCostCenterId(formValue.costCenter),
                clientDepartmentId: formValue.department && formValue.department > 0 ? formValue.department : null,
                clientDivisionId: formValue.division && formValue.division > 0 ? formValue.division : null,
                clientJobCostingAssignmentId1 : formValue.clientJobCostingAssignmentId1,
                clientJobCostingAssignmentId2 : formValue.clientJobCostingAssignmentId2,
                clientJobCostingAssignmentId3 : formValue.clientJobCostingAssignmentId3,
                clientJobCostingAssignmentId4 : formValue.clientJobCostingAssignmentId4,
                clientJobCostingAssignmentId5 : formValue.clientJobCostingAssignmentId5,
                clientJobCostingAssignmentId6 : formValue.clientJobCostingAssignmentId6
            }
        };
        return this.appendJobCostingInputHoursRequest(form, result, jobCostingList);
    }

    private appendJobCostingInputHoursRequest(form: FormGroup, result: InputHoursPunchRequest, jobCostingList: JobCosting[]) {
        if (jobCostingList && jobCostingList.length) {
            const jcFormValue = this.destructJobCostingFormObjects(form, jobCostingList);

            jobCostingList.forEach(jc => {
                let formControlValue;

                if (jc.jobCostingTypeId == JobCostingType.CostCenter) {
                    formControlValue = jcFormValue.jobCostingCostCenter;
                    result.data.clientCostCenterId = formControlValue;
                }

                if (jc.jobCostingTypeId == JobCostingType.Department) {
                    formControlValue = jcFormValue.jobCostingDepartment;
                    result.data.clientDepartmentId = formControlValue;
                }

                if (jc.jobCostingTypeId == JobCostingType.Division) {
                    formControlValue = jcFormValue.jobCostingDivision;
                    result.data.clientDivisionId = formControlValue;
                }

                if (jc.jobCostingTypeId == JobCostingType.Employee) {
                    formControlValue = jcFormValue.jobCostingEmployee;
                }

                if (jc.jobCostingTypeId == JobCostingType.Custom) {
                    formControlValue = jcFormValue[jc.formName];
                    let isSet = false;

                    for (let i = 1; i < 7; i++) {
                        if (isSet) break;

                        const hasSetJcAssignmentValue = result.data[`clientJobCostingAssignmentId${i}`];
                        if (!hasSetJcAssignmentValue) {
                            result.data[`clientJobCostingAssignmentId${i}`] = formControlValue;
                            isSet = true;
                        }
                    }

                    for (let i = 1; i < 7; i++) {
                        if (result.data[`clientJobCostingAssignmentId${i}`] > 0) {
                            continue;
                        }
                        result.data[`clientJobCostingAssignmentId${i}`] = null;
                    }
                }
            });
        }
        return result;
    }

    private addJobCostingToModel(form: FormGroup, result: RealTimePunchRequest, jcList: JobCosting[]) {
        if (jcList && jcList.length) {
            const formValue = this.destructJobCostingFormObjects(form, jcList);
            jcList.forEach(jc => {
                let seqValue;

                if (jc.jobCostingTypeId == JobCostingType.CostCenter) {
                    result.costCenterId = formValue.jobCostingCostCenter;
                    seqValue = result.costCenterId;
                }

                if (jc.jobCostingTypeId == JobCostingType.Department) {
                    result.departmentId = formValue.jobCostingDepartment;
                    seqValue = result.departmentId;
                }

                if (jc.jobCostingTypeId == JobCostingType.Division) {
                    result.divisionId = formValue.jobCostingDivision;
                    seqValue = result.divisionId;
                }

                if (jc.jobCostingTypeId == JobCostingType.Employee) {
                    seqValue = formValue.jobCostingEmployee;
                }

                if (jc.jobCostingTypeId == JobCostingType.Custom) {
                    seqValue = formValue[jc.formName];
                    let isSet = false;

                    for (let i = 1; i < 7; i++) {
                        if (isSet) break;
                        const hasSetJcAssignmentValue = result[`jobCostingAssignmentId${i}`];
                        if (!hasSetJcAssignmentValue) {
                            result[`jobCostingAssignmentId${i}`] = seqValue;
                            isSet = true;
                        }
                    }

                    for (let i = 1; i < 7; i++) {
                        if (result[`jobCostingAssignmentId${i}`] > 0) {
                            continue;
                        }
                        result[`jobCostingAssignmentId${i}`] = null;
                    }
                }
            });
        }
        return result;
    }

    private validateInputHoursForm(form: FormGroup) {
        form.markAllAsTouched();
        const f = form.value;
        if (!f.hours) form.get('hours').setErrors({ required: true });
        // if (!f.clientEarning || f.clientEarning < 1) form.get('clientEarning').setErrors({ required: true });
        // https://thumbs.gfycat.com/SmoggyHilariousBaiji-size_restricted.gif per the following line...
        // if (moment(f.startDate).isBefore(moment(), 'd')) form.get('startDate').setErrors({ minDate: true });
    }

}
