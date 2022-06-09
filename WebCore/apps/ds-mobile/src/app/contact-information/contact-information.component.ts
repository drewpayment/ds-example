import { Component, OnInit, OnDestroy  } from '@angular/core';
import { ContactInformationService } from './contact-information.service';
import { IEmployeeContactInfo } from '@ds/employees/profile/shared/employee-contact-info.model';
import { Country } from '../../models';
import { UserInfo, UserType } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { tap, switchMap, skipWhile, distinctUntilChanged, catchError, map } from 'rxjs/operators';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { State } from '@ds/core/employee-services/models';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PhonePipe } from '@ds/core/ui/pipes/phone.pipe';
import { IActionNotAllowedRejection } from '@ajs/core/account/account.service';
import { Router } from '@angular/router';
import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';

enum PhoneNumberType {
    home,
    cell
}

@Component({
    selector: 'ds-contact-information',
    templateUrl: './contact-information.component.html',
    styleUrls: ['./contact-information.component.scss']
})
export class ContactInformationComponent implements OnInit, OnDestroy  {

    currentUser: UserInfo;
    hasEditPermissions: boolean | IActionNotAllowedRejection;
    pageTitle: string;

    form: FormGroup;
    employeeContactInfo: IEmployeeContactInfo;
    isEditing$ = new BehaviorSubject<boolean>(false);
    countries: Country[];
    states: State[];
    saveButtonText = 'Request Change';
    // isLoading = true;
    isLoading$: Observable<boolean>;
    subs: Subscription[] = [];

    constructor(
        private service: ContactInformationService,
        private accountService: AccountService,
        private fb: FormBuilder,
        private sb: MatSnackBar,
        private phonePipe: PhonePipe,
        private router: Router
    ) {}

    ngOnInit() {
        this.hasEditPermissions = false;
        this.createForm();
        this.initializeObservers();

        this.isLoading$ = this.accountService.getUserInfo()
            .pipe(
                tap(user => this.currentUser = user),
                switchMap(_ => this.accountService.getClientAccountFeature(this.currentUser.lastClientId || this.currentUser.clientId, Features.EmployeeChangeRequests)),
                switchMap(employeeChangeRequestfeature => {
                    this.hasEditPermissions = _.isEmpty(employeeChangeRequestfeature);
                    return this.service.getEmployeeContactInfo(this.currentUser.employeeId);
                }),
                map(result => {
                    this.employeeContactInfo = _.cloneDeep(result);
                    this.patchForm(this.employeeContactInfo);
                    this.pageTitle = 'Edit Employee Contact';
                    return false;
                }, err => {
                    // if (err?.error?.errors?.length) {
                    if (err.error && err.error.errors && err.error.errors.length) {
                        this.sb.open(err.error.errors[0].msg, 'dismiss', { duration: 5000 });
                    } else {
                        this.sb.open(err.message, 'dismiss', { duration: 5000 });
                    }

                    // do something with API errors?
                    // this makes sure that we never try to render the form with
                    // unusable data on the page to the user
                    return false;
                })
            );
        // this.accountService.getUserInfo().subscribe((user: UserInfo) => {
        //    this.currentUser = user;
        //    this.accountService.canPerformActions("Employee.EmployeeUpdate").subscribe(x => {
        //        if (x === true) {
        //            this.hasEditPermissions = true;
        //        }

        //        this.service.getEmployeeContactInfo(this.currentUser.employeeId).subscribe(result => {
        //            this.employeeContactInfo = _.cloneDeep(result);
        //            this.patchForm(this.employeeContactInfo);
        //            this.pageTitle = `Edit Employee Contact`;
        //            this.isLoading = false;
        //        });
        //    });
        //});

        this.subs.push(this.service.getCountries().subscribe(countries => this.countries = countries));
    }

    ngOnDestroy() {
        this.subs.forEach(s => s.unsubscribe());
    }

    saveEmployeeContact() {
        this.form.markAllAsTouched();
        if (this.form.invalid) return;

        const dto = this.prepareModel();
        if(dto.countryId <= 0) {
            dto.countryId = 1; // Just assume United States.
        }
        this.subs.push(
            this.service.updateEmployeeContactInfo(dto, (this.hasEditPermissions as boolean) === true).subscribe((data) => {
                const successMessage = ((this.hasEditPermissions as boolean) === true) ? 'Successfully updated the changes.' : 'Successfully submitted change request.';
                this.sb.open(successMessage, 'dismiss', { duration: 2000 });
                this.router.navigateByUrl('/profile');
            }, err => {
                if (err.error && err.error.errors && err.error.errors.length) {
                    this.sb.open(err.error.errors[0].msg, 'dismiss', { duration: 5000 });
                } else {
                    this.sb.open(err.message, 'dismiss', { duration: 5000 });
                }
            }));
    }

    private createForm(): void {
        this.form = this.fb.group({
            firstName: this.fb.control('', [Validators.required, Validators.maxLength(25)]),
            middleInitial: this.fb.control('', [Validators.maxLength(1)]),
            lastName: this.fb.control('', [Validators.required, Validators.maxLength(25)]),
            addressLine1: this.fb.control('', [Validators.required, Validators.maxLength(50)]),
            addressLine2: this.fb.control('', [Validators.maxLength(50)]),
            city: this.fb.control('', [Validators.required, Validators.maxLength(25)]),
            state: this.fb.control('', [Validators.required]),
            country: this.fb.control('', [Validators.required]),
            postalCode: this.fb.control('', [Validators.required, Validators.maxLength(10)]),
            homePhone: this.fb.control('', [Validators.required, Validators.pattern('^\\d{3}-\\d{3}-\\d{4}$')]),
            cellPhone: this.fb.control('', [Validators.pattern('^\\d{3}-\\d{3}-\\d{4}$')])
        });
    }

    private patchForm(c: IEmployeeContactInfo) {
        this.form.patchValue({
            firstName: c.firstName || '',
            middleInitial: c.middleInitial || '',
            lastName: c.lastName || '',
            addressLine1: c.addressLine1 || '',
            addressLine2: c.addressLine2 || '',
            city: c.city || '',
            country: c.countryId || '',
            state: c.stateId || '',
            postalCode: c.postalCode || '',
            homePhone: c.homePhoneNumber || '',
            cellPhone: c.cellPhoneNumber || ''
        });
    }

    private prepareModel(): IEmployeeContactInfo {
        return {
            employeeId: this.employeeContactInfo.employeeId,
            employeeNumber: this.employeeContactInfo.employeeNumber,
            firstName: this.form.value.firstName,
            middleInitial: this.form.value.middleInitial,
            lastName: this.form.value.lastName,
            addressLine1: this.form.value.addressLine1,
            addressLine2: this.form.value.addressLine2,
            city: this.form.value.city,
            stateId: this.form.value.state,
            stateName: (!this.form.value.state) ? '' : _.find(this.states, (state) => {
                return state.stateId == this.form.value.state;
            }).name || '',
            countryId: this.form.value.country,
            countryName: (!this.form.value.country) ? '' : _.find(this.countries, (country) => {
                return country.countryId == this.form.value.country;
            }).name || '',
            postalCode: this.form.value.postalCode,
            countyId: this.employeeContactInfo.countyId,
            countyName: this.employeeContactInfo.countyName,
            homePhoneNumber: this.form.value.homePhone,
            cellPhoneNumber: this.form.value.cellPhone,
            relation: this.employeeContactInfo.relation,
            emailAddress: this.employeeContactInfo.emailAddress,
            gender: this.employeeContactInfo.gender,
            birthDate: this.employeeContactInfo.birthDate,
            jobProfileId: this.employeeContactInfo.jobProfileId,
            driversLicenseExpirationDate: this.employeeContactInfo.driversLicenseExpirationDate,
            driversLicenseNumber: this.employeeContactInfo.driversLicenseNumber || '',
            driversLicenseIssuingStateId: (this.employeeContactInfo.driversLicenseIssuingStateId && this.employeeContactInfo.driversLicenseIssuingStateId > 0) ? this.employeeContactInfo.driversLicenseIssuingStateId : null,
            driversLicenseIssuingStateName: this.employeeContactInfo.driversLicenseIssuingStateName,
            noDriversLicense: this.employeeContactInfo.noDriversLicense,
            jobTitleInfoDescription: this.employeeContactInfo.jobTitleInfoDescription,
            divisionId: this.employeeContactInfo.divisionId,
            divisionName: this.employeeContactInfo.divisionName,
            departmentId: this.employeeContactInfo.departmentId,
            departmentName: this.employeeContactInfo.departmentName
        };
    }

    private initializeObservers() {
        this.subs.push(
            this.form.get('country').valueChanges
                .pipe(
                    skipWhile(v => !v),
                    distinctUntilChanged(),
                    switchMap(countryId => this.service.getStatesByCountryId(countryId))
                )
                .subscribe(s => this.states = s));
    }
}
