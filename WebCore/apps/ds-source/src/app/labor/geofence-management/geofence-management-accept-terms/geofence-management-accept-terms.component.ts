import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { GeofenceManagementService } from '../geofence-management.service';
import { GeofenceService } from '../../services/geofence.service';
import { IGeofenceAgreement } from '../../../models/geofence-agreement.model';
import { IClientNotes } from '../../../models';
import { map, switchMap, catchError } from 'rxjs/operators';
import { AccountService } from '@ds/core/account.service';
import { UserInfo, UserType } from '@ds/core/shared';
import { IGeofenceOptIn, GeofenceOptionType, IClientAgreement } from '../../../models/geofence-opt-in.model';
import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';

@Component({
    selector: 'ds-geofence-accept-terms',
    templateUrl: './geofence-management-accept-terms.component.html',
    styleUrls: ['./geofence-management-accept-terms.component.scss']
})

export class GeofenceAcceptTermsComponent implements OnInit {
    acceptTerms: FormGroup;
    isLoading = true;
    formInit: Observable<any>;
    geofenceAgreement: IGeofenceAgreement;
    loggedInUser: UserInfo;
    userAgreed: IClientAgreement;
    payrollMultiplier = 1;
    employeeCount = 0;
    optedIn = false;

    constructor(
        private geofenceApiService: GeofenceService,
        private service: GeofenceManagementService,
        private msg: DsMsgService,
        private acctSvc: AccountService
    ) { }

    ngOnInit() {
        this.createForm();
        this.formInit = this.acctSvc.getUserInfo().pipe(
            switchMap((user: UserInfo) => {
                this.loggedInUser = user;

                if (this.loggedInUser.userTypeId === UserType.systemAdmin) {
                    this.acceptTerms.get('pin').clearValidators();
                    this.acceptTerms.get('pin').updateValueAndValidity();
                }

                return this.service.employeeCount.pipe();
            }),
            switchMap((employeeCount: number) => {
                this.employeeCount = employeeCount > 0 ? employeeCount : 0;

                return this.service.payrollMultiplier.pipe();
            }),
            switchMap((multiplier: number) => {
                this.payrollMultiplier = multiplier ? multiplier : 1;

                return this.geofenceApiService.getClientAgreement(Features.Geofencing).pipe(
                    catchError(() => {
                        this.msg.setTemporaryMessage('Something went wrong.', this.msg.messageTypes.error);
                        this.isLoading = false;

                        return of({});
                    }),
                );

            }),
            switchMap((userAgreed: IClientAgreement) => {
                this.userAgreed = userAgreed;

                return this.service.geofenceOptIn.pipe();
            }),
            map((optedIn: boolean) => {
                this.optedIn = optedIn;
                if (this.optedIn) {
                    if (this.userAgreed != null) {
                        this.acceptTerms.get('firstName').setValue(this.userAgreed.firstName);
                        this.acceptTerms.get('lastName').setValue(this.userAgreed.lastName);
                    }
                    this.acceptTerms.get('firstName').disable();
                    this.acceptTerms.get('lastName').disable();
                    this.acceptTerms.get('pin').disable();
                } else {
                    if (this.loggedInUser.userTypeId === UserType.systemAdmin) {
                        this.acceptTerms.get('firstName').setValue('System');
                        this.acceptTerms.get('lastName').setValue('Admin');
                        this.acceptTerms.get('firstName').disable();
                        this.acceptTerms.get('lastName').disable();
                        this.acceptTerms.get('pin').disable();
                    } else {
                        this.acceptTerms.get('firstName').setValue(this.loggedInUser.userFirstName);
                        this.acceptTerms.get('lastName').setValue(this.loggedInUser.userLastName);
                    }
                }
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

    private createForm(): void {
        this.acceptTerms = new FormGroup({
            firstName: new FormControl(null, [Validators.required, Validators.pattern('[a-zA-Z-,.\']*')]),
            lastName: new FormControl(null, [Validators.required, Validators.pattern('[a-zA-Z-,.\']*')]),
            pin: new FormControl(null, [Validators.required, Validators.pattern('[a-zA-Z0-9]*')]),
        });
    }

    saveForm(): void {
        this.acceptTerms.markAllAsTouched();
        if (this.acceptTerms.invalid) return;

        const tmpClientNote: IClientNotes = {
            clientID: this.loggedInUser.clientId,
            clientNoteID: 0,
            clientNoteSubjectID: 0,
            remark: {
                addedBy: this.loggedInUser.userId,
                addedDate: new Date,
                description: null,
                isSystemGenerated: true,
                remarkId: 0,
            },
            remarkID: 0,
        };

        const tmpGeofenceOptIn: IGeofenceOptIn = {
            clientNotes: tmpClientNote,
            firstName: this.acceptTerms.get('firstName').value,
            lastName: this.acceptTerms.get('lastName').value,
            geofenceOptionID: GeofenceOptionType.ESSENTIAL,
            userPin: this.acceptTerms.get('pin').value,
            optIn: true,
        };

        this.geofenceApiService.clientGeofenceOptIn(tmpGeofenceOptIn).subscribe(() => {
            this.msg.setTemporarySuccessMessage('You have successfully opted into geofencing');
            this.service.updateGeofenceOptIn(true);
        }, (err) => {
            if (err.error.errors && err.error.errors.length > 0) {
                this.msg.setTemporaryMessage(err.error.errors[0].msg, this.msg.messageTypes.error);
            } else this.msg.setTemporaryMessage('Something went wrong.', this.msg.messageTypes.error);

        });
    }

}
