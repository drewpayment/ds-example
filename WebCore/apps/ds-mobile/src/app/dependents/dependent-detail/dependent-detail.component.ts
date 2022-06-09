import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeDependentsApiService } from '../../shared/employee-dependents-api.service';
import { UserInfo } from '@ds/core/shared';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { IEmployeeDependent } from '@ds/employees/profile/shared/employee-dependent.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, Observable } from 'rxjs';
import { tap, switchMap, skipWhile, distinctUntilChanged, catchError, map } from 'rxjs/operators';
import { EMPLOYEE_ACTIONS } from '@ds/core/employees/shared/employee-actions';
import { IActionNotAllowedRejection, AccountService } from '@ds/core/account.service';
import { Features } from '@ds/admin/client-statistics/shared/models/featureEnum';

@Component({
  selector: 'ds-dependent-detail',
  templateUrl: './dependent-detail.component.html',
  styleUrls: ['./dependent-detail.component.scss']
})
export class DependentDetailComponent implements OnInit, OnDestroy {
  id: number;
  clientId: number;
  employeeId: number;
  currentUser: UserInfo;
  hasEditPermissions: boolean | IActionNotAllowedRejection;
  pageTitle: string;

  form: FormGroup;
  employeeDependent: IEmployeeDependent;
  maskedNumber: string;
  age: number;
  isLoading$: Observable<boolean>;
  subs: Subscription[] = [];

  constructor(public route: ActivatedRoute,
      private api: EmployeeDependentsApiService,
      private router: Router,
      private acctService: AccountService,
      private fb: FormBuilder,
      private sb: MatSnackBar
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
                    return this.api.getDependentByDependentId(this.id);
                }),
                map(result => {
                    if (this.id) {
                        this.employeeDependent = _.cloneDeep(result);
                        this.pageTitle = `Edit Dependent`;
                    }
                    else {
                        this.employeeDependent = this.createEmptyEmployeeDependent(this.employeeId, this.clientId);
                        this.pageTitle = `Add Dependent`;
                    }
                    this.patchForm(this.employeeDependent);
                    this.CalculateAge();
                    return false;
                }, err => {
                    //if (err?.error?.errors?.length) {
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

    private prepareModel(): IEmployeeDependent {
        return {
            employeeDependentId: this.employeeDependent != null ? this.employeeDependent.employeeDependentId : null,
            clientId: this.employeeDependent.clientId,
            firstName: this.form.value.firstName,
            middleInitial: this.employeeDependent.middleInitial,
            lastName: this.form.value.lastName,
            employeeId: this.employeeDependent.employeeId,
            unmaskedSocialSecurityNumber: this.form.value.socialSecurityNumber,
            maskedSocialSecurityNumber: this.form.value.socialSecurityNumber,
            relationship: this.form.value.relationship,
            gender: this.form.value.gender,
            birthDate: this.form.value.birthDate,
            isAStudent: this.form.value.isAStudent ? this.form.value.isAStudent : false,
            hasADisability: this.form.value.hasADisability ? this.form.value.hasADisability : false,
            tobaccoUser: this.form.value.tobaccoUser ? this.form.value.tobaccoUser : false,
            comments: this.form.value.comments,

            insertStatus: this.employeeDependent.insertStatus,
            lastModifiedByDescription: this.employeeDependent.lastModifiedByDescription,
            lastModifiedDate: this.employeeDependent.lastModifiedDate,
            isSelected: this.employeeDependent.isSelected,
            primaryCarePhysician: this.employeeDependent.primaryCarePhysician,
            hasPcp: this.employeeDependent.hasPcp,
            employeeDependentsRelationshipId: this.employeeDependent.employeeDependentsRelationshipId,
            isChild: this.employeeDependent.isChild,
            isSpouse: this.employeeDependent.isSpouse,
            isInactive: this.employeeDependent.isInactive,
            inactiveDate: this.employeeDependent.inactiveDate
        };
    }

    private createEmptyEmployeeDependent(employeeId: number, clientId: number): IEmployeeDependent {
        return {
            employeeDependentId: 0,
            clientId: clientId,
            firstName: null,
            middleInitial: null,
            lastName: null,
            employeeId: employeeId,
            unmaskedSocialSecurityNumber: null,
            maskedSocialSecurityNumber: null,
            relationship: null,
            gender: null,
            comments: null,
            birthDate: null,
            insertStatus: 0,
            lastModifiedByDescription: '0',
            lastModifiedDate: new Date(),
            isAStudent: false,
            hasADisability: false,
            tobaccoUser: false,
            isSelected: false,
            primaryCarePhysician: null,
            hasPcp: false,
            employeeDependentsRelationshipId: null,
            isChild: false,
            isSpouse: false,
            isInactive: false,
            inactiveDate: null
        }
    }

    saveDependent() {
        this.form.markAllAsTouched();
        if (this.form.invalid) return;
        const dto = this.prepareModel();
        this.subs.push(
            this.api.updateEmployeeDependent(dto, (this.hasEditPermissions as boolean) === true).subscribe((data) => {
                let successMessage = (this.hasEditPermissions as boolean) === true ? "Successfully updated the changes." : "Successfully submitted change request.";
                  this.sb.open(successMessage, 'dismiss', { duration: 2000 });
                  this.router.navigateByUrl('/profile/dependents');
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
            lastName: this.fb.control('', [Validators.required, Validators.maxLength(25)]),
            relationship: this.fb.control('', [Validators.required, Validators.maxLength(20)]),
            socialSecurityNumber: this.fb.control('', [Validators.required, Validators.maxLength(11), , this.ssnValidator]),
            gender: this.fb.control('', [Validators.required]),
            birthDate: this.fb.control('', [Validators.required]),
            isAStudent: this.fb.control(''),
            hasADisability: this.fb.control(''),
            tobaccoUser: this.fb.control(''),
            comments: this.fb.control('', [Validators.maxLength(50)]),
            age: this.fb.control({ value: '', disabled: true }, [Validators.required])
        });
    }

    private patchForm(d: IEmployeeDependent) {
        this.form.patchValue({
            firstName: d.firstName || '',
            lastName: d.lastName || '',
            relationship: d.relationship || '',
            socialSecurityNumber: d.unmaskedSocialSecurityNumber || '',
            gender: d.gender || '',
            birthDate: d.birthDate || '',
            isAStudent: d.isAStudent || '',
            hasADisability: d.hasADisability || '',
            tobaccoUser: d.tobaccoUser || '',
            comments: d.comments || '',
            age: '',
        });
    }

  CalculateAge(): void {
    let date;
    if (this.form.value.birthDate == null) {
      date = this.employeeDependent.birthDate;
    } else date = this.form.value.birthDate;
    if (date) {
      const timeDiff = Math.abs(Date.now() - new Date(date).getTime());
      this.age = Math.floor(timeDiff / (1000 * 3600 * 24) / 365.25);

      this.form.patchValue({age:this.age});
    }
  }

    ssnValidator(ssn): any {
        if (ssn.pristine) {
        return null;
        }
        const SSN_REGEXP = /^(?!219-09-9999|078-05-1120)(?!666|000|9\d{2})\d{3}-(?!00)\d{2}-(?!0{4})\d{4}$/;
        ssn.markAsTouched();
        if (SSN_REGEXP.test(ssn.value)) {
        return null;
        }
        return {
        invalidSsn: true
        };
    }
}
