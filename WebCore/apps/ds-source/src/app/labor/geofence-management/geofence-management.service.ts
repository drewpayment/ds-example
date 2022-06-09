import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { coerceArray, coerceNumberProperty, coerceBooleanProperty } from '@angular/cdk/coercion';
import { isArray, isNumber, isBoolean } from 'util';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IClockClientTimePolicy } from '@ajs/labor/models';
import { ISortedGeofenceEmployees } from '../../models';

@Injectable({
    providedIn: 'root'
})
export class GeofenceManagementService {

    private timePolicyIdList$ = new BehaviorSubject<number[]>(null);
    private timePolicyList$ = new BehaviorSubject<IClockClientTimePolicy[]>(null);
    private employeeCount$ = new BehaviorSubject<number>(null);
    private payrollMultiplier$ = new BehaviorSubject<number>(null);
    private geofenceOptIn$ = new BehaviorSubject<boolean>(null);
    private sortedEmployees$ = new BehaviorSubject<ISortedGeofenceEmployees[]>(null);

    public geofenceFormGroup: FormGroup;

    timePolicyIdList: Observable<number[]> = this.timePolicyIdList$.asObservable();
    timePolicyList: Observable<IClockClientTimePolicy[]> = this.timePolicyList$.asObservable();
    employeeCount: Observable<number> = this.employeeCount$.asObservable();
    payrollMultiplier: Observable<number> = this.payrollMultiplier$.asObservable();
    geofenceOptIn: Observable<boolean> = this.geofenceOptIn$.asObservable();
    sortedEmployees: Observable<ISortedGeofenceEmployees[]> = this.sortedEmployees$.asObservable();

    constructor() {
        this.timePolicyIdList = this.timePolicyIdList$.asObservable();
        this.timePolicyList = this.timePolicyList$.asObservable();
        this.employeeCount = this.employeeCount$.asObservable();
        this.payrollMultiplier = this.payrollMultiplier$.asObservable();
        this.geofenceOptIn = this.geofenceOptIn$.asObservable();
        this.sortedEmployees = this.sortedEmployees$.asObservable();

        this.geofenceFormGroup = new FormGroup({
            acceptTerms: new FormControl(null, Validators.required),
            reviewEmployees: new FormControl(null, Validators.required),
            timePolicies: new FormControl(null, Validators.required),
        });
    }

    updateTimePolicyIds(val: number[]): void {
        const cVal = coerceArray(val);

        if (!isArray(cVal)) return;

        this.timePolicyIdList$.next(cVal);
    }

    updateTimePolicies(val: IClockClientTimePolicy[]): void {
        const cVal = coerceArray(val);

        if (!isArray(cVal)) return;

        this.timePolicyList$.next(cVal);
    }

    updateEmployeeCount(val: number): void {
        const cVal = coerceNumberProperty(val);

        if (!isNumber(cVal)) return;

        this.employeeCount$.next(cVal);
    }

    updatePayrollMultiplier(val: number): void {
        const cVal = coerceNumberProperty(val);

        if (!isNumber(cVal)) return;

        this.payrollMultiplier$.next(cVal);
    }

    updateGeofenceOptIn(val: boolean): void {
        const cVal = coerceBooleanProperty(val);

        if (!isBoolean(cVal)) return;

        this.geofenceOptIn$.next(cVal);
    }

    updateSortedEmployees(val: ISortedGeofenceEmployees[]) {
        const cVal = coerceArray(val);

        if (!isArray(cVal)) return;

        this.sortedEmployees$.next(cVal);
    }
}
