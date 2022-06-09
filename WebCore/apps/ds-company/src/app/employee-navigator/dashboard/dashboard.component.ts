import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import * as moment from 'moment';
import { rotateAnimation } from 'angular-animations';
import { EmployeeNavigatorSyncLog, EnSyncType, EnSyncDirectionType, EnSyncStatusType } from '@models';


const FAKE_DATA: EmployeeNavigatorSyncLog[] = [
    {
        employeeNavigatorSyncLogId: 1,
        employeeId: 56373,
        clientId: 358,
        syncType: EnSyncType.employeeDemographic,
        syncDirection: EnSyncDirectionType.fromDsToEn,
        syncStatus: EnSyncStatusType.success,
        hasRetriedSync: false,
        createdAt: moment().subtract(14, 'd'),
        modifiedBy: 152490,
        modified: moment().subtract(3, 'd')
    },
    {
        employeeNavigatorSyncLogId: 1,
        employeeId: 56373,
        clientId: 358,
        syncType: EnSyncType.deduction,
        syncDirection: EnSyncDirectionType.fromEnToDs,
        syncStatus: EnSyncStatusType.failure,
        hasRetriedSync: false,
        createdAt: moment().subtract(14, 'd'),
        modifiedBy: 152490,
        modified: moment().subtract(3, 'd')
    },
    {
        employeeNavigatorSyncLogId: 1,
        employeeId: 56373,
        clientId: 358,
        syncType: EnSyncType.employeeDemographic,
        syncDirection: EnSyncDirectionType.fromDsToEn,
        syncStatus: EnSyncStatusType.failure,
        hasRetriedSync: true,
        createdAt: moment().subtract(14, 'd'),
        modifiedBy: 152490,
        modified: moment().subtract(3, 'd')
    }
];

@Component({
    selector: 'ds-employee-navigator-dashboard',
    templateUrl: 'dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
    animations: [
        rotateAnimation,
    ]
})
export class DashboardComponent implements OnInit {

    animationState = false;
    animationWithState = false;
    tableData = FAKE_DATA;
    tableColumns = ['date', 'time', 'from', 'status'];
    tableFilters = this.createTableFilters();

    constructor(private fb: FormBuilder) { }

    ngOnInit() { }

    sendForRetry(row: EmployeeNavigatorSyncLog) {
        console.dir(row);

        setTimeout(() => {
            this.animationState = true;
            this.animationWithState = !this.animationWithState;
        }, 1);
    }

    private createTableFilters(): FormGroup {
        return this.fb.group({
            from: this.fb.control(''),
            to: this.fb.control(''),
            fromClient: this.fb.control(''),
            status: this.fb.control(''),
        });
    }
}
