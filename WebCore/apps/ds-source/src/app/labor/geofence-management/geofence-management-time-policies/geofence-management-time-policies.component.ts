import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormArray, FormBuilder } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { GeofenceManagementService } from '../geofence-management.service';
import { map, catchError, switchMap } from 'rxjs/operators';
import { EmployeeLaborManagementService } from '../../services/employee-labor-management.service';
import { IClockClientTimePolicy } from '@ajs/labor/models';
import { ISortedGeofenceEmployees, IGeofenceEmployeeInfo } from '../../../models';

@Component({
    selector: 'ds-geofence-time-policies',
    templateUrl: './geofence-management-time-policies.component.html',
    styleUrls: ['./geofence-management-time-policies.component.scss']
})

export class GeofenceTimePolicyComponent implements OnInit {
    timePolicies: FormGroup;
    isLoading = true;
    formInit: Observable<any>;
    clientTimePolicies: IClockClientTimePolicy[];
    timePolicyCount = 0;
    timePolicyIdList: number[] = [];
    timePolicyList: IClockClientTimePolicy[] = [];
    sortedEmps: ISortedGeofenceEmployees[];
    optedIn = false;
    saveClicked = false;
    hasEmployeesSelected = false;

    get timePolicyRows(): FormArray {
        return this.timePolicies.get('rows') as FormArray;
    }

    constructor(
        private service: GeofenceManagementService,
        private msg: DsMsgService,
        private employeeMngmntSvc: EmployeeLaborManagementService,
        private fb: FormBuilder,
    ) { }

    ngOnInit() {
        this.createForm();
        this.initializeListener();
    }

    private initializeListener(): void {
        this.formInit = this.service.timePolicyList.pipe(
            switchMap((timePolicies: IClockClientTimePolicy[]) => {
                this.clientTimePolicies = timePolicies.filter(x => x.employeeCount !== 0);
                this.timePolicyIdList.length = 0;

                this.timePolicyRows.clear();

                this.clientTimePolicies.forEach(timePolicy => {
                    this.timePolicyRows.push(this.fb.control(timePolicy.geofenceEnabled));
                    if (timePolicy.geofenceEnabled) {
                        this.timePolicyIdList.push(timePolicy.clockClientTimePolicyId);
                        this.timePolicyCount++;
                    }
                });

                this.service.updateTimePolicyIds(this.timePolicyIdList);

                return this.service.sortedEmployees.pipe();
            }),
            switchMap((sortedEmployees) => {
                this.sortedEmps = sortedEmployees;
                this.checkEmployeesSelected();

                return this.service.geofenceOptIn.pipe();
            }),
            map((optedIn: boolean) => {
                this.optedIn = optedIn;
                this.isLoading = false;

                return true;
            }),
            catchError(() => {
                this.msg.setTemporaryMessage('Something went wrong.', this.msg.messageTypes.error);
                this.isLoading = false;

                return of(false);
            })
        );
        this.timePolicies.statusChanges
            .subscribe(_ => this.service.geofenceFormGroup.get('timePolicies').setValue(this.timePolicies.valid || null));
    }

    private createForm(): void {
        this.timePolicies = new FormGroup({
            rows: new FormArray([], Validators.required),
        });
    }

    selectedOption(idx: number): void {
        const timePolicy = this.clientTimePolicies[idx];

        timePolicy.geofenceEnabled = !timePolicy.geofenceEnabled;
        if (timePolicy.geofenceEnabled) {
            this.timePolicyCount++;
        } else {
            this.timePolicyCount--;
        }

        this.timePolicyIdList.length = 0;
        this.timePolicyList.length = 0;

        this.clientTimePolicies.forEach((tp: IClockClientTimePolicy) => {
            if (tp.geofenceEnabled) this.timePolicyIdList.push(tp.clockClientTimePolicyId);
            this.timePolicyList.push(tp);
        });

        this.checkEmployeesSelected();
    }

    private checkEmployeesSelected(): void {
        this.hasEmployeesSelected = false;

        const selectedTimeEmps = this.sortedEmps
            .filter((empList: ISortedGeofenceEmployees) => this.timePolicyIdList.includes(empList.timePolicyId));

        selectedTimeEmps.forEach((empList: ISortedGeofenceEmployees) => {
            if (!this.hasEmployeesSelected) {
                const checkLength = empList.employees
                    .filter((emp: IGeofenceEmployeeInfo) => emp.clockEmployee.geofenceEnabled).length > 0;

                this.hasEmployeesSelected = checkLength;
            }
        });

        if (this.hasEmployeesSelected && this.timePolicyCount > 0) this.timePolicies.setErrors(null);
        else this.timePolicies.setErrors({ invalid: true });
    }

    saveGeofenceOption() {
        this.saveClicked = true;

        if (this.timePolicyCount === 0 || !this.hasEmployeesSelected) return false;

        this.msg.sending(true);

        this.employeeMngmntSvc.updateTimePoliciesGeofence(this.timePolicyList).subscribe(() => {
            this.msg.sending(false);
            this.service.updateTimePolicyIds(this.timePolicyIdList);
            this.msg.setTemporarySuccessMessage('The selected time policies were saved');
        }, () => {
            this.msg.sending(false);
            this.msg.setTemporaryMessage('There was an issue with saving the selected time policies', this.msg.messageTypes.error);
        });
    }

}
