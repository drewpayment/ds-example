import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { GeofenceManagementService } from '../geofence-management.service';
import { ClientService } from '@ds/core/clients/shared';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from '@ds/core/shared';
import { map, catchError, switchMap } from 'rxjs/operators';
import { of, Observable } from 'rxjs';
import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';
import { IClockClientTimePolicy } from '@ajs/labor/models';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { EmployeeLaborManagementService } from '../../services/employee-labor-management.service';
import { ClockService } from '@ds/core/employee-services/clock.service';

/**
 * @title Stepper overview
 */
@Component({
  selector: 'ds-geofence-management-stepper',
  templateUrl: './geofence-management-stepper.component.html',
  styleUrls: ['./geofence-management-stepper.component.scss'],
})

export class GeofenceStepperComponent implements OnInit {
  isLinear = false;
  geofenceFormGroup: FormGroup;
  isLoading = true;
  formInit = new Observable<any>();
  loggedInUser: UserInfo;
  stepIndex = 0;

  constructor(
    private service: GeofenceManagementService,
    private clientSvc: ClientService,
    private acctSvc: AccountService,
    private msg: DsMsgService,
    private employeeMngmntSvc: EmployeeLaborManagementService,
    private clockSvc: ClockService,
  ) { }

  ngOnInit() {
    this.geofenceFormGroup = this.service.geofenceFormGroup;
    this.initializeListener();
  }

  private initializeListener(): void {
    this.formInit = this.acctSvc.getUserInfo().pipe(
      switchMap((user: UserInfo) => {
        this.loggedInUser = user;

        return this.service.geofenceOptIn.pipe();
      }),
      map((optedIn) => {
        if (optedIn == null) {
          this.formInit = this.employeeMngmntSvc.getClientTimePolicies(this.loggedInUser.clientId).pipe(
            switchMap((timePolicies: IClockClientTimePolicy[]) => {
              const timePolicyIdList = [];
              const allTimePolicyIds = [];

              timePolicies.forEach(timePolicy => {
                if (timePolicy.geofenceEnabled) timePolicyIdList.push(timePolicy.clockClientTimePolicyId);
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
              optedIn = res != null;
              this.isLinear = !optedIn;
              this.isLoading = false;
              this.service.updateGeofenceOptIn(optedIn);

              return true;
            }),
            catchError(() => {
              this.msg.setTemporaryMessage('Something went wrong.', this.msg.messageTypes.error);
              this.isLoading = false;

              return of(false);
            })
          );
        } else {
          this.isLoading = false;
          this.isLinear = !optedIn;
          if (optedIn) this.stepIndex = 2;

          return true;
        }
      }),
      catchError(() => {
        this.msg.setTemporaryMessage('Something went wrong.', this.msg.messageTypes.error);
        this.isLoading = false;

        return of([]);
      }),
    );
  }

}
