import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { GeofenceManagementService } from '../geofence-management.service';
import { map, switchMap, catchError } from 'rxjs/operators';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from '@ds/core/shared';
import { EmployeeLaborManagementService } from '../../services/employee-labor-management.service';
import { IClockClientTimePolicy } from '@ajs/labor/models';
import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';
import { ClientService } from '@ds/core/clients/shared';
import { ClockService } from '@ds/core/employee-services/clock.service';

@Component({
    selector: 'ds-geofence-admin',
    templateUrl: './geofence-management-admin.component.html',
    styleUrls: ['./geofence-management-admin.component.scss']
})

export class GeofenceAdminComponent implements OnInit {
    isLoading = true;
    formInit: Observable<any>;
    loggedInUser: UserInfo;
    showGetStarted = true;
    optedIn = false;

    constructor(
        private service: GeofenceManagementService,
        private msg: DsMsgService,
        private acctSvc: AccountService,
        private employeeMngmntSvc: EmployeeLaborManagementService,
        private clientSvc: ClientService,
        private clockSvc: ClockService,
    ) { }

    ngOnInit() {
        this.initializeListener();
    }

    private initializeListener(): void {
        this.formInit = this.acctSvc.getUserInfo().pipe(
            map(userInfo => this.loggedInUser = userInfo),
            switchMap(_ => this.employeeMngmntSvc.getClientTimePolicies(this.loggedInUser.clientId).pipe(
                catchError(err => {
                    this.msg.setTemporaryMessage(err.error.message, this.msg.messageTypes.error);
                    this.isLoading = false;

                    return of([]);
                }),
            )),
            switchMap((timePolicies: IClockClientTimePolicy[]) => {
                const timePolicyIdList = [];
                const allTimePolicyIds = [];
                timePolicies.forEach(timePolicy => {
                    if (timePolicy.geofenceEnabled) {
                        timePolicyIdList.push(timePolicy.clockClientTimePolicyId);
                        this.showGetStarted = false;
                    }
                    allTimePolicyIds.push(timePolicy.clockClientTimePolicyId);
                });

                this.service.updateTimePolicyIds(timePolicyIdList);
                this.service.updateTimePolicies(timePolicies);

                return this.clockSvc.getEmployeesByTimePolicy(allTimePolicyIds).pipe(
                    catchError(() => {
                        this.isLoading = false;
                        this.msg.setTemporaryMessage('Something went wrong.', this.msg.messageTypes.error);
                        return of([]);
                    }),
                );
            }),
            switchMap(employees => {
                this.service.updateSortedEmployees(employees);

                return this.clientSvc.getClientAccountFeatureByFeatureId(this.loggedInUser.clientId, Features.Geofencing).pipe(
                    catchError(() => {
                        this.msg.setTemporaryMessage('Something went wrong.', this.msg.messageTypes.error);
                        this.isLoading = false;

                        return of({});
                    }),
                );
            }),
            map(res => {
                this.optedIn = res != null;
                this.service.updateGeofenceOptIn(this.optedIn);
                this.isLoading = false;

                return true;
            }),
            catchError(() => {
                this.msg.setTemporaryMessage('Something went wrong.', this.msg.messageTypes.error);
                this.isLoading = false;

                return of(false);
            })
        );
    }

}
