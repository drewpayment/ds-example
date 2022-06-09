import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, Validators, FormArray, FormBuilder } from '@angular/forms';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { GeofenceManagementService } from '../geofence-management.service';
import { ClockService } from '@ds/core/employee-services/clock.service';
import { map, filter, switchMap, catchError, tap } from 'rxjs/operators';
import { IGeofenceEmployeeInfo, ISortedGeofenceEmployees } from '../../../models/geofence-employee-info.model';
import { MatPaginator } from '@angular/material/paginator';
import { PayFrequencyTypeEnum } from '@ajs/employee/hiring/shared/models';
import { PayFrequencyCount } from '../../../models';

@Component({
    selector: 'ds-geofence-employees',
    templateUrl: './geofence-management-employees.component.html',
    styleUrls: ['./geofence-management-employees.component.scss']
})

export class GeofenceEmployeesComponent implements OnInit {
    reviewEmployees: FormGroup = this.createForm();
    isLoading = true;
    formInit: Observable<any>;
    timePolicyIds: number[];
    employeeList: IGeofenceEmployeeInfo[];
    sortedEmployees: ISortedGeofenceEmployees[];
    dataSource = new BehaviorSubject<IGeofenceEmployeeInfo[]>(null);
    dataSource$ = this.dataSource.asObservable();
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    displayedColumns: string[] = ['avi', 'name', 'checkbox', 'number', 'timePolicy', 'jobTitle', 'location', 'department', 'supervisor'];
    pagLength = 0;
    tmpIndex = 0;
    employeeCount = 0;
    optedIn = false;
    saveClicked = false;

    totalEmployeePerPayrolls: PayFrequencyCount[] = [
        {
            key: PayFrequencyTypeEnum.Weekly,
            total: 0,
            multiplier: 4,
        },
        {
            key: PayFrequencyTypeEnum.BiWeekly,
            total: 0,
            multiplier: 2,
        },
        {
            key: PayFrequencyTypeEnum.SemiMonthly,
            total: 0,
            multiplier: 2,
        },
        {
            key: PayFrequencyTypeEnum.AlternateBiWeekly,
            total: 0,
            multiplier: 2,
        },
        {
            key: PayFrequencyTypeEnum.Monthly,
            total: 0,
            multiplier: 1,
        },
        {
            key: PayFrequencyTypeEnum.Quarterly,
            total: 0,
            multiplier: (1 / 4),
        },
        {
            key: PayFrequencyTypeEnum.Annually,
            total: 0,
            multiplier: (1 / 12),
        },
    ];

    get employeeRows(): FormArray {
        return this.reviewEmployees.get('rows') as FormArray;
    }

    constructor(
        private service: GeofenceManagementService,
        private msg: DsMsgService,
        private clockSvc: ClockService,
        private fb: FormBuilder,
    ) { }

    ngOnInit() {
        this.initializeListener();
    }

    private createForm(): FormGroup {
        this.service.geofenceFormGroup.get('reviewEmployees').setValue(true);

        return this.reviewEmployees = new FormGroup({
            rows: new FormArray([], Validators.required),
        });
    }

    private initializeListener(): void {
        this.formInit = this.service.timePolicyIdList.pipe(
            filter(timePolicyList => !!timePolicyList && timePolicyList.length > 0),
            switchMap(timePolicyList => {
                this.timePolicyIds = timePolicyList;

                return this.service.sortedEmployees;
            }),
            switchMap((employees) => {
                console.log("GeoFence Employees: ", employees);
                let tmpEmployees: IGeofenceEmployeeInfo[] = [];
                const selectedEmps = employees.filter(empList => this.timePolicyIds.includes(empList.timePolicyId));
                this.employeeCount = 0;
                this.sortedEmployees = employees;

                selectedEmps.forEach((empList: ISortedGeofenceEmployees) => {
                    tmpEmployees = tmpEmployees.concat(empList.employees);
                });

                this.employeeList = tmpEmployees;

                this.employeeRows.clear();

                this.employeeList.forEach((employee) => {
                    this.employeeRows.push(this.fb.control(employee.clockEmployee.geofenceEnabled));

                    if (employee.clockEmployee.geofenceEnabled) this.employeeCount++;
                    this.totalEmployeePerPayrolls.find(x => x.key === employee.payFrequency.payFrequencyId).total++;
                });

                this.totalEmployeePerPayrolls = this.totalEmployeePerPayrolls.sort((b1, b2) => {
                    return b1.total < b2.total ? 1 : -1;
                });

                this.service.updatePayrollMultiplier(this.totalEmployeePerPayrolls[0].multiplier);
                this.service.updateEmployeeCount(this.employeeCount);

                this.pagLength = this.dataSource != null ? this.employeeList.length : 0;
                this.paginatorClicked();

                return this.service.geofenceOptIn;
            }),
            map((optedIn: boolean) => {
                this.optedIn = optedIn;
                this.isLoading = false;

                return true;
            }),
            catchError(err => {
                this.isLoading = false;
                this.msg.setTemporaryMessage('Something went wrong.', this.msg.messageTypes.error);
                console.error(err);

                return of([]);
            })
        );
        this.reviewEmployees.statusChanges
            .subscribe(_ => this.service.geofenceFormGroup.get('reviewEmployees').setValue(this.reviewEmployees.valid || null));
    }

    paginatorClicked(): void {
        const index = this.paginator == null ? 0 : this.paginator.pageIndex;
        const tmpPageSize = this.paginator == null ? 10 : this.paginator.pageSize;
        const tmpIndex = index * tmpPageSize;
        this.tmpIndex = tmpIndex;
        const filtered = this.employeeList.slice(tmpIndex, tmpIndex + tmpPageSize);
        this.dataSource.next(filtered);
    }

    saveEmployees(): void {
        this.saveClicked = true;
        if (this.employeeCount === 0) return;

        this.msg.sending(true);

        this.clockSvc.updateEmployeesGeofence(this.employeeList).subscribe(() => {
            this.msg.sending(false);
            this.service.updateEmployeeCount(this.employeeCount);
            this.service.updateSortedEmployees(this.sortedEmployees);
            this.msg.setTemporarySuccessMessage('The selected employees were saved');
        }, () => {
            this.msg.sending(false);
            this.msg.setTemporaryMessage('There was an error saving the selected employees', this.msg.messageTypes.error);
        });
    }

    checkboxClicked(idx: number): void {
        this.employeeList[idx].clockEmployee.geofenceEnabled = !!this.employeeRows.at(idx).value;
        !!this.employeeRows.at(idx).value ? this.employeeCount++ : this.employeeCount--;

        if (this.employeeCount > 0) this.reviewEmployees.setErrors(null);
        else this.reviewEmployees.setErrors({ invalid: true });
    }

}
