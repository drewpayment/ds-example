import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, ReplaySubject } from 'rxjs';
import { coerceNumberProperty, coerceArray } from '@angular/cdk/coercion';
import { IClientData } from '@ajs/onboarding/shared/models';
import { isArray } from 'util';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IClientPay } from '@ajs/job-profiles/shared/models/client-payroll.interface';

@Injectable({
    providedIn: 'root'
})
export class EmpEeocService {

    private selectedW2Year$ = new BehaviorSubject<number>(null);
    private clientList$ = new BehaviorSubject<IClientData[]>(null);
    private payrollList$ = new BehaviorSubject<number[]>(null);
    private selectedClientIdsList$ = new ReplaySubject<number[]>(1);
    private locationsChanged$ = new BehaviorSubject<boolean>(false);
    private feinNumber$ = new BehaviorSubject<number>(null);
    private clientPayrollList$ = new BehaviorSubject<IClientPay[]>(null);
    private disableYearList$ = new BehaviorSubject<boolean>(false);
    public eeocFormGroup: FormGroup;


    selectedW2Year: Observable<number> = this.selectedW2Year$.asObservable();
    clientList: Observable<IClientData[]> = this.clientList$.asObservable();
    payrollList: Observable<number[]> = this.payrollList$.asObservable();
    selectedClientIdsList: Observable<number[]> = this.selectedClientIdsList$.asObservable();
    locationsChanged: Observable<boolean> = this.locationsChanged$.asObservable();
    feinNumber: Observable<number> = this.feinNumber$.asObservable();
    clientPayrollList: Observable<IClientPay[]> = this.clientPayrollList$.asObservable();
    disableYearList: Observable<boolean> = this.disableYearList$.asObservable();

    constructor() {
        this.selectedW2Year = this.selectedW2Year$.asObservable();
        this.clientList = this.clientList$.asObservable();
        this.payrollList = this.payrollList$.asObservable();
        this.selectedClientIdsList = this.selectedClientIdsList$.asObservable();
        this.locationsChanged = this.locationsChanged$.asObservable();
        this.feinNumber = this.feinNumber$.asObservable();
        this.clientPayrollList = this.clientPayrollList$.asObservable();
        this.disableYearList = this.disableYearList$.asObservable();
        this.eeocFormGroup = new FormGroup({
            companySelector: new FormControl(null, Validators.required),
            validateLocations: new FormControl(null, Validators.required),
            validateEmployees: new FormControl(null, Validators.required),
            createReport: new FormControl(null, Validators.required),
        });
    }

    updateW2Year(val: number): void {
        const cVal = coerceNumberProperty(val);

        if (isNaN(cVal)) return;

        this.selectedW2Year$.next(cVal);
    }

    updateClients(val: IClientData[]): void {
        const cVal = coerceArray(val);

        if (!isArray(cVal)) return;

        this.clientList$.next(cVal);
    }

    updatePayrolls(val: number[]): void {
        const cVal = coerceArray(val);

        if (!isArray(cVal)) return;

        this.payrollList$.next(cVal);
    }

    updateSelectedClientIdsList(val: number[]): void {
        const cVal = coerceArray(val);

        if (!isArray(cVal)) return;

        this.selectedClientIdsList$.next(cVal);
    }

    notifyLocationsChanged(): void {
        this.locationsChanged$.next(null);
    }

    updateSelectedFein(val: number): void {
        const cVal = coerceNumberProperty(val);

        if (isNaN(cVal)) return;

        this.feinNumber$.next(cVal);
    }

    updateClientPayrollList(val: IClientPay[]) {
        const cVal = coerceArray(val);

        if (!isArray(cVal)) return;

        this.clientPayrollList$.next(cVal);
    }

    updateDisableYearList(val: boolean): void {
        this.disableYearList$.next(val);
    }
}
