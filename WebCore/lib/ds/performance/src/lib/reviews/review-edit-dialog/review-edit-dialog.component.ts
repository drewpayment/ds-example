import { Component, OnInit, Inject, ViewChild, ElementRef, Input, ChangeDetectionStrategy, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { IReview } from '../shared/review.model';
import { ReviewsService } from '../shared/reviews.service';
import { IReviewEditDialogData } from './review-edit-dialog-data.model';
import { Observable, forkJoin, concat, iif, of, combineLatest, merge, defer, Subject, zip, Subscription, empty, fromEvent } from 'rxjs';
import { catchError, map, concatMap, tap, share, switchMap, filter, withLatestFrom, scan, startWith, debounceTime, take, takeUntil } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { IReviewEditDialogResult } from './review-edit-dialog-result.model';
import { FormControl, FormGroup, FormBuilder, Validators, NgModel, AbstractControl } from '@angular/forms';
import * as _ from "lodash";
import * as moment from "moment";
import { EmployeeApiService } from '@ds/core/employees';
import { AccountService } from '@ds/core/account.service';
import { ICalendarYearForm, IFormItem, IFormItemComponent } from '@ds/performance/shared/forms/calendar-year-form/calendar-year-form.model';
import { SupervisorComp, EmployeeNamecomp, ReviewMeetingComp, EmployeeDateRangeComp } from '@ds/performance/shared/forms/calendar-year-form/calendar-year-form.component';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { InjectedAttachSaveHandler, AttachSaveHandlerToken } from '@ds/core/shared/shared-api-fn';
import { Maybe } from '@ds/core/shared/Maybe';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { ReviewProfilesApiService } from '@ds/performance/review-profiles/review-profiles-api.service';
import { EvaluationRoleType } from '@ds/performance/evaluations/shared/evaluation-role-type.enum';
import { IReviewProfileSetup } from '@ds/performance/review-profiles/shared/review-profile-setup.model';
import { IContact } from '@ds/core/contacts/shared/contact.model';
import { IReviewTemplate, SortByName, GetReviewTemplateName } from '@ds/core/groups/shared/review-template.model';
import { ReviewPolicyApiService } from '@ds/performance/review-policy/review-policy-api.service';
import { IEvaluationTemplateBase } from '@ds/performance/review-policy/shared/evaluation-template.model';
import { SortContacts } from '@ds/core/ui/pipes/sort-contacts.pipe';

@Component({
    selector: 'ds-review-edit-dialog',
    templateUrl: './review-edit-dialog.component.html',
    styleUrls: ['./review-edit-dialog.component.scss']
})
export class ReviewEditDialogComponent implements OnInit, OnDestroy {
    ngOnDestroy(): void {
        this.unsubscriber.next()
    }

    private _calendarYearFormValue: ICalendarYearForm;
    public get CalendarYearFormValue(): ICalendarYearForm {
        return this._calendarYearFormValue;
    }
    public set CalendarYearFormValue(value: ICalendarYearForm) {
        this._calendarYearFormValue = value;
    }

    @ViewChild('saveBtn', { static:true }) saveBtn: ElementRef<HTMLButtonElement>;

    unsubscriber = new Subject();
    review: IReview;
    owners$: Observable<IContact[]>;
    private readonly maxDate = 8639968460000000;
    private readonly minDate = -8639968460000000;
    reviewTemplates$: Observable<IReviewTemplate[]>;
    selectedRCR$: Observable<ICalendarYearForm>;
    autocompleteComponent$: Observable<IFormItem<SupervisorComp>>;
    employeeNameComponent$: Observable<IFormItem<EmployeeNamecomp>>;
    reviewMeetingComp: IFormItem<ReviewMeetingComp>;
    EmpDateRangeComp$: Observable<IFormItem<EmployeeDateRangeComp>>;
    setSup$: Observable<any>;
    ownerCtrl = new FormControl();
    defaultMaxMoment = moment(new Date(this.maxDate));
    defaultMinMoment = moment(new Date(this.minDate));
    submitted = false;
    readonly ReviewTemplateForm: FormGroup;

    get reviewTemplateId(){ return this.ReviewTemplateForm.controls.reviewTemplateId as FormControl; }
    get clientId() { return this.ReviewTemplateForm.controls.clientId as FormControl; }
    get instructions() { return this.ReviewTemplateForm.controls.instructions as FormControl; }
    get isGoalsWeighted() { return this.ReviewTemplateForm.controls.isGoalsWeighted as FormControl; }
    get isReviewMeetingRequired() { return this.ReviewTemplateForm.controls.isReviewMeetingRequired as FormControl; }
    get payrollRequestEffectiveDate() { return this.ReviewTemplateForm.controls.payrollRequestEffectiveDate as FormControl; }
    get reviewId() { return this.ReviewTemplateForm.controls.reviewId as FormControl; }
    get reviewProfileId() { return this.ReviewTemplateForm.controls.reviewProfileId as FormControl; }
    get reviewedEmployeeId() { return this.ReviewTemplateForm.controls.reviewedEmployeeId as FormControl; }
    get title() { return this.ReviewTemplateForm.controls.title as FormControl; }
    get supervisorCtrl() { return this.ReviewTemplateForm.controls.supervisorCtrl as FormControl; }
    get employeeUserId() { return this.ReviewTemplateForm.controls.employeeUserId as FormControl; }
    get reviewOwner() { return this.ReviewTemplateForm.controls.reviewOwner as FormControl; }

    constructor(
        private dialogRef: MatDialogRef<ReviewEditDialogComponent, IReviewEditDialogResult>,
        @Inject(MAT_DIALOG_DATA)
        private dialogData: IReviewEditDialogData,
        private reviewSvc: ReviewsService,
        private reviewPolicySvc: ReviewPolicyApiService,
        private reviewProfileSvc: ReviewProfilesApiService,
        private employeeSvc: EmployeeApiService,
        fb: FormBuilder,
        private msg: DsMsgService,
        private acctSvc: AccountService,
        @Inject(AttachSaveHandlerToken) private createSaveHandler: InjectedAttachSaveHandler) {
            this.ReviewTemplateForm = fb.group({
                reviewTemplateId: fb.control(null, {validators: Validators.required}),
                clientId: fb.control(null),
                instructions: fb.control(null),
                isGoalsWeighted: fb.control(null),
                payrollRequestEffectiveDate: fb.control(null),
                reviewId: fb.control(null),
                reviewProfileId: fb.control(null),
                reviewedEmployeeId: fb.control(null),
                title: fb.control(null),
                isViewableByEmployee: fb.control(null),
                supervisorCtrl: fb.control(null, {validators: Validators.required}),
                employeeUserId: fb.control(null),
                reviewOwner: fb.control(null)
            });
    }

    ngOnInit() {


        this.EmpDateRangeComp$ = forkJoin(this.reviewSvc.getReviewSetupContacts({
            page: 1,
            pageSize: 20,
            searchText: `${this.dialogData.employee.firstName} ${this.dialogData.employee.lastName}`,
            excludeTimeClockOnly: false,
            haveActiveEmployee: false,
            ifSupervisorGetSubords: false
        }),
            of(this.dialogData.employee.employeeId)
        ).pipe(
            map(x => x[0].results.find(y => y.employeeId === x[1])),
            filter(x => null != x),
            tap(x => this.employeeUserId.setValue(x)),
            map<IContact, IFormItem<EmployeeDateRangeComp>>(x => (
                {
                    component: EmployeeDateRangeComponent,
                    data: {
                        defaultMaxMoment: this.defaultMaxMoment,
                        defaultMinMoment: this.defaultMinMoment,
                        submitted: () => this.submitted,
                        employeeContact: x
                    }
                }
            ))
        );



        this.owners$ = this.ownerCtrl.valueChanges.pipe(
            startWith(null),
            debounceTime(500),
            switchMap((searchText: string) => this.reviewSvc.getReviewSetupContacts({ page: 1, pageSize: 75, searchText: searchText, excludeTimeClockOnly: true, haveActiveEmployee: true, ifSupervisorGetSubords: true })),
            map(searchResults => _.filter(searchResults.results, c => !!c.userId))
        );

        let employee = this.dialogData.employee;
        this.review = this.dialogData.review ? _.cloneDeep(this.dialogData.review) : {
            clientId:           employee.clientId,
            reviewId:           0,
            title:              "",
            reviewedEmployeeId: employee.employeeId,
            reviewProfileId:    null,
            reviewedEmployeeContact: {
                employeeId: employee.employeeId,
                firstName:  employee.firstName,
                lastName:   employee.lastName
            },
            evaluationGroupReviews: []
        };

        this.reviewMeetingComp = {
            component: ReviewEditMeetingComponent,
            data: {
                submitted: () => this.submitted
            }
        }

        this.autocompleteComponent$ = forkJoin(
            of(false),
            this.employeeSvc.getSupervisorsForEmployee(employee.employeeId, true, true, true),
            of(this.supervisorCtrl),
            of(() => this.submitted)).pipe(
                map(x => {
                    return <IFormItem<SupervisorComp>>{
                        data: {
                            multiple: x[0],
                            contacts: x[1],
                            inputControl: x[2],
                            submitted: x[3]
                        },
                        component: SupervisorSelectComponent
                    };
                })
            );


        const reviewMaybe = new Maybe(this.dialogData.review);

        this.reviewTemplateId.setValue(reviewMaybe.map(x => x.reviewTemplateId).value());
        this.instructions.setValue(reviewMaybe.map(x => x.instructions).value());
        this.isGoalsWeighted.setValue(reviewMaybe.map(x => x.isGoalsWeighted).value());
        this.payrollRequestEffectiveDate.setValue(reviewMaybe.map(x => x.payrollRequestEffectiveDate).value());
        this.reviewId.setValue(reviewMaybe.map(x => x.reviewId).value());
        this.reviewOwner.setValue(reviewMaybe.map(x => (<IContact>{userId: x.reviewOwnerUserId})).value());
        this.reviewProfileId.setValue(reviewMaybe.map(x => x.reviewProfileId).value());
        this.title.setValue(reviewMaybe.map(x => x.title).value());
        this.clientId.setValue(employee.clientId);
        this.reviewedEmployeeId.setValue(employee.employeeId);
        this.reviewOwner.setValue(reviewMaybe.map(x => x.reviewOwnerContact).value());


        this.reviewTemplates$ = this.acctSvc.PassUserInfoToRequest(x => this.reviewPolicySvc.getReviewTemplatesByClientId(x.selectedClientId(), true, true)).pipe(
            map(x => {
                return SortByName(GetReviewTemplateName, x);
            }),
            share());

        this.setSup$ = merge(
            this.setSupervisor(
            (employeeId) => this.employeeSvc.getSupervisorsForEmployee(employeeId, true, true, true),
            this.dialogData.employee.employeeId,
            new Maybe(this.dialogData.review),
            this.supervisorCtrl),

            this.clearReviewTemplate(this.reviewTemplates$, this.reviewTemplateId));

            const handleRCRChange$ = this.getRTAndRPStream(
                this.reviewTemplateId,
                this.dialogData.review,
                employee.employeeId,
                (reviewTemplateId, employeeId) => this.reviewPolicySvc.getReviewTemplateForEmployee(reviewTemplateId, employeeId),
                (reviewProfileId) => this.reviewProfileSvc.getReviewProfileSetup(reviewProfileId));

        const setForm$ = handleRCRChange$.pipe(
                tap(x => {
                    const reviewProfile = x[1];
                    const reviewTemplate = x[0];
                    this.instructions.setValue(reviewProfile.defaultInstructions);
                    this.payrollRequestEffectiveDate.setValue(reviewTemplate.payrollRequestEffectiveDate);
                    this.reviewProfileId.setValue(reviewProfile.reviewProfileId);
                    this.title.setValue(reviewTemplate.name);
                    this.isGoalsWeighted.setValue(new Maybe(reviewProfile.evaluations).map(z => z.some(y => y.isGoalsWeighted)).valueOr(false));
                }),
                map(x => {

                const evalTemplates = new Maybe(x[0].evaluations);
                const selfEvalTemplate = evalTemplates.map(y => y.find(z => z.role === EvaluationRoleType.Self));
                const supEvalTemplate = evalTemplates.map(y => y.find(z => z.role === EvaluationRoleType.Manager));

                    return <ICalendarYearForm>{
                        employeeEvaluationDueDate: selfEvalTemplate.map(y => y.dueDate).value(),
                        employeeEvaluationStartDate: selfEvalTemplate.map(y => y.startDate).value(),
                        supervisorEvaluationStartDate: supEvalTemplate.map(y => y.startDate).value(),
                        supervisorEvaluationDueDate: supEvalTemplate.map(y => y.dueDate).value(),
                        evaluationPeriodFromDate: x[0].evaluationPeriodFromDate,
                        evaluationPeriodToDate: x[0].evaluationPeriodToDate,
                        employeeRCRReviewProfileEvalId: new Maybe(x[1].evaluations).map(y => y.find(y => y.role == EvaluationRoleType.Self)).map(y => y.reviewProfileEvaluationId).value(),
                        supervisorRCRReviewProfileEvalId: new Maybe(x[1].evaluations).map(y => y.find(y => y.role == EvaluationRoleType.Manager)).map(y => y.reviewProfileEvaluationId).value(),
                        hasReviewMeeting: x[1].includeReviewMeeting,
                        payrollRequestDate: x[0].payrollRequestEffectiveDate,
                        reviewProcessDueDate: x[0].reviewProcessEndDate,
                        reviewProcessStartDate: x[0].reviewProcessStartDate,
                        hasSupervisorEval: new Maybe(x[1].evaluations).map(y => y.some(z => z.role === EvaluationRoleType.Manager)).valueOr(false),
                        hasEmployeeEval: new Maybe(x[1].evaluations).map(y => y.some(z => z.role === EvaluationRoleType.Self)).valueOr(false),
                        empEvalCompleteDate: new Maybe(x[2]).map(y => y.evaluations).map(y => y.find(z => z.role === EvaluationRoleType.Self)).map(y => y.completedDate).value(),
                        supEvalCompleteDate: new Maybe(x[2]).map(y => y.evaluations).map(y => y.find(z => z.role === EvaluationRoleType.Manager)).map(y => y.completedDate).value(),
                        supEvalSignatures: new Maybe(x[2]).map(y => y.evaluations).map(y => y.find(z => z.role === EvaluationRoleType.Manager)).map(y => y.signatures).value(),
                        supEvalSignatureId: new Maybe(x[2]).map(y => y.evaluations).map(y => y.find(z => z.role === EvaluationRoleType.Manager)).map(y => y.signature).value(),
                        empEvalSignatureId: new Maybe(x[2]).map(y => y.evaluations).map(y => y.find(z => z.role === EvaluationRoleType.Self)).map(y => y.signature).value(),
                        currentAssignedSupervisor: new Maybe(x[2]).map(y => y.evaluations).map(y => y.find(z => z.role === EvaluationRoleType.Manager)).map(y => y.currentAssignedUserId).value()
                    }
                }),
                share());

                this.setSupervisorCtrlInvalid(setForm$, this.supervisorCtrl, this.unsubscriber);

        this.selectedRCR$ = iif(() => this.dialogData.review == null,
            setForm$,
            merge(setForm$, defer(() => {
                const rcrSelfEval = new Maybe(this.dialogData.review.evaluations).map(y => y.find(y => y.role == EvaluationRoleType.Self));
                const rcrSupEval = new Maybe(this.dialogData.review.evaluations).map(y => y.find(y => y.role == EvaluationRoleType.Manager));

                return of(<ICalendarYearForm>{
                employeeEvaluationDueDate: rcrSelfEval.map(y => y.dueDate).value(),
                employeeEvaluationStartDate: rcrSelfEval.map(y => y.startDate).value(),
                supervisorEvaluationStartDate: rcrSupEval.map(y => y.startDate).value(),
                supervisorEvaluationDueDate: rcrSupEval.map(y => y.dueDate).value(),
                evaluationPeriodFromDate: this.dialogData.review.evaluationPeriodFromDate,
                evaluationPeriodToDate: this.dialogData.review.evaluationPeriodToDate,
                employeeEvaluationId: rcrSelfEval.map(y => y.evaluationId).value(),
                supervisorEvaluationId: rcrSupEval.map(y => y.evaluationId).value(),
                hasReviewMeeting: this.dialogData.review.isReviewMeetingRequired,
                payrollRequestDate: this.dialogData.review.payrollRequestEffectiveDate,
                reviewProcessDueDate: this.dialogData.review.reviewProcessDueDate,
                reviewProcessStartDate: this.dialogData.review.reviewProcessStartDate,
                hasEmployeeEval: rcrSelfEval.value() != null,
                hasSupervisorEval: rcrSupEval.value() != null
            });
        })));

        fromEvent(this.saveBtn.nativeElement, 'click').pipe(
            tap(() => this.submitted = true),
            filter(() => null != this.CalendarYearFormValue && this.ReviewTemplateForm.valid),
            tap(() => this.dialogRef.close({ review: this.convertFormToReview(this.CalendarYearFormValue, this.ReviewTemplateForm.value)})),
            takeUntil(this.unsubscriber)
        ).subscribe();

                // this.createSaveHandler(
                //     this.saveBtn,
                //     () => {
                //         this.supervisorCtrl.markAsTouched();
                //         this.submitted = true;
                //     },
                //     () => null != this.CalendarYearFormValue && this.ReviewTemplateForm.valid,
                //     () => this.reviewSvc.saveReview(this.convertFormToReview(this.CalendarYearFormValue, this.ReviewTemplateForm.value)),
                //     review => {
                //         this.dialogRef.close({ review: review });
                //     },
                //     this.unsubscriber,
                //     "Save review");
    }

    private setSupervisorCtrlInvalid(formData$: Observable<ICalendarYearForm>, supCtrl: AbstractControl, unsubscriber: Subject<any>): Subscription {
        return combineLatest(formData$, of(supCtrl)).pipe(map(x =>{
            if(!!!x[0].hasSupervisorEval){
                x[1].setValidators(Validators.nullValidator);
                x[1].updateValueAndValidity();
            }
        }),
        takeUntil(unsubscriber)).subscribe();
    }

    private clearReviewTemplate(reviewTemplates$: Observable<IReviewTemplate[]>, selectedRCR: FormControl): Observable<any> {
        return combineLatest(reviewTemplates$, of(selectedRCR)).pipe(
            map(x => ({allTemplates: x[0], selectedTemplate: x[1]})),
            tap(x => {
                x.selectedTemplate.setValue(new Maybe(x.allTemplates).map(z => z.find(y => y.reviewTemplateId === +x.selectedTemplate.value)).map(z => z.reviewTemplateId).value())
            })
        );
    }

    /**
     * Gets the supervisor contact information and stores it in the provided FormControl
     * @param getSups A function that gets all of the supervisors for the employee
     * @param employeeId The id of the employee we want to get the supervisors for
     * @param savedReview The saved review the user is editing
     * @param controlToSet The FormControl we want to store the found Supervisor Contact
     */
    private setSupervisor(
        getSups: (employeeId) => Observable<IContact[]>,
        employeeId: number,
        savedReview: Maybe<IReview>,
        controlToSet: FormControl): Observable<any> {

        const supervisorId = savedReview.map(x => x.evaluations).map(x => x.find(y => y.role === EvaluationRoleType.Manager)).map(x => x.evaluatedByUserId);

        return forkJoin(getSups(employeeId), of(supervisorId), of(controlToSet)).pipe(
            map(x => ({ supervisors: x[0], selectedSup: x[1], controlToSet: x[2] })),
            tap(x => x.controlToSet.setValue(x.selectedSup.map(supId => x.supervisors.find(z => z.userId === supId)).value()))
        );
    }

    /**
     * When the user selects a different ReviewTemplate get all of the ReviewTemplate and Review Profile info.
     * @param reviewTemplateInput The FormControl used by the user to update the selected `ReviewTemplate`
     * @param savedReview The Revew we are currently editing
     * @param employeeId The id of the employee we want to apply this template to (we may need this in case the ReviewTemplate has calculated values)
     * @param getRTDetail A function to get all of the data for the selected `ReviewTemplate`
     * @param getRPDetail A function to get all of the data for the selected `ReviewTemplate's` Review Profile
     */
    private getRTAndRPStream(
        reviewTemplateInput: FormControl,
        savedReview: IReview,
        employeeId: number,
        getRTDetail: (reviewTemplateId: number, employeeId: number) => Observable<IReviewTemplate>,
        getRPDetail: (RPid: number) => Observable<IReviewProfileSetup>
        ): Observable<[IReviewTemplate, IReviewProfileSetup, IReview]> {

            return combineLatest(reviewTemplateInput.valueChanges, of(savedReview), of(employeeId)).pipe(
                filter(x => x[0] != null && x[0] != ""),
                switchMap(x => {
                    const existingReview = new Maybe(x[1]);
                    const rcrDetail$ = getRTDetail(+x[0], +x[2]).pipe(
                        map(result => {
                            const evals: IEvaluationTemplateBase[] = new Maybe(result.evaluations).valueOr([]);
                            evals.forEach(evaluation => {
                                const existingEval = existingReview.map(x => x.evaluations).map(x => x.find(y => y.role === evaluation.role));
                                evaluation.startDate = existingEval.map(x => x.startDate).valueOr(evaluation.startDate);
                                evaluation.dueDate = existingEval.map(x => x.dueDate).valueOr(evaluation.dueDate);
                            });
                            result.reviewProcessStartDate = existingReview.map(x => x.reviewProcessStartDate).valueOr(result.reviewProcessStartDate);
                            result.reviewProcessEndDate = existingReview.map(x => x.reviewProcessDueDate).valueOr(result.reviewProcessEndDate);
                            result.evaluationPeriodFromDate = existingReview.map(x => x.evaluationPeriodFromDate).valueOr(result.evaluationPeriodFromDate);
                            result.evaluationPeriodToDate = existingReview.map(x => x.evaluationPeriodToDate).valueOr(result.evaluationPeriodToDate);
                            result.includeReviewMeeting = existingReview.map(x => x.isReviewMeetingRequired).valueOr(result.includeReviewMeeting);
                            result.payrollRequestEffectiveDate = existingReview.map(x => x.payrollRequestEffectiveDate).valueOr(result.payrollRequestEffectiveDate);
                            return result;
                        }), share()
                    );
                    const reviewProfile$ = rcrDetail$.pipe(
                        map(result => result.reviewProfileId),
                        concatMap(result => getRPDetail(result)))

                    return forkJoin(rcrDetail$, reviewProfile$, of(savedReview));
                }));
        }


    /**
     * Combines the values found in the forms into something that can be saved in the database.
     * @param childFormVal The value retrieved from the CalendarYearFormComponent
     * @param localFormVal The value of the form in this component
     */
    convertFormToReview(
        childFormVal: ICalendarYearForm,
        localFormVal: any,

    ): IReview {

        const removeTime = (val: any) => convertToMoment(val).startOf('day').toDate();

        let assignedSupervisor = this.supervisorCtrl.value.userId;

        const supEval = childFormVal.hasSupervisorEval ? {
            startDate: removeTime(childFormVal.supervisorEvaluationStartDate),
            dueDate: removeTime(childFormVal.supervisorEvaluationDueDate),
            role: EvaluationRoleType.Manager,
            reviewId: localFormVal.reviewId,
            evaluationId: childFormVal.supervisorEvaluationId,
            evaluatedByUserId: localFormVal.supervisorCtrl.userId,
            completedDate: childFormVal.supEvalCompleteDate,
            isViewableByEmployee: false,
            signature: childFormVal.supEvalSignatureId,
            signatures: childFormVal.supEvalSignatures,
            currentAssignedUserId: assignedSupervisor ? assignedSupervisor : childFormVal.currentAssignedSupervisor, //TPR-599 Selected supervisor not carrying over correctly from calendar-form-year component
        } : null;

        const employeeEval = childFormVal.hasEmployeeEval ? {
            startDate: removeTime(childFormVal.employeeEvaluationStartDate),
            dueDate: removeTime(childFormVal.employeeEvaluationDueDate),
            role: EvaluationRoleType.Self,
            reviewId: localFormVal.reviewId,
            evaluatedByUserId: localFormVal.employeeUserId.userId,
            evaluationId: childFormVal.employeeEvaluationId,
            completedDate: childFormVal.empEvalCompleteDate,
            isViewableByEmployee: true,
            signature: childFormVal.empEvalSignatureId,
            signatures: null,
            currentAssignedUserId: null
        } : null;

        const evals = new Maybe(supEval).map(x => [x]).valueOr([]).concat(new Maybe(employeeEval).map(x => [x]).valueOr([]));

        return {
            evaluationPeriodFromDate: removeTime(childFormVal.evaluationPeriodFromDate),
            evaluationPeriodToDate: removeTime(childFormVal.evaluationPeriodToDate),
            reviewProcessStartDate: removeTime(childFormVal.reviewProcessStartDate),
            reviewProcessDueDate: removeTime(childFormVal.reviewProcessDueDate),
            evaluations: evals,
            clientId: localFormVal.clientId,
            instructions: localFormVal.instructions,
            isGoalsWeighted: localFormVal.isGoalsWeighted,
            isReviewMeetingRequired: childFormVal.hasReviewMeeting,
            payrollRequestEffectiveDate: removeTime(localFormVal.payrollRequestEffectiveDate),
            reviewOwnerUserId: new Maybe(localFormVal.reviewOwner).map(x => x.userId).value(),
            reviewProfileId: localFormVal.reviewProfileId,
            reviewId: localFormVal.reviewId,
            reviewedEmployeeId: localFormVal.reviewedEmployeeId,
            title: localFormVal.title,
            reviewTemplateId: localFormVal.reviewTemplateId,
            evaluationGroupReviews:[]

        };
    }

    cancel() {
        this.dialogRef.close();
    }
}

@Component({
    selector: 'ds-employee-date-range',
    templateUrl: './employee-date-range.component.html'
})
export class EmployeeDateRangeComponent implements OnInit, IFormItemComponent<EmployeeDateRangeComp> {
    @Input()
    data: EmployeeDateRangeComp;
    readonly momentFormatString = 'MM/DD/YYYY';
    constructor() { }

    ngOnInit(): void { }
}



@Component({
    selector: 'ds-supervisor-select',
    template: `
    <ng-container *ngIf="setFormControl$ | async"></ng-container>
    <div class="col-md-12">
    <div class="form-group">
    <label class="form-control-label">Supervisor</label>
    <ds-contact-autocomplete
                         [multiple]="data.multiple"
                         [contacts]="data.contacts"
                         [inputControl]="textInputControl"
                         [formControl]="data.inputControl"
                         [formSubmitted]="data.submitted()"
                         name="supervisorContact"
                         required
                         errorFeedback="Please select a supervisor"
                         displayUserType="1">
                     </ds-contact-autocomplete>
    </div>
    </div>`,
    styles: [``]
})
export class SupervisorSelectComponent implements OnInit, IFormItemComponent<SupervisorComp> {
    @Input()
    data: SupervisorComp;
    textInputControl = new FormControl();

    @ViewChild(NgModel, {static: false}) model: NgModel;
    setFormControl$: Observable<any>;

    constructor() { }

    ngOnInit(): void {
        // this.setFormControl$ = this.model.valueChanges.pipe(tap(x => this.data.inputControl.setValue(x)));
    }
}

@Component({
    selector: 'ds-employee-name',
    template: `<div class="form-group">
    <label class="form-control-label">Employee</label>
    <input type="text" readonly class="form-control-plaintext" id="staticEmail" [value]="data.contact.firstName + ' ' + data.contact.lastName">
</div>`,
    styles: [``],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class EmployeeNameComponent implements OnInit, IFormItemComponent<EmployeeNamecomp> {
    @Input()
    data: EmployeeNamecomp;
    constructor() { }

    ngOnInit(): void { }
}


@Component({
    selector: 'ds-review-edit-meeting',
    template: `
    <ng-container *ngIf="listenToInputChanges$ | async"></ng-container>
    <div *ngIf="data.isMeetingRequiredCtrl.value === true" class="row">
    <div class="col-md-12">
    <h3>Review Meeting</h3>
    </div>
    </div>
    <div class="row" *ngIf="data.isMeetingRequiredCtrl.value === true">
    <div class="col-md-12">
    <span class="instruction-text">Meeting can be scheduled after saving review setup.</span>
    </div>
    </div>`,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ReviewEditMeetingComponent implements OnInit, IFormItemComponent<ReviewMeetingComp> {
    constructor(private changeDetector: ChangeDetectorRef) { }

    listenToInputChanges$: Observable<any>;
    ngOnInit(): void {
        this.listenToInputChanges$ = this.data.isMeetingRequiredCtrl.valueChanges.pipe(
            scan<boolean, {val: any, changed: boolean}>((acc, curr) => {
                const hadValue = acc.val === true;
                const hasValue = curr === true;
                const valueChanged = hadValue !== hasValue;
                acc.val = curr;
                acc.changed = valueChanged;
                return acc;
        }, {val: this.data.isMeetingRequiredCtrl.value, changed: false}),
        tap(x => {
            if(true === x.changed){
                this.changeDetector.detectChanges();
            }
        }));
    }
    @Input()
    data: ReviewMeetingComp;

}
