import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormArray,
  AbstractControl,
  FormControl,
} from '@angular/forms';
import { IEmployeeSearchResult } from '@ajs/employee/search/shared/models';
import {
  throwError,
  merge,
  iif,
  concat,
  defer,
  of,
  forkJoin,
  Subject,
} from 'rxjs';
import {
  flatMap,
  map,
  tap,
  takeUntil,
  switchMap,
  scan,
  finalize,
  filter,
  debounceTime,
} from 'rxjs/operators';
import { IContactSearchResult, IContact } from '@ds/core/contacts';
import {
  ValidateDatesWhenSourcesValueChange,
  SetValueWhenSourceGainsValue,
} from '@ds/performance/shared/shared-performance-fns';
import {
  MIN_DATEPICKER_VALIDATOR,
  MAX_DATEPICKER_VALIDATOR,
  datepicker_validator_builder_default,
} from '@ds/core/ui/forms/datepicker-validator/datepicker-validators';
import {
  findMinDate,
  findMaxDate,
} from '@ds/core/date-comparison/date-comparison';
import * as moment from 'moment';
import { Maybe } from '@ds/core/shared/Maybe';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { AccountService } from '@ds/core/account.service';
import { IReviewSearchOptions } from '@ds/performance/performance-manager';
import { IReview, IReviewWithEmployees } from '@ds/performance/reviews';
import {
  IReviewProfileBasic,
  IReviewProfileSetup,
} from '@ds/performance/review-profiles';
import { ReviewsService } from '@ds/performance/reviews/shared/reviews.service';
import { ReviewProfilesApiService } from '@ds/performance/review-profiles/review-profiles-api.service';
import { ReviewPolicyApiService } from '@ds/performance/review-policy/review-policy-api.service';
import { EvaluationRoleType, IEvaluation } from '@ds/performance/evaluations';
import {
  IReviewTemplate,
  SortByName,
  GetReviewTemplateName,
} from '@ds/core/groups/shared/review-template.model';
import { ReferenceDate } from '@ds/performance/review-policy/shared/schedule-type.enum';
import { debounce } from 'lodash';

interface DialogData {
  options: IReviewSearchOptions;
  review: IReview;
  employees: IEmployeeSearchResult[];
  supervisors: IContact[];
  totalEmps: number;
}

const defaultFormVal = '';

@Component({
  selector: 'ds-multi-review-edit-dialog',
  templateUrl: './multi-review-edit-dialog.component.html',
  styleUrls: ['./multi-review-edit-dialog.component.scss'],
})
export class MultiReviewEditDialogComponent implements OnInit, OnDestroy {
  ngOnDestroy(): void {
    this.unsubscriber.next();
  }

  form: FormGroup;
  formSubmitted: boolean = false;
  options: IReviewSearchOptions;
  review: IReview;
  reviewTemplates: IReviewTemplate[];
  reviewProfiles: IReviewProfileBasic[] = [];
  employees: IEmployeeSearchResult[] = [];
  employeeCount: number = 0;
  ownersResponse: IContactSearchResult;
  owners: IContact[];
  supervisors: IContact[] = [];
  selectedProfileSetup: IReviewProfileSetup;
  reviewProfileHasEvaluations: boolean = false;
  reviewProfileHasPayrollRequests: boolean = false;
  reviewProfileHasMeeting: boolean = false;
  selectedReviewTemplate: IReviewTemplate;
  selectedReviewTemplateDetail: IReviewTemplate = {} as IReviewTemplate;
  dateOfHireForm: FormGroup;
  unsubscriber: Subject<any> = new Subject();
  readonly momentFormatString = 'MM/DD/YYYY';
  convertToMoment = convertToMoment;
  totalEmps: number;

  private readonly minValIsRPStartDate = this.minDateValidator(
    () => this.form.controls['processStartDate'].value,
    defaultFormVal
  );
  private readonly maxValIsRPDueDate = this.maxDateValidator(
    () => this.form.controls['processEndDate'].value,
    defaultFormVal
  );
  private readonly evalDueDateValidators = [
    Validators.required,
    this.minDateValidator(() => this.evalStartDate.value, defaultFormVal),
    this.maxValIsRPDueDate
  ];
  private readonly evalStartDateValidators = [
    Validators.required,
    this.maxDateValidator(() => this.evalEndDate.value, defaultFormVal),
    this.maxValIsRPDueDate
  ];
  private readonly maxDate = 8639968460000000;
  private readonly minDate = -8639968460000000;

  defaultMaxMoment = moment(new Date(this.maxDate));
  defaultMinMoment = moment(new Date(this.minDate));

  get processStartDate(): FormControl {
    return new Maybe(this.form)
      .map((x) => x.controls['processStartDate'] as FormControl)
      .valueOr(null);
  }

  get processEndDate(): FormControl {
    return new Maybe(this.form)
      .map((x) => x.controls['processEndDate'] as FormControl)
      .valueOr(null);
  }

  get evalStartDate(): FormControl {
    return new Maybe(this.form)
      .map((x) => x.controls['evalStartDate'] as FormControl)
      .valueOr(null);
  }

  get evalEndDate(): FormControl {
    return new Maybe(this.form)
      .map((x) => x.controls['evalEndDate'] as FormControl)
      .valueOr(null);
  }

  get evaluations(): FormArray {
    return new Maybe(this.form)
      .map((x) => x.get('evaluations') as FormArray)
      .valueOr(null);
  }

  get ReferenceDate() {
    return ReferenceDate;
  }

  constructor(
    private ref: MatDialogRef<MultiReviewEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: DialogData,
    private reviewService: ReviewsService,
    private fb: FormBuilder,
    private profileService: ReviewProfilesApiService,
    private reviewPolicyService: ReviewPolicyApiService,
    @Inject(MAX_DATEPICKER_VALIDATOR)
    private maxDateValidator: datepicker_validator_builder_default,
    @Inject(MIN_DATEPICKER_VALIDATOR)
    private minDateValidator: datepicker_validator_builder_default,
    private acctSvc: AccountService
  ) {
    this.dateOfHireForm = fb.group({
      owner: fb.control(null),
      supervisor: fb.control(-1),
    });
  }

  ngOnInit() {
    this.options = this.data.options || {};
    this.review = this.data.review || ({} as IReview);
    this.employees = this.data.employees || [];
    this.employeeCount = this.employees.length;
    this.supervisors = this.data.supervisors || [];
    this.totalEmps = this.data.totalEmps || 0;

    this.acctSvc.getUserInfo()
      .pipe(
        switchMap(x => this.reviewPolicyService.getReviewTemplatesByClientId(x.selectedClientId(), true, false)),
        map(x => SortByName(GetReviewTemplateName, x)),

      );
    //TODO clean up this stream
    concat(
      this.acctSvc
        .PassUserInfoToRequest((x) =>
          this.reviewPolicyService.getReviewTemplatesByClientId(
            x.selectedClientId(),
            true,
            false
          )
        )
        .pipe(
          map((x) => {
            return SortByName(GetReviewTemplateName, x);
          }),
          tap((x) => {
            this.reviewTemplates = x;
            this.selectedReviewTemplate = x.find(
              (r) => r.reviewTemplateId == this.options.reviewTemplateId
            );
          })
        ),
      defer(() => {
        const setupValidators$ = defer(() => {
          const controlsToReactTo = [
            this.processStartDate,
            this.processEndDate,
          ];
          const controlsToUpdate = this.evaluations.controls
            .map((x) => [
              (x as FormGroup).controls['startDate'],
              (x as FormGroup).controls['endDate'],
            ])
            .reduce((x, y) => x.concat(y), []);

            // Manually update dependent dates when updated after an initial value is added
            this.processEndDate.valueChanges.pipe(debounceTime(200)).subscribe( val => {
              this.form.get('processStartDate').updateValueAndValidity();
              this.form.get('evalStartDate').updateValueAndValidity();
              this.form.get('evalEndDate').updateValueAndValidity();
            })
            this.processStartDate.valueChanges.pipe(debounceTime(200)).subscribe( val => {
              this.form.get('processEndDate').updateValueAndValidity();
            })

          return merge(
            ValidateDatesWhenSourcesValueChange(
              controlsToReactTo,
              controlsToUpdate
            ),
      
            //...this.setEndDateWhenStartDateHasValue(this.evaluations),

            of(null)
          );
        });
        var emittedOnce = false;
        return (
          merge(
            concat(
              forkJoin(
                this.profileService
                  .getReviewProfileSetup(
                    this.selectedReviewTemplate.reviewProfileId
                  )
                  .pipe(
                    tap((setup) => {
                      this.reviewProfileHasMeeting = setup.includeReviewMeeting;
                      this.reviewProfileHasEvaluations =
                        setup.evaluations.length > 0;

                      /** TODO: this needs to be updated when merit increases are merged in */
                      this.reviewProfileHasPayrollRequests = false; // setup.meritIncreases.length > 0

                      /** remove eval array and update */
                      this.selectedProfileSetup = setup;

                      this.form = this.createForm();
                    })
                  ),
                this.getReviewTemplateDetail()
              ),
              merge(
                defer(() =>
                  this.form.controls.reviewTemplate.valueChanges.pipe(
                    scan<number, { changed: boolean; newVal: number }>(
                      (acc, curr) => {
                        acc.changed = acc.newVal !== curr;
                        acc.newVal = curr;
                        return acc;
                      },
                      {
                        changed: false,
                        newVal: this.form.controls.reviewTemplate.value,
                      }
                    ),
                    filter((x) => x.changed),
                    switchMap((val) => {
                      if (val.newVal == null) return of(null);
                      this.selectedReviewTemplate = this.reviewTemplates.find(
                        (r) => r.reviewTemplateId == val.newVal
                      );
                      return this.getReviewTemplateDetail().pipe(
                        tap(() => this.patchForm())
                      );
                    })
                  )
                ),
                setupValidators$.pipe(
                  tap(() => {
                    if (emittedOnce !== true) {
                      emittedOnce = true;
                      this.patchForm();
                    }
                  })
                )
              )
            ),
            this.reviewService
              .getReviewSetupContacts({
                haveActiveEmployee: true,
                excludeTimeClockOnly: true,
                ifSupervisorGetSubords: true,
              })
              .pipe(
                tap((resp) => {
                  this.ownersResponse = resp;
                  this.owners = resp.results.filter((x) => x.userId != null);
                })
              )
          )
            // We use takeUntil here instead of the async pipe because using
            // the async pipe for this observable causes an 'ExpressionChangedAfterItHasBeenCheckedError'
            // to be thrown when the supervisor evaluation due date is modified
            // by the user (it probably also happens with the other date inputs
            // but this problem/solution was tested with just the supervisor evaluation due date input).
            .pipe(takeUntil(this.unsubscriber))
        );
      })
    ).subscribe();

    // this.processEndDate.valueChanges.pipe(debounceTime(500)).subscribe( val => {
    //   this.form.get('processStartDate').updateValueAndValidity();
    //   console.dir(this.form);
    // })
  }

  /** Close the dialog by clicking off, clear icon or cancel */
  onNoClick() {
    this.ref.close();
  }

  /** save the form */
  save() {
    this.formSubmitted = true;
    if (this.form.invalid) return;

    this.ref.close({
      calendarYear: this.prepareModel(),
      dateOfHire: this.selectedReviewTemplateDetail,
      owner: this.dateOfHireForm.value.owner,
      supervisor: this.dateOfHireForm.value.supervisor,
    });
  }

  private getReviewTemplateDetail() {
    const getSetup$ = defer(() =>
      this.reviewPolicyService
        .getReviewTemplateDetail(this.selectedReviewTemplate.reviewTemplateId)
        .pipe(
          tap((detail) => {
            if (detail.referenceDateTypeId == ReferenceDate.DateOfHire) {
              this.form.disable();
              this.ref.updateSize('30vw');
            }
            this.selectedReviewTemplateDetail = detail;
          })
        )
    );

    return iif(
      () =>
        this.selectedReviewTemplate == null ||
        this.selectedReviewTemplate.reviewProfileId == null ||
        this.selectedReviewTemplate.reviewProfileId < 1,
      throwError('Unknown fatal error. Please try again.'),
      getSetup$
    );
  }

  private createEvaluationFormArray(): FormArray {
    /** if we don't have any evals, return an empty formarray */
    if (
      this.selectedProfileSetup == null ||
      this.selectedProfileSetup.evaluations == null ||
      this.selectedProfileSetup.evaluations.length < 1
    )
      return this.fb.array([]);

    const evals = this.selectedProfileSetup.evaluations;

    let result = this.fb.array([]);
    evals.forEach((e) => {
      if (e.role == EvaluationRoleType.Manager) {
        const supEval = this.getEvaluationByRole(EvaluationRoleType.Manager);
        result.push(
          this.fb.group({
            evaluationType: EvaluationRoleType.Manager,
            startDate: this.fb.control(supEval.startDate || defaultFormVal, {
              updateOn: 'blur',
            }),
            endDate: this.fb.control(supEval.dueDate || defaultFormVal, {
              updateOn: 'blur',
            }),
            supervisor: this.fb.control(
              supEval.evaluatedByContact || defaultFormVal,
              { updateOn: 'blur' }
            ),
          })
        );
      } else if (e.role == EvaluationRoleType.Self) {
        const empEval = this.getEvaluationByRole(EvaluationRoleType.Self);
        result.push(
          this.fb.group({
            evaluationType: EvaluationRoleType.Self,
            startDate: this.fb.control(empEval.startDate || defaultFormVal, {
              updateOn: 'blur',
            }),
            endDate: this.fb.control(empEval.dueDate || defaultFormVal, {
              updateOn: 'blur',
            }),
          })
        );
      } else if (e.role == EvaluationRoleType.Peer) {
        const peerEval = this.getEvaluationByRole(EvaluationRoleType.Peer);
        result.push(
          this.fb.group({
            evaluationType: EvaluationRoleType.Peer,
            startDate: this.fb.control(peerEval.startDate || defaultFormVal, {
              updateOn: 'blur',
            }),
            endDate: this.fb.control(peerEval.dueDate || defaultFormVal, {
              updateOn: 'blur',
            }),
          })
        );
      }
    });
    this.getControlsFromFormArray(result).forEach((x) => {
      const startDate = x[0];
      const endDate = x[1];

      const startDateMaxVal = this.maxDateValidator(
        () =>
          findMinDate(
            endDate.value,
            this.processEndDate.value,
            this.defaultMaxMoment
          ),
        defaultFormVal
      );
      const dueDateMinVal = this.minDateValidator(
        () =>
          findMaxDate(
            startDate.value,
            this.processStartDate.value,
            this.defaultMinMoment
          ),
        defaultFormVal
      );

      startDate.setValidators([
        Validators.required,
        startDateMaxVal,
        this.minValIsRPStartDate,
      ]);
      endDate.setValidators([
        Validators.required,
        dueDateMinVal,
        this.maxValIsRPDueDate,
      ]);
    });
    return result;
  }

  private createForm(): FormGroup {
    this.options = this.options || {};
    this.review = this.review || ({} as IReview);

    return this.fb.group({
      reviewTemplate: this.fb.control(
        this.review.reviewTemplateId || defaultFormVal,
        { validators: [Validators.required] }
      ),
      owner: this.fb.control(this.review.reviewOwnerUserId || defaultFormVal),
      processStartDate: this.fb.control(
        this.review.reviewProcessStartDate || defaultFormVal,
        {
          validators: Validators.compose([
            Validators.required,
            this.maxValIsRPDueDate,
          ]),
          updateOn: 'blur',
        }
      ),
      processEndDate: this.fb.control(
        this.review.reviewProcessDueDate || defaultFormVal,
        {
          validators: Validators.compose([
            Validators.required,
            this.minValIsRPStartDate,
          ]),
          updateOn: 'blur',
        }
      ),
      evalStartDate: this.fb.control(
        this.review.evaluationPeriodFromDate || defaultFormVal,
        {
          validators: Validators.compose(
            this.evalStartDateValidators
          ),
          updateOn: 'blur',
        }
      ),
      evalEndDate: this.fb.control(
        this.review.evaluationPeriodToDate || defaultFormVal,
        {
          validators: Validators.compose(this.evalDueDateValidators),
          updateOn: 'blur',
        }
      ),
      evaluations: this.createEvaluationFormArray(),
      // meritIncreaseDate: this.fb.control(this.review.) /** NOT IMPLEMENTED YET */
    });
  }

  private patchForm(rcrChanged?: boolean) {
    const changeRCR = !!rcrChanged;
    this.form.patchValue(
      {
        reviewTemplate:
          new Maybe(this.selectedReviewTemplate)
            .map((x) => x.reviewTemplateId)
            .value() || this.options.reviewTemplateId,
      },
      { emitEvent: changeRCR }
    );

    this.form.patchValue({
      // owner: this.review.reviewOwnerContact, // I don't think we have anything to patch the owner field with...
      processStartDate:
        this.selectedReviewTemplateDetail.reviewProcessStartDate,
      processEndDate: this.selectedReviewTemplateDetail.reviewProcessEndDate,
      evalStartDate: this.selectedReviewTemplateDetail.evaluationPeriodFromDate,
      evalEndDate: this.selectedReviewTemplateDetail.evaluationPeriodToDate,
    });
    this.patchEvaluationFormArrray();
  }

  private patchEvaluationFormArrray() {
    if (this.selectedReviewTemplateDetail.evaluations == null) return;

    //first check to see if we need to remove an evaluation type
    const roles = this.selectedReviewTemplateDetail.evaluations.map(
      (x) => x.role
    );
    for (var i = 0; i < this.evaluations.controls.length; i++) {
      const current = this.evaluations.controls[i] as FormGroup;
      const existing = roles.find(
        (x) => current.controls.evaluationType.value == x
      );
      if (existing == null) {
        this.evaluations.removeAt(i);
      }
    }
    this.selectedReviewTemplateDetail.evaluations.forEach((e) => {
      const patch =
        e.role == EvaluationRoleType.Manager
          ? {
              startDate: e.startDate,
              endDate: e.dueDate,
              supervisor: -1,
              evaluationType: e.role,
            }
          : {
              startDate: e.startDate,
              endDate: e.dueDate,
              evaluationType: e.role,
            };
      const existingEval = this.evaluations.controls.find(
        (x) => (x as FormGroup).controls.evaluationType.value === e.role
      );
      if (existingEval == null) {
        let group = this.fb.group({});
        for (let p in patch) {
          group.controls[p] = new FormControl(patch[p], { updateOn: 'blur' });
        }
        group.updateValueAndValidity(); // for some reason the form value doesn't update to the value of it's controls.  Calling this function seems to fix that
        // merge(...this.setEndDateWhenStartDateHasValue(new FormArray([group])))
          //  .pipe(takeUntil(this.unsubscriber))
          //  .subscribe();
        this.evaluations.controls.push(group);
      } else {
        existingEval.patchValue(patch);
      }
    });
    this.setvalidatorsOnEvaluationDateInputs(this.evaluations);
  }

  private setvalidatorsOnEvaluationDateInputs(formArray: FormArray): FormArray {
    this.getControlsFromFormArray(formArray).forEach((x) => {
      const startDate = x[0];
      const endDate = x[1];

      const startDateMaxVal = this.maxDateValidator(
        () =>
          findMinDate(
            endDate.value,
            this.processEndDate.value,
            this.defaultMaxMoment
          ),
        defaultFormVal
      );
      const dueDateMinVal = this.minDateValidator(
        () =>
          findMaxDate(
            startDate.value,
            this.processStartDate.value,
            this.defaultMinMoment
          ),
        defaultFormVal
      );

      startDate.setValidators([
        Validators.required,
        this.minValIsRPStartDate,
        startDateMaxVal,
      ]);
      endDate.setValidators([
        Validators.required,
        dueDateMinVal,
        this.maxValIsRPDueDate,
      ]);
    });
    return formArray;
  }

  private getEvaluationByRole(role: EvaluationRoleType): IEvaluation {
    if (role == null) return {} as IEvaluation;
    return this.review.evaluations != null
      ? this.review.evaluations.find((e) => e.role == role)
      : ({} as IEvaluation);
  }

  private prepareModel(): IReviewWithEmployees {
    return {
      reviewId: null,
      clientId: null,
      reviewOwnerUserId: this.form.value.owner,
      reviewProfileId: this.selectedReviewTemplate.reviewProfileId,
      // reviewPolicyId: this.form.value.reviewPolicy,
      reviewTemplateId: this.selectedReviewTemplate.reviewTemplateId,
      reviewedEmployeeIds: this.employees.map((e) => e.employeeId),
      title: this.selectedReviewTemplate.name,
      evaluationPeriodFromDate: this.form.value.evalStartDate,
      evaluationPeriodToDate: this.form.value.evalEndDate,
      reviewProcessStartDate: this.form.value.processStartDate,
      reviewProcessDueDate: this.form.value.processEndDate,
      isReviewMeetingRequired: this.reviewProfileHasMeeting,
      evaluations: this.form.value.evaluations.map((e) => {
        return {
          evaluationId: null,
          reviewId: null,
          reviewProfileId: this.selectedReviewTemplate.reviewProfileId,
          role: e.evaluationType,
          evaluatedByUserId: e.supervisor,
          currentAssignedUserId: e.supervisor,
          startDate: e.startDate,
          dueDate: e.endDate,
        } as IEvaluation;
      }) as IEvaluation[],
      evaluationGroupReviews: [],
    };
  }

  private getControlsFromFormArray(array: FormArray) {
    return new Maybe(array)
      .map((y) =>
        y.controls.map((x) => [
          (x as FormGroup).controls['startDate'],
          (x as FormGroup).controls['endDate'],
        ])
      )
      .valueOr(<AbstractControl[][]>[[]]);
  }
}
