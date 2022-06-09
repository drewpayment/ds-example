import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  OnDestroy,
} from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  ValidationErrors,
  AbstractControl,
} from '@angular/forms';
import {
  CheckPunchTypeResult,
  JobCosting,
  ClientDivision,
  ClientDepartment,
  ClientCostCenter,
  ClientEarning,
  PunchTypeItem,
  EmployeeTimePolicyRuleConfiguration,
  JobCostingAssignment,
  JobCostingWithAssociations,
} from '@ds/core/employee-services/models';
import { UserInfo, UserType, MOMENT_FORMATS } from '@ds/core/shared';
import {
  JobCostingType,
  PunchOptionType,
  ClientEarningCategory,
} from '@ds/core/employee-services/enums';
import * as moment from 'moment';
import { forkJoin, of, Observable, Observer, BehaviorSubject } from 'rxjs';
import { tap, switchMap, map, startWith } from 'rxjs/operators';
import { Moment } from 'moment';
import { ClockService } from '@ds/core/employee-services/clock.service';
import { PunchService } from '@ds/core/timeclock/punch.service';
import { environment } from '../../environments/environment';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { Router } from '@angular/router';
import { AppService } from '../app.service';
import { AccountService } from '@ds/core/account.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';

interface JobCostingUIHelper extends JobCosting {
  disabled?: boolean;
  touched?: boolean;
}

export function validateIsObject(control: AbstractControl): ValidationErrors {
  const value = control.value;

  return typeof value === 'object' && value !== null
    ? null
    : { notAnObject: true };
}

@Component({
  selector: 'ds-punch-card',
  templateUrl: './punch-card.component.html',
  styleUrls: ['./punch-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PunchCardComponent implements OnInit, OnDestroy {
  user: UserInfo;
  form: FormGroup = this.createForm();
  punchDetail: CheckPunchTypeResult;
  rules: EmployeeTimePolicyRuleConfiguration;
  private _pageLoading = true;
  get pageLoading(): boolean {
    return this._pageLoading;
  }
  set pageLoading(value: boolean) {
    this._pageLoading = coerceBooleanProperty(value);
    this.updateView();
  }
  punchTypes: PunchTypeItem[];
  jobCostingList: JobCosting[];
  filteredJobCostingList: JobCostingUIHelper[];
  hiddenJobCostingList: JobCostingUIHelper[];

  formTypes = { normal: 'normal', punches: 'punches', hours: 'hours' };
  punchView = this.formTypes.normal;
  blockPunching = false;

  // form controls
  showDepartment = false;
  showCostCenter = false;
  showEarning = false;
  _useJobCosting = false;
  get useJobCosting(): boolean {
    return this._useJobCosting;
  }
  set useJobCosting(value: boolean) {
    if (value) {
      this.showDepartment = false;
      this.showCostCenter = false;
    }
    this._useJobCosting = value;
  }

  // used to track model building on punch button click
  processingFailed = false;

  divisions: ClientDivision[];
  departments: ClientDepartment[];
  costCenters: ClientCostCenter[];
  earnings: ClientEarning[];

  private _saving = false;
  get saving(): boolean {
    return this._saving;
  }
  set saving(value: boolean) {
    this._saving = coerceBooleanProperty(value);
    this.updateView();
  }
  formControlObservers = {} as {
    [key: string]: BehaviorSubject<JobCostingAssignment[]>;
  };

  serverClockTime$: Observable<string>;

  constructor(
    private fb: FormBuilder,
    private service: ClockService,
    private sb: MatSnackBar,
    private accountService: AccountService,
    private punchService: PunchService,
    private cd: ChangeDetectorRef,
    private router: Router,
    private appService: AppService
  ) {}

  ngOnInit() {
    this.serverClockTime$ = this.accountService.getServerTime();
    this.loadResources();
  }

  ngOnDestroy() {
    if (this.accountService.serverTimeIntervalTask)
      clearInterval(this.accountService.serverTimeIntervalTask);
  }

  verifyUserRole() {
    if (
      this.user &&
      this.user.userTypeId !== UserType.employee &&
      this.user.userTypeId !== UserType.supervisor
    ) {
      this.accountService.getSiteConfigurations().subscribe((config) => {
        window.location.href = config.dsRootUrl;
      });
    }
  }

  getJobCostingCustomName(index: number) {
    return 'clientJobCostingAssignmentId' + index;
  }

  savePunch() {
    this.saving = true;

    switch (this.punchView) {
      case this.formTypes.hours:
        this.saveInputHours();
        break;
      case this.formTypes.punches:
        this.saveInputPunches();
        break;
      case this.formTypes.normal:
      default:
        this.saveNormalPunch();
        break;
    }
  }

  formatDate(
    date: Moment | string,
    momentDateFormat: string = 'MM-DD-YYYY hh:mm a'
  ): string {
    return moment(date).format(momentDateFormat);
  }

  getMinDate(): Moment {
    return moment().subtract('30', 'd');
  }

  private saveNormalPunch() {
    this.punchService.saveNormalPunch(this.form, this.jobCostingList, 'Mobile Site').subscribe(
      (detail) => {
        this.sb.open(`Punch saved successfully!`, 'dismiss', {
          duration: 2500,
        });
        this.punchDetail = detail;

        this.resetForm();
        this.saving = false;
      },
      (err) => {
        this.saving = false;
        this.sb.open(`${err.message}`, 'dismiss', { duration: 30000 });
        if (!environment.production) console.error(err);
      }
    );
  }

  private resetForm() {
    this.form.reset();
    this.clearJobCostingSelections();
    this.setPunchTypeFormControl();
    this.setDepartmentFormControl();
    this.setCostCenterFormControl();
    this.setClientEarningFormControl();

    const today = moment();
    if (this.punchView == this.formTypes.punches) {
      this.form.patchValue({
        startDate: today,
        endDate: today,
        startTime: today.format('hh:mm a'),
        endTime: today.format('hh:mm a'),
      });
    } else {
      this.form.get('startDate').setValue(today);
    }
  }

  private saveInputPunches() {
    this.punchService
      .saveInputPunches(this.form, this.jobCostingList, 'Mobile Site')
      .pipe(tap(() => (this.saving = false)))
      .subscribe(
        (detail) => {
          this.sb.open(`Punches saved successfully!`, 'dismiss', {
            duration: 2500,
          });
          this.punchDetail = detail;

          this.resetForm();
          this.saving = false;
        },
        (err) => {
          this.saving = false;
          this.sb.open(`${err.message}`, 'dismiss', { duration: 5000 });
          if (!environment.production) console.error(err);
        }
      );
  }

  private saveInputHours() {
    this.punchService
      .saveInputHours(this.form, this.jobCostingList, 'Mobile Site')
      .pipe(tap(() => (this.saving = false)))
      .subscribe(
        (detail) => {
          this.sb.open(`Hours saved successfully!`, 'dismiss', {
            duration: 2500,
          });
          this.punchDetail = detail;

          this.resetForm();
          this.saving = false;
        },
        (err) => {
          this.saving = false;
          this.sb.open(`${err.message}`, 'dismiss', { duration: 5000 });
          if (!environment.production) console.error(err);
        }
      );
  }

  private updateView() {
    this.disableHiddenFields();
    this.cd.detectChanges();
  }

  private createForm(): FormGroup {
    const now = moment();
    return this.fb.group({
      punchType: this.fb.control(''),
      division: this.fb.control(''),
      department: this.fb.control(''),
      costCenter: this.fb.control(''),
      clientEarning: this.fb.control(''),
      startDate: this.fb.control(now, [Validators.required]),
      startTime: this.fb.control(now.format('hh:mm a')),
      endDate: this.fb.control(now, []),
      endTime: this.fb.control(now.format('hh:mm a')),
      hours: this.fb.control(''),
      clientJobCostingAssignmentId1: this.fb.control(''),
      clientJobCostingAssignmentId2: this.fb.control(''),
      clientJobCostingAssignmentId3: this.fb.control(''),
      clientJobCostingAssignmentId4: this.fb.control(''),
      clientJobCostingAssignmentId5: this.fb.control(''),
      clientJobCostingAssignmentId6: this.fb.control(''),
      jobCostingEmployee: this.fb.control(''),
      jobCostingDivision: this.fb.control(''),
      jobCostingDepartment: this.fb.control(''),
      jobCostingCostCenter: this.fb.control(''),
      startPunchNote: this.fb.control(''),
      endPunchNote: this.fb.control(''),
      employeeNote: this.fb.control(''),
    });
  }

  private disableHiddenFields() {
    if (this.punchDetail.shouldHideCostCenter)
      this.form.get('costCenter').disable();
    if (this.punchDetail.shouldHideDepartment)
      this.form.get('department').disable();
    if (this.punchDetail.shouldHideEmployeeNotes)
      this.form.get('employeeNote').disable();
    if (this.punchDetail.shouldHideJobCosting) {
      this.form.get('clientJobCostingAssignmentId1').disable();
      this.form.get('clientJobCostingAssignmentId2').disable();
      this.form.get('clientJobCostingAssignmentId3').disable();
      this.form.get('clientJobCostingAssignmentId4').disable();
      this.form.get('clientJobCostingAssignmentId5').disable();
      this.form.get('clientJobCostingAssignmentId6').disable();
    }
    if (this.punchView != this.formTypes.normal) {
      this.form.get('punchType').disable();
    }
  }

  private clearJobCostingSelections() {
    if (this.punchDetail.shouldHideJobCosting) return;

    this.jobCostingList.forEach((jc) => {
      jc.selectedAvailableAssignmentId = null;
      jc.jobCostingAssignmentSelection = null;
    });

    this.filteredJobCostingList.forEach((jc) => {
      jc.selectedAvailableAssignmentId = null;
      jc.jobCostingAssignmentSelection = null;
    });
  }

  private observeJobCostingFormControls() {
    /**
     * Watch for form changes
     **/
    // this.form.get('jobCostingCostCenter').valueChanges.subscribe(val => this.jobCostingAsssignmentChangeNC(4, val, 2));
    // this.form.get('jobCostingDepartment').valueChanges.subscribe(val => this.jobCostingAsssignmentChangeNC(3, val, 2));
    // this.form.get('jobCostingDivision').valueChanges.subscribe(val => this.jobCostingAsssignmentChangeNC(2, val, 2));
    // this.form.get('jobCostingEmployee').valueChanges.subscribe(val => this.jobCostingAsssignmentChangeNC(6, val, 2));
  }

  private fetchPunchTypes() {
    return new Observable((ob: Observer<void>) => {
      this.service
        .getPunchTypeItems(this.user.userEmployeeId)
        .pipe(tap((res) => (this.punchTypes = res.items)))
        .subscribe(() => {
          this.setPunchTypeFormControl();
          ob.next();
          ob.complete();
        });
    });
  }

  private loadResources() {
    this.verifyUserRole();
    this.accountService
      .getUserInfo()
      .pipe(
        switchMap((user) => {
          this.user = user;
          return this.service.getNextPunchDetail(this.user.userEmployeeId);
        })
      )
      .subscribe(
        (detail) => {
          this.punchDetail = detail;

          if (
            this.punchDetail &&
            this.punchDetail.employeeClockConfiguration &&
            this.punchDetail.employeeClockConfiguration.clockEmployee &&
            this.punchDetail.employeeClockConfiguration.clockEmployee
              .timePolicy &&
            this.punchDetail.employeeClockConfiguration.clockEmployee.timePolicy
              .rules
          ) {
            this.rules =
              this.punchDetail.employeeClockConfiguration.clockEmployee.timePolicy.rules;
          } else {
            this.punchDetail = {} as any;
            this.rules = {} as any;
          }

          /**
           * isHidePunchType does not actually relate to the "Punch Type" field on the form.
           * It actually is related to the Client Earning based on the settings of the ClockClientRules page.
           *
           * IMPORTANT: This property is also inverted from the rest. When the option is turned on the client's clock
           * record, it shows on the punch form. So, if `isHidePunchType` is true, the intention is actually
           * to show the earning type field on the form.
           *
           * TODO: FIX THIS HOT MESS
           */
          this.showEarning = !this.rules.isHidePunchType;

          this.useJobCosting = !this.punchDetail.shouldHideJobCosting;
          this.showDepartment = !this.rules.isHideDepartment;
          this.showCostCenter = !this.rules.isHideCostCenter;

          /** SO YOU GOT SOME THINGS TO LOAD EH? */
          const pageLoadingObservables = [];

          if (this.punchDetail.allowInputPunches) {
            this.punchView = this.formTypes.punches;
          } else if (
            this.punchDetail.punchOption === PunchOptionType.NormalPunch
          ) {
            this.punchView = this.formTypes.normal;
            const punchTypeObservable = new BehaviorSubject<null>(null);
            pageLoadingObservables.push(punchTypeObservable);
            this.service
              .getPunchTypeItems(this.user.userEmployeeId)
              .pipe(tap((res) => (this.punchTypes = res.items)))
              .subscribe(() => {
                this.setPunchTypeFormControl();
                punchTypeObservable.complete();
              });
            // this.fetchPunchTypes();
          } else if (
            this.punchDetail.punchOption === PunchOptionType.InputHours
          ) {
            this.punchView = this.formTypes.hours;
          } else {
            this.blockPunching = true;
            this.form.disable();
          }

          // If user doesn't need departments, cost centers, earnings or job costing
          // stop showing the page loading animation
          if (
            !this.showDepartment &&
            !this.showCostCenter &&
            !this.useJobCosting &&
            !this.showEarning
          )
            this.pageLoading = false;

          if (this.punchView == this.formTypes.hours) {
            const ob = new BehaviorSubject<null>(null);
            pageLoadingObservables.push(ob);
            this.service
              .getEmployeeClientEarnings(this.user.selectedClientId())
              .pipe(tap((earnings) => (this.earnings = earnings)))
              .subscribe(() => {
                ob.complete();
                this.setClientEarningFormControl();
              });
          } else if (
            this.showEarning &&
            this.punchView == this.formTypes.punches
          ) {
            const ob = new BehaviorSubject<null>(null);
            pageLoadingObservables.push(ob);

            this.fetchPunchTypes().subscribe(() => ob.complete());
          }

          if (this.showDepartment) {
            const ob = new BehaviorSubject<null>(null);
            pageLoadingObservables.push(ob);
            this.service
              .getEmployeeDepartments(
                this.user.selectedClientId(),
                this.user.employeeId
              )
              .pipe(
                tap((departments) => {
                  this.departments = departments;
                  this.departments.unshift({
                    clientDepartmentId: '-1',
                    name: '',
                  } as any);
                })
              )
              .subscribe(() => {
                ob.complete();
                this.setDepartmentFormControl();
              });
          }

          if (this.showCostCenter) {
            const ob = new BehaviorSubject<null>(null);
            pageLoadingObservables.push(ob);
            this.service
              .getEmployeeCostCenters(this.user.selectedClientId())
              .pipe(
                tap((costCenters) => {
                  this.costCenters = costCenters;

                  if (this.punchDetail.isCostCenterSelectionRequired) {
                    this.costCenters.unshift({
                      clientCostCenterId: '',
                      description: 'Select Cost Center',
                    } as any);
                    this.form
                      .get('costCenter')
                      .setValidators(Validators.required);
                  } else {
                    this.costCenters.unshift({
                      clientCostCenterId: '-1',
                      description: '',
                    } as any);
                  }
                })
              )
              .subscribe(() => {
                this.setCostCenterFormControl();
                ob.complete();
              });
          }

          if (this.useJobCosting) {
            this.observeJobCostingFormControls();

            const divisionsOb = new BehaviorSubject<null>(null);
            pageLoadingObservables.push(divisionsOb);
            this.service
              .getClientDivisions(this.user.selectedClientId())
              .subscribe((divisions) => {
                this.divisions = divisions;
                this.divisions.unshift({
                  clientDivisionId: -1,
                  name: 'Use Home Division',
                } as ClientDivision);
                divisionsOb.complete();
              });

            this.service
              .getClientJobCostingList(this.user.selectedClientId())
              .subscribe((jobCostingList) => {
                this.jobCostingList = jobCostingList;
                this.hiddenJobCostingList = this.jobCostingList.filter(
                  (jc) =>
                    jc.hideOnScreen &&
                    jc.jobCostingTypeId === JobCostingType.Employee
                );
                this.filteredJobCostingList = this.jobCostingList.filter(
                  (jc) => !jc.hideOnScreen
                );

                if (
                  this.hiddenJobCostingList &&
                  this.hiddenJobCostingList.length
                ) {
                  this.loadHiddenJobCostingAvailableAssignments();
                } else {
                  this.loadJobCostingAvailableAssignments();
                }
              });
          }

          forkJoin(pageLoadingObservables).subscribe(() => {
            this.pageLoading = false;
          });
        },
        (err) => {
          var result = this.parseError(err);
          if (
            result === 'Time Policy does not exist' ||
            result.trim().toLowerCase().includes('clock.user')
          ) {
            this.appService.hasTimePolicy$.next(false);
            this.router.navigate(['paycheck']);
          }
        }
      );
  }

  private parseError(error: any): string {
    return error.error.errors != null && error.error.errors.length
      ? error.error.errors[0].msg
      : error.message;
  }

  jobCostingAssignmentSelected(
    event: MatAutocompleteSelectedEvent,
    jc: JobCosting
  ) {
    this.saving = true;
    const jca = event.option.value as JobCostingAssignment;
    this.changeJobCostingHandler(jca.clientJobCostingAssignmentId, jc);
  }

  changeJobCostingHandler(assignmentId: number, jc: JobCosting) {
    // this shouldn't ever be called if the jc is hideonscreen
    if (jc.hideOnScreen) return;
    const availableChildren = this.filteredJobCostingList.filter((j) =>
      j.parentJobCostingIds.includes(jc.clientJobCostingId)
    );
    if (availableChildren && availableChildren.length) {
      this.setJobCostingFieldsDisabledState(true);
      const idx = this.filteredJobCostingList.findIndex(
        (j) => j.clientJobCostingId == jc.clientJobCostingId
      );

      if (
        idx > -1 &&
        jc.availableAssignments &&
        jc.availableAssignments.length
      ) {
        this.filteredJobCostingList[idx].jobCostingAssignmentSelection =
          jc.availableAssignments.find(
            (j) => j.clientJobCostingAssignmentId == assignmentId
          );

        if (
          this.filteredJobCostingList[idx] &&
          this.filteredJobCostingList[idx].jobCostingAssignmentSelection
        )
          this.filteredJobCostingList[idx].selectedAvailableAssignmentId =
            this.filteredJobCostingList[
              idx
            ].jobCostingAssignmentSelection.clientJobCostingAssignmentId;
      }

      const nextChild = availableChildren[0];
      const parentJobCostingsAssignmentIdList = [];
      const parentJobCostingList = this.filteredJobCostingList.filter((item) =>
        nextChild.parentJobCostingIds.includes(item.clientJobCostingId)
      );

      if (parentJobCostingList)
        parentJobCostingList.forEach((pj) =>
          parentJobCostingsAssignmentIdList.push(
            pj.selectedAvailableAssignmentId
          )
        );

      const associatedJobCostingList: JobCostingWithAssociations[] = [
        {
          clientJobCostingId: nextChild.clientJobCostingId,
          parentJobCostingIds: nextChild.parentJobCostingIds,
          parentJobCostingAssignmentIds: parentJobCostingsAssignmentIdList,
        },
      ];

      this.service
        .getEmployeeJobCostingAssignmentLists(
          this.user.clientId,
          this.user.userEmployeeId,
          associatedJobCostingList
        )
        .pipe(
          switchMap((data) => of(data[0])),
          map((data) => {
            nextChild.availableAssignments = data.availableAssignments;
            const formControlName =
              this.resolveJobCostingFormControlName(nextChild);
            this.updateJobCostingAssignmentListObservable(
              formControlName,
              nextChild.availableAssignments
            );
            return data;
          })
        )
        .subscribe(() => {
          this.setJobCostingFieldsDisabledState(false);
          this.manageJobCostingFormControlState(nextChild);

          this.saving = false;
        });
    } else {
      this.saving = false;
    }
  }

  /**
   * Enable/Disable all job costing fields on the form programmatically. When we select one dropdown,
   * we need to do some logic, make api calls and populate the other dropdowns with refreshed data and that
   * loading process can feel wonky for the end user if they're trying to click things too fast.
   */
  private setJobCostingFieldsDisabledState(disabled: boolean) {
    this.filteredJobCostingList.forEach((fjc) => {
      const controlName = this.resolveJobCostingFormControlName(fjc);

      if (disabled)
        this.form.controls[controlName].disable({ emitEvent: false });
      else this.form.controls[controlName].enable({ emitEvent: false });
    });
  }

  private updateJobCostingAssignmentListObservable(
    formControlName: string,
    value: JobCostingAssignment[]
  ) {
    const subject = this.formControlObservers[formControlName];

    if (subject) {
      subject.next(value);

      this.form.get(formControlName).setValue(null);
      this.form
        .get(formControlName)
        .setValidators([Validators.required, validateIsObject]);

      this.cd.detectChanges();
    }
  }

  private loadHiddenJobCostingAvailableAssignments() {
    const clientJobCostingListSubject = new BehaviorSubject<null>(null);
    this.hiddenJobCostingList.forEach((jc) => {
      const associatedJobCostingList: JobCostingWithAssociations[] = [
        {
          clientJobCostingId: jc.clientJobCostingId,
          parentJobCostingIds: jc.parentJobCostingIds,
          parentJobCostingAssignmentIds: [],
        },
      ];

      const identifier = 'jobCostingEmployee';

      this.service
        .getEmployeeJobCostingAssignmentLists(
          this.user.clientId,
          this.user.userEmployeeId,
          associatedJobCostingList
        )
        .pipe(
          switchMap((data) => of(data[0])),
          switchMap((data) => {
            jc.availableAssignments = data.availableAssignments;
            // set the assigned job costing for the hidden one...
            const myEmployeeAssignment =
              data.availableAssignments.find(
                (aa) =>
                  aa.clientJobCostingAssignmentId == this.user.userEmployeeId
              ) ||
              ({
                clientJobCostingAssignmentId: this.user.userEmployeeId,
              } as JobCostingAssignment);

            jc.selectedAvailableAssignmentId =
              myEmployeeAssignment.clientJobCostingAssignmentId;
            jc.jobCostingAssignmentSelection = myEmployeeAssignment;
            this.form
              .get(identifier)
              .setValue(jc.selectedAvailableAssignmentId, { emitEvent: false });

            const jcIdx = this.jobCostingList.findIndex(
              (i) => i.clientJobCostingId == jc.clientJobCostingId
            );
            if (jcIdx) {
              this.jobCostingList[jcIdx].selectedAvailableAssignmentId =
                myEmployeeAssignment.clientJobCostingAssignmentId;
              this.jobCostingList[jcIdx].jobCostingAssignmentSelection =
                myEmployeeAssignment;
            }
            return of(data);
          })
        )
        .subscribe((data) => {
          /// do the filtered list dance here
          this.loadJobCostingAvailableAssignments();
          clientJobCostingListSubject.complete();
        });
    });
  }

  displayJobCostingAssignment(value: JobCostingAssignment): string {
    if (!value) {
      return '';
    }

    if (!value.code) {
      return value ? `${value.description}` : '';
    }
    return value ? `${value.description} (${value.code})` : '';
  }

  private getJobCostingAssignmentObservable(
    formControlName: string,
    jc: JobCosting
  ): Observable<JobCostingAssignment[]> {
    return this.form.get(formControlName).valueChanges.pipe(
      startWith(''),
      // distinctUntilChanged(),
      map((value) => {
        if (typeof value === 'string') return value;
        return null;
      }),
      map((value) => this._filter(value, jc))
    );
  }

  private loadJobCostingAvailableAssignments() {
    let enabledFirstCtrl = false;
    const jobCostingAssignmentListLoading = [];

    this.filteredJobCostingList.forEach((jc) => {
      if (!jc.hideOnScreen && !enabledFirstCtrl) {
        jc.disabled = false;
        enabledFirstCtrl = true;
      } else {
        jc.disabled = true;
      }

      const formControlName = this.resolveJobCostingFormControlName(jc);

      if (!this.formControlObservers[formControlName]) {
        this.formControlObservers[formControlName] = new BehaviorSubject<
          JobCostingAssignment[]
        >([]);
      }

      const parentJobCostingsAssignmentIdList = [];
      const parentJobCostingList = this.jobCostingList.filter((item) =>
        jc.parentJobCostingIds.includes(item.clientJobCostingId)
      );

      if (parentJobCostingList)
        parentJobCostingList.forEach((v, i, a) =>
          parentJobCostingsAssignmentIdList.push(
            v.selectedAvailableAssignmentId
          )
        );

      const associatedJobCostingList: JobCostingWithAssociations[] = [
        {
          clientJobCostingId: jc.clientJobCostingId,
          parentJobCostingIds: jc.parentJobCostingIds,
          parentJobCostingAssignmentIds:
            parentJobCostingsAssignmentIdList || [],
        },
      ];

      if (!jc.hideOnScreen && jc.jobCostingTypeId != JobCostingType.Employee) {
        const ob = new BehaviorSubject<null>(null);
        jobCostingAssignmentListLoading.push(ob);

        this.service
          .getEmployeeJobCostingAssignmentLists(
            this.user.clientId,
            this.user.userEmployeeId,
            associatedJobCostingList
          )
          .pipe(switchMap((data) => of(data[0])))
          .subscribe((data) => {
            jc.availableAssignments = data.availableAssignments;

            if (!jc.availableAssignments$) {
              jc.availableAssignments$ = this.getJobCostingAssignmentObservable(
                formControlName,
                jc
              );
            }

            this.updateJobCostingAssignmentListObservable(
              formControlName,
              jc.availableAssignments
            );

            if (
              jc.disabled &&
              (!jc.availableAssignments || !jc.availableAssignments.length)
            )
              this.form.get(formControlName).disable();
            ob.complete();
          });
      } else {
        this.manageJobCostingFormControlState(jc);
      }
    });
    forkJoin(jobCostingAssignmentListLoading).subscribe(() => {
      this.updateView();
    });
  }

  private manageJobCostingFormControlState(jc: JobCosting) {
    const formControlName = this.resolveJobCostingFormControlName(jc);

    if (
      jc.hideOnScreen ||
      (jc.availableAssignments && jc.availableAssignments.length == 0)
    ) {
      this.form.get(formControlName).disable({ emitEvent: false });
    } else if (
      jc.availableAssignments &&
      jc.availableAssignments.length &&
      !jc.hideOnScreen
    ) {
      this.form.controls[formControlName].enable({ emitEvent: false });
    } else {
      // this.form.get(formControlName).setValue('', { emitEvent: false, selfOnly: true });
    }
  }

  private _filter(value: any, jc: JobCosting): JobCostingAssignment[] {
    if (!value) return jc.availableAssignments;
    const filterValue = value.toLowerCase();
    return jc.availableAssignments.filter((a) =>
      a.description.toLowerCase().includes(filterValue)
    );
  }

  private resolveJobCostingFormControlName(jc: JobCostingUIHelper): string {
    let identifier = '';
    switch (jc.jobCostingTypeId) {
      case JobCostingType.Division:
        identifier = 'jobCostingDivision';
        break;
      case JobCostingType.Department:
        identifier = 'jobCostingDepartment';
        break;
      case JobCostingType.CostCenter:
        identifier = 'jobCostingCostCenter';
        break;
      case JobCostingType.Employee:
        identifier = 'jobCostingEmployee';
        break;
      case JobCostingType.Custom:
        jc.formName = `clientJobCostingAssignmentId${jc.sequence + 1}`;
        identifier = jc.formName;
        break;
    }

    // Update the formName property on the main JobCostingList that was returned from the API, because it is sent to the
    // PunchService when the request is built and that formName property is needed to correctly populate the API request
    const originalJcItemIndex = this.jobCostingList.findIndex(
      (jcl) => jcl.clientJobCostingId == jc.clientJobCostingId
    );
    if (originalJcItemIndex > 0) {
      this.jobCostingList[originalJcItemIndex].formName = jc.formName;
    }

    return identifier;
  }

  private setCostCenterFormControl() {
    if (!this.showCostCenter) return;

    const punches =
      this.punchDetail &&
      this.punchDetail.employeeClockConfiguration &&
      this.punchDetail.employeeClockConfiguration.clockEmployee &&
      this.punchDetail.employeeClockConfiguration.clockEmployee
        ? this.punchDetail.employeeClockConfiguration.clockEmployee.punches
        : [];

    const existingLunchPunches = punches.filter(
      (p) => p.clockClientLunchId && p.clockClientLunchId > 0
    );
    const hasTwoLunchPunches =
      existingLunchPunches &&
      existingLunchPunches.length &&
      existingLunchPunches.length % 2 === 0;

    if (
      !this.punchDetail.isFirstPunchOfDay &&
      this.punchDetail.lastOutCostCenterId &&
      this.punchDetail.homeCostCenterId !=
        this.punchDetail.lastOutCostCenterId &&
      (this.punchDetail.isOutPunch ||
        (!hasTwoLunchPunches &&
          existingLunchPunches != null &&
          existingLunchPunches.length > 0))
    ) {
      this.form
        .get('costCenter')
        .setValue(this.punchDetail.lastOutCostCenterId);
    } else if (
      this.punchDetail.isCostCenterSelectionRequired &&
      this.punchDetail.homeCostCenterId == null
    ) {
      this.form.get('costCenter').setValue('');
    } else if (this.costCenters && this.costCenters.length) {
      this.form.get('costCenter').setValue(this.punchDetail.homeCostCenterId);
      // this.form.get('costCenter').setValue(this.costCenters[0].clientCostCenterId);
    }
  }

  private setPunchTypeFormControl() {
    if (this.punchView == this.formTypes.hours) return;
    const fc = this.form.controls['punchType'];

    const lunchPunchTypeId = this.punchService.qualifyLunchPunchType(
      this.punchDetail
    );

    if (this.punchDetail.punchTypeId) {
      fc.setValue(this.punchDetail.punchTypeId);
    } else if (lunchPunchTypeId) {
      fc.setValue(lunchPunchTypeId);
    } else if (this.punchTypes && this.punchTypes.length) {
      const defaultPunchType = this.punchTypes.find((p) => p.isDefault);
      if (defaultPunchType) fc.setValue(defaultPunchType.id);
      else if (lunchPunchTypeId) fc.setValue(lunchPunchTypeId);
    }

    if (this.punchDetail.shouldDisablePunchTypeSelection) fc.disable();
    else fc.enable();
  }

  private setDepartmentFormControl() {
    if (!this.showDepartment) return;

    const punches =
      this.punchDetail &&
      this.punchDetail.employeeClockConfiguration &&
      this.punchDetail.employeeClockConfiguration.clockEmployee &&
      this.punchDetail.employeeClockConfiguration.clockEmployee
        ? this.punchDetail.employeeClockConfiguration.clockEmployee.punches
        : [];

    const existingLunchPunches = punches.filter(
      (p) => p.clockClientLunchId && p.clockClientLunchId > 0
    );
    const hasTwoLunchPunches =
      existingLunchPunches &&
      existingLunchPunches.length &&
      existingLunchPunches.length % 2 === 0;

    let tmpHomeDepartmentId: number = -1;

    if (
      !this.punchDetail.isFirstPunchOfDay &&
      this.punchDetail.departmentId != null &&
      this.punchDetail.homeDepartmentId != this.punchDetail.departmentId &&
      (this.punchDetail.isOutPunch ||
        (!hasTwoLunchPunches &&
          existingLunchPunches != null &&
          existingLunchPunches.length > 0))
    ) {
      tmpHomeDepartmentId = this.punchDetail.departmentId;
    } else if (this.punchDetail.homeDepartmentId != null) {
      tmpHomeDepartmentId = this.punchDetail.homeDepartmentId;
    }

    if (this.departments && this.departments.length)
      this.form.get('department').setValue(tmpHomeDepartmentId);
  }

  private setClientEarningFormControl() {
    if (this.punchView != this.formTypes.hours) return;
    let selectedEarning;

    const regularEarnings = this.earnings.filter(
      (e) => e.earningCategoryId == ClientEarningCategory.Regular
    );

    if (regularEarnings && regularEarnings.length) {
      selectedEarning =
        regularEarnings.find((r) => r.isDefault) || regularEarnings[0];
    }

    if (selectedEarning)
      this.form.controls['clientEarning'].setValue(
        selectedEarning.clientEarningId
      );
  }

  compareClientEarnings(v1, v2) {
    return v1 && v2 && v1 == v2;
  }

  comparePunchTypes(v1, v2) {
    return v1 && v2 && v1 == v2;
  }
}
