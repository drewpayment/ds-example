import { IEmergencyContact } from '@ds/employees/profile/shared/emergency-contact.model';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmergencyContactsApiService } from '../shared/emergency-contacts-api.service';
import { UserInfo } from '@ds/core/shared';
import { AccountService, IActionNotAllowedRejection } from '@ds/core/account.service';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PhonePipe } from '@ds/core/ui/pipes/phone.pipe';
import { Subscription, Observable } from 'rxjs';
import { tap, switchMap, skipWhile, distinctUntilChanged, catchError, map } from 'rxjs/operators';
import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';

enum PhoneNumberType {
    homePhone,
    workPhone,
    mobilePhone
}

@Component({
    selector: 'ds-update-emergency-contact',
    templateUrl: './update-emergency-contact.component.html',
    styleUrls: ['./update-emergency-contact.component.scss']
})
export class UpdateEmergencyContactComponent implements OnInit, OnDestroy {
    id: number;
    clientId: number;
    employeeId: number;
    currentUser: UserInfo;
    hasEditPermissions: boolean | IActionNotAllowedRejection;
    pageTitle: string;

    form: FormGroup;
    contact: IEmergencyContact;

    isLoading$: Observable<boolean>;
    subs: Subscription[] = [];

    constructor(public route: ActivatedRoute,
        private api: EmergencyContactsApiService,
        private router: Router,
        private acctService: AccountService,
        private sb: MatSnackBar,
        private phonePipe: PhonePipe,
        private fb: FormBuilder
    ) { }

    ngOnInit() {
        this.hasEditPermissions = false;
        this.createForm();
        this.isLoading$ = this.route.params
            .pipe(
            tap(params => this.id = +params['id']),
            switchMap(_ => this.acctService.getUserInfo()),
            tap(user => {
                this.currentUser = user;
                this.employeeId = this.currentUser.employeeId;
                this.clientId = this.currentUser.lastClientId || this.currentUser.clientId;
            }),
            switchMap(_ => this.acctService.getClientAccountFeature(this.currentUser.lastClientId || this.currentUser.clientId, Features.EmployeeChangeRequests)),
                switchMap(employeeChangeRequestfeature => {
                    this.hasEditPermissions = _.isEmpty(employeeChangeRequestfeature);
                    return this.api.getEmergencyContactByContactId(this.id);
                }),
            map(result => {
                    if (this.id) {
                        this.contact = _.cloneDeep(result);
                        this.pageTitle = `Edit Emergency Contact`;
                    } else {
                        this.contact = this.createEmptyEmergencyContact(this.employeeId, this.clientId);
                        this.pageTitle = `Add Emergency Contact`;
                    }
                    this.patchForm(this.contact);
                    return false;
                }, err => {
                    // if (err?.error?.errors?.length) {
                    if (err.error && err.error.errors && err.error.errors.length) {
                        this.sb.open(err.error.errors[0].msg, 'dismiss', { duration: 5000 });
                    } else {
                        this.sb.open(err.message, 'dismiss', { duration: 5000 });
                    }
                    return false;
                })
            );
    }

    ngOnDestroy() {
        this.subs.forEach(s => s.unsubscribe());
    }

    private createForm(): void {
        this.form = this.fb.group({
            firstName: this.fb.control('', [Validators.required, Validators.maxLength(25)]),
            lastName: this.fb.control('', [Validators.required, Validators.maxLength(25)]),
            relationship: this.fb.control('', [Validators.required, Validators.maxLength(20)]),
            homePhone: this.fb.control('', [Validators.pattern('^\\d{3}-\\d{3}-\\d{4}$')]),
            workPhone: this.fb.control('', [Validators.pattern('^\\d{3}-\\d{3}-\\d{4}$')]),
            mobilePhone: this.fb.control('', [Validators.pattern('^\\d{3}-\\d{3}-\\d{4}$')]),
            email: this.fb.control('', [Validators.required, Validators.maxLength(120), Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')])
        });
    }

    private patchForm(c: IEmergencyContact) {
        this.form.patchValue({
            firstName: c.firstName || '',
            lastName: c.lastName || '',
            relationship: c.relation || '',
            homePhone: c.homePhoneNumber || '',
            mobilePhone: c.cellPhoneNumber || '',
            email: c.emailAddress || ''
        });
    }

    private prepareModel(): IEmergencyContact {
        return {
            insertApproved: this.contact.insertApproved,
            employeeEmergencyContactId: this.contact != null ? this.contact.employeeEmergencyContactId : null,
            clientId: this.contact.clientId,
            employeeId: this.contact.employeeId,

            firstName: this.form.value.firstName,
            lastName: this.form.value.lastName,
            relation: this.form.value.relationship,
            homePhoneNumber: this.form.value.homePhone,
            cellPhoneNumber: this.form.value.mobilePhone,
            emailAddress: this.form.value.email,

            
        };
    }

    private createEmptyEmergencyContact(employeeId: number, clientId: number): IEmergencyContact {
        return {
            employeeEmergencyContactId: 0,
            employeeId: employeeId,
            clientId: clientId,
            homePhoneNumber: null,
            cellPhoneNumber: null,
            relation: null,
            emailAddress: null,
            insertApproved: 0,
            firstName: null,
            lastName: null
        };
    }

    checkForAnyOnePhoneNumber() {
        const homeCtrl = this.form.get('homePhone');
        const workCtrl = this.form.get('workPhone');
        const mobileCtrl = this.form.get('mobilePhone');

        homeCtrl.reset(homeCtrl.value, { emitEvent: false });
        workCtrl.reset(workCtrl.value, { emitEvent: false });
        mobileCtrl.reset(mobileCtrl.value, { emitEvent: false });

        if (!this.form.value.homePhone && !this.form.value.workPhone && !this.form.value.mobilePhone) {
            homeCtrl.setErrors({ oneRequired: true });
            workCtrl.setErrors({ oneRequired: true });
            mobileCtrl.setErrors({ oneRequired: true });
            this.form.markAllAsTouched();
            return;
        }
    }

    saveContact() {
        this.form.markAllAsTouched();
        this.checkForAnyOnePhoneNumber();
        if (this.form.invalid) return;

        const dto = this.prepareModel();
        this.subs.push(
        this.api.updateEmergencyContact(dto, (this.hasEditPermissions as boolean) === true).subscribe((data) => {
            const successMessage = (this.hasEditPermissions as boolean) === true ? 'Successfully updated the changes.' : 'Successfully submitted change request.';
            this.sb.open(successMessage, 'dismiss', { duration: 2000 });
            this.router.navigateByUrl('/profile/emergency-contacts');
        }, err => {
            if (err.error && err.error.errors && err.error.errors.length) {
                this.sb.open(err.error.errors[0].msg, 'dismiss', { duration: 5000 });
            } else {
                this.sb.open(err.message, 'dismiss', { duration: 5000 });
            }
        }));
    }
}
