// Vendor
import * as _ from 'lodash';
import * as moment from 'moment';
import {
    FormGroup,
    Validators,
    FormBuilder,
    AbstractControl,
    FormControl
} from '@angular/forms';
import {
    Component,
    OnInit,
    Inject,
    OnDestroy,
    ViewChild,
    ElementRef
} from '@angular/core';
import {
    tap,
    catchError,
    takeUntil,
    share,
    map,
    switchMap,
    startWith,
    concatMap,
    filter,
    exhaustMap,
    debounceTime
} from 'rxjs/operators';
import { throwError, of, Subject, Observable, combineLatest, empty, Subscription, fromEvent } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
// Dominion
import {
    IReviewProfileSetup
} from '@ds/performance/review-profiles';
import {
    IReviewTemplateEditDialogData
} from './review-template-dialog.model';
import { IReviewProfile } from '../shared/review-profile.model';
import { EvaluationRoleType } from '@ds/performance/evaluations';
import { ReviewPolicyApiService as ReviewPolicyApiService } from '../review-policy-api.service';
import { IEvaluationTemplateBase } from '../shared/evaluation-template.model';
import { IFormItem, ICalendarYearForm } from '@ds/performance/shared/forms/calendar-year-form/calendar-year-form.model';
import { MeritIncreaseComp } from '@ds/performance/shared/forms/calendar-year-form/calendar-year-form.component';
import { MeritIncreaseComponent } from '@ds/performance/shared/forms/merit-increase/merit-increase.component';
import { Maybe } from '@ds/core/shared/Maybe';
import { convertToMoment } from '@ds/core/shared/convert-to-moment.func';
import { ReviewProfilesApiService } from '@ds/performance/review-profiles/review-profiles-api.service';
import { ReferenceDate } from '../shared/schedule-type.enum';
import { AccountService } from '@ds/core/account.service';
import { treatHardCodedRangeAsCalendarYear } from '../shared/shared-fn';
import { DateUnit } from '../shared/time-unit.enum';
import { ReviewTemplateDialogFormData, ReviewReminder, ReviewOwner, IReviewTemplate, SortByName } from '@ds/core/groups/shared/review-template.model';
import { IContact } from '@ds/core/contacts/shared/contact.model';
import { GroupService } from '@ds/core/groups/group.service';
import { ReviewsService } from '@ds/performance/reviews/shared/reviews.service';
import { MonthType } from '@ds/core/shared/month-type.enum';
import { MapToAutocomplete } from '@ds/core/groups/group-dialog/to-autocomplete-item.pipe';
import { Group } from '@ds/core/groups/shared/group.model';

const defaultFormVal = '';
@Component({
    selector: 'ds-review-template-add-profile',
    templateUrl: './review-template-dialog.component.html',
    styleUrls: ['./review-template-dialog.component.scss']
})
export class ReviewTemplateDialogComponent implements OnInit, OnDestroy {

    private readonly unsubscriber = new Subject();

    private _calendarYearFormValue: any;
    public get CalendarYearFormValue(): any {
        return this._calendarYearFormValue;
    }
    public set CalendarYearFormValue(value: any) {
        this._calendarYearFormValue = value;
    }

    get ScheduleType() {
        return ReferenceDate;
    }
    submitted = false;
    selectedReviewProfile$: Observable<{data: ICalendarYearForm, scheduleType: ReferenceDate, selectedRCR: ReviewTemplateDialogFormData, setup: IReviewProfileSetup}>;
    reviewProfiles: IReviewProfile[];
    reviewCycleReview: IReviewTemplate;
    determineFormType: FormGroup;
    SetTitle$: Observable<string>;
    meritIncreaseComp: IFormItem<MeritIncreaseComp>;
    reviewOwnerSet$: Observable<any>;
    @ViewChild('saveReviewBtn', {static: true}) saveBtn: ElementRef<HTMLInputElement>;


    momentRef = moment;
    readonly momentFormatString = 'MM/DD/YYYY';
    private readonly maxDate = 8639968460000000;
    private readonly minDate = -8639968460000000;

    defaultMaxMoment = moment(new Date(this.maxDate));
    defaultMinMoment = moment(new Date(this.minDate));
    owners$: Observable<IContact[]>;
    ownerCtrl = new FormControl();


    // determineFormType controls
    get scheduleBasedOn() { return this.determineFormType.controls['scheduleBasedOn']; }
    get reviewProfile() { return this.determineFormType.controls['reviewProfile']; }
    get title() { return this.determineFormType.controls['title']; }
    get reviewTemplateId() { return this.determineFormType.controls.reviewTemplateId; }
    get reviewCycleId() { return this.determineFormType.controls.reviewCycleId; }
    get isArchived() { return this.determineFormType.controls.isArchived; }
    get groups() { return this.determineFormType.controls.groups; }
    get reviewOwner() { return this.determineFormType.controls.reviewOwner; }
    get realReviewOwners() { return this.determineFormType.controls.realReviewOwners; }

    constructor(
        private dialogRef: MatDialogRef<ReviewTemplateDialogComponent, IReviewTemplate>,
        @Inject(MAT_DIALOG_DATA)
        public dialogData: IReviewTemplateEditDialogData,
        public reviewCyclesService: ReviewPolicyApiService,
        private reviewProfileSvc: ReviewProfilesApiService,
        public fb: FormBuilder,
        private acctSvc: AccountService,
        public groupSvc: GroupService,
        private reviewSvc: ReviewsService) {
        this.determineFormType = fb.group({
            reviewProfile: this.fb.control(defaultFormVal, { validators: [Validators.required] }),
            title: this.fb.control(defaultFormVal, { validators: [Validators.required], updateOn: 'blur'}),
            scheduleBasedOn: fb.control(defaultFormVal),
            reviewTemplateId: fb.control(defaultFormVal),
            reviewCycleId: fb.control(defaultFormVal),
            isArchived: fb.control(defaultFormVal),
            groups: fb.control(new Maybe(dialogData.reviewTemplate).map(x => x.groups).valueOr([])),
            realReviewOwners: fb.control(null),
            reviewOwner: fb.control(null)
        });
        this.reviewOwnerSet$ = this.reviewOwner.valueChanges.pipe(
            filter(x => x != null && isNaN(+x)),
            map(contact => isNaN(+contact) ? contact.userId : contact),
            tap(x => this.realReviewOwners.setValue(x)));
    }

    ngOnInit() {
        if(!this.dialogData.reviewTemplate) this.dialogData.reviewTemplate = this.defaultModel();

        fromEvent(this.saveBtn.nativeElement, 'click').pipe(
            tap(() => this.submitted = true),
            filter(() => null != this.CalendarYearFormValue && this.determineFormType.valid),
            exhaustMap(() => this.acctSvc.PassUserInfoToRequest(userInfo => of(null).pipe(tap(() => this.dialogRef.close(this.convertFormValIntoDto(this.CalendarYearFormValue, this.determineFormType.value, userInfo.selectedClientId())))))),
            takeUntil(this.unsubscriber)).subscribe();

        this.meritIncreaseComp = {
            component: MeritIncreaseComponent,
            data: {
                payrollRequestDate: null,
                submitted: () => this.submitted
            }
        };

        this.reviewProfile.setValue(new Maybe(this.dialogData.reviewTemplate).map(x => x.reviewProfileId).value());
        this.reviewTemplateId.setValue(new Maybe(this.dialogData.reviewTemplate).map(x => x.reviewTemplateId).value());
        this.reviewCycleId.setValue(this.reviewCyclesService.cycleId);
        this.scheduleBasedOn.setValue(new Maybe(this.dialogData.reviewTemplate).map(x => treatHardCodedRangeAsCalendarYear(x.referenceDateTypeId)).value());
        this.isArchived.setValue(new Maybe(this.dialogData.reviewTemplate).map(x => x.isArchived).valueOr(false));
        this.reviewOwner.setValue(new Maybe(this.dialogData.reviewTemplate).map(x => x.reviewOwners).map(x => x.map(owner => owner.contact)).valueOr([]));


        const gettingReviewProfiles$ = this.reviewCyclesService.getReviewProfilesFull(false).pipe(
            map(x => SortByName((profile) => profile.name, x)),
            tap(response => {
                this.reviewProfiles = response;
            }),
            share());

        this.setSelectedReviewProfileArchived(gettingReviewProfiles$, this.dialogData, this.unsubscriber);

        const reviewProfileInput$ = this.getReviewProfileInputStream(this.dialogData, this.reviewProfile);

        const whenRPChanges$ = combineLatest(gettingReviewProfiles$, reviewProfileInput$).pipe(
            map(x => ({ reviewProfiles: x[0], reviewProfileId: +x[1] })));

        this.SetTitle$ = this.setTitleWhenNoValueSaved(whenRPChanges$, this.dialogData);

        const selectedRP$ = whenRPChanges$.pipe(switchMap(x =>
            this.reviewProfileSvc.getReviewProfileSetup(x.reviewProfileId).pipe(catchError(y => {
                return y.status == 400 ? of(<IReviewProfileSetup>{}) : throwError(y);
            }))));

        this.selectedReviewProfile$ = combineLatest(selectedRP$, of(this.dialogData.reviewTemplate), this.scheduleBasedOn.valueChanges.pipe(startWith(this.scheduleBasedOn.value))).pipe(map(x =>
            ({ setup: x[0], selectedRCR: x[1], scheduleType: x[2] })),
            map(x => {
                const savedRCR = new Maybe(x.selectedRCR);
                const rcrEvals = savedRCR.map(y => y.evaluations);
                const rcrSelfEval = rcrEvals.map(y => y.find(z => z.role === EvaluationRoleType.Self));
                const rcrSupEval = rcrEvals.map(y => y.find(z => z.role === EvaluationRoleType.Manager));


                return <{data: ICalendarYearForm, scheduleType: ReferenceDate, selectedRCR: ReviewTemplateDialogFormData, setup: IReviewProfileSetup}>{
                data: {employeeEvaluationDueDate: rcrSelfEval.map(y => y.dueDate).value(),
                employeeEvaluationStartDate: rcrSelfEval.map(y => y.startDate).value(),
                supervisorEvaluationStartDate: rcrSupEval.map(y => y.startDate).value(),
                supervisorEvaluationDueDate: rcrSupEval.map(y => y.dueDate).value(),
                evaluationPeriodFromDate: savedRCR.map(y => y.evaluationPeriodFromDate).value(),
                evaluationPeriodToDate: savedRCR.map(y => y.evaluationPeriodToDate).value(),
                employeeRCRReviewProfileEvalId: new Maybe(x.setup.evaluations).map(y => y.find(y => y.role == EvaluationRoleType.Self)).map(y => y.reviewProfileEvaluationId).value(),
                supervisorRCRReviewProfileEvalId: new Maybe(x.setup.evaluations).map(y => y.find(y => y.role == EvaluationRoleType.Manager)).map(y => y.reviewProfileEvaluationId).value(),
                hasReviewMeeting: x.setup.includeReviewMeeting,
                payrollRequestDate: savedRCR.map(y => y.payrollRequestEffectiveDate).value(),
                reviewProcessDueDate: savedRCR.map(y => y.reviewProcessEndDate).value(),
                reviewProcessStartDate: savedRCR.map(y => y.reviewProcessStartDate).value(),
                hasEmployeeEval: new Maybe(x.setup.evaluations).map(y => y.some(z => z.role === EvaluationRoleType.Self)).valueOr(false),
                hasSupervisorEval: new Maybe(x.setup.evaluations).map(y => y.some(z => z.role === EvaluationRoleType.Manager)).valueOr(false)
                },
                scheduleType: x.scheduleType,
                selectedRCR: { template: x.selectedRCR,  usersNotified: null},
                setup: x.setup
            };
        }));

        this.owners$ = this.ownerCtrl.valueChanges.pipe(
            startWith(null),
            debounceTime(500),
            switchMap((searchText: string) => this.reviewSvc.getReviewSetupContacts({ page: 1, pageSize: 75, searchText: searchText, excludeTimeClockOnly: true, haveActiveEmployee: true, ifSupervisorGetSubords: true})),
            map(searchResults => _.filter(searchResults.results, c => !!c.userId))
        );
    }

    ngOnDestroy(): void {
        this.unsubscriber.next();
    }

    private setTitleWhenNoValueSaved(selectedRPChanged$: Observable<{reviewProfiles: IReviewProfile[], reviewProfileId: number}>,  dialogData: IReviewTemplateEditDialogData): Observable<any> {
        return combineLatest(selectedRPChanged$, of(new Maybe(dialogData.reviewTemplate).map(x => x.name).value())).pipe(
            map(x => x[1] || x[0].reviewProfiles.find(y => y.reviewProfileId === x[0].reviewProfileId).name),
            tap(x => this.title.setValue(x)));
    }

/**
 * When a review profile is loaded into the reviewProfile formcontrol, make sure the initial value is emitted.
 */
    private getReviewProfileInputStream(dialogData: IReviewTemplateEditDialogData, reviewProfileCtrl: AbstractControl) {
        return new Maybe(dialogData.reviewTemplate)
        .map(x => x.reviewProfileId).value() == null ? reviewProfileCtrl.valueChanges : reviewProfileCtrl.valueChanges.pipe(startWith(reviewProfileCtrl.value));
    }

    private setSelectedReviewProfileArchived(profiles: Observable<IReviewProfile[]>, dialogData: IReviewTemplateEditDialogData, unsubscriber: Observable<any>): Subscription {
        return profiles.pipe(concatMap(response => {
            const id = new Maybe(dialogData.reviewTemplate)
        .map(x => response.find(y => y.reviewProfileId === x.reviewProfileId)).value();
        if (id == null) {
            return new Maybe(dialogData.reviewTemplate)
            .map(x => x.reviewProfileId)
            .map(x => this.reviewProfileSvc.getReviewProfileSetup(x))
            .map(data$ => data$.pipe(
                tap(archived => {
                    this.reviewProfiles.push(<IReviewProfile>{
                    reviewProfileId: archived.reviewProfileId,
                    name: `${archived.name} (Archived)`
                });
            })
            )).valueOr(empty());
        }
        return empty();
    }),
        takeUntil(unsubscriber)).subscribe();
    }

    private readonly convertFormValIntoDto = (childFormVal: any, formVal: any, clientId: number) => {
        const convertToDate = (formVal: any) => new Maybe(formVal).map(x => convertToMoment(x)).map(x => x.toDate()).value();
        const getEval = (startDate: any,
            dueDate: any,
            roleType: EvaluationRoleType,
            reviewProfileEvaluationId?: number,
            reviewTemplateId?: number,
            evaluationDuration?: number,
            evaluationDurationUnitTypeId?: DateUnit,
            conductedBy?: number) =>
        new Maybe(startDate || evaluationDuration) // only create eval when dates provided
        .map(() => [<IEvaluationTemplateBase>{
            dueDate: dueDate,
            startDate: startDate,
            role: roleType,
            reviewProfileEvaluationId: reviewProfileEvaluationId,
            reviewTemplateId: reviewTemplateId,
            evaluationDuration: evaluationDuration,
            evaluationDurationUnitTypeId: evaluationDurationUnitTypeId,
            conductedBy: conductedBy == -1 ? null : conductedBy
        }]).valueOr(<IEvaluationTemplateBase[]>[]);

        const employeeEval = getEval(
            childFormVal.employeeEvaluationStartDate,
            childFormVal.employeeEvaluationDueDate,
            EvaluationRoleType.Self,
            childFormVal.employeeRCRReviewProfileEvalId,
            formVal.reviewTemplateId,
            childFormVal.employeeDuration,
            childFormVal.employeeUnitType,
            null);
        const supervisorEval = getEval(childFormVal.supervisorEvaluationStartDate,
            childFormVal.supervisorEvaluationDueDate,
            EvaluationRoleType.Manager,
            childFormVal.supervisorRCRReviewProfileEvalId,
            formVal.reviewTemplateId,
            childFormVal.supervisorDuration,
            childFormVal.supervisorUnitType,
            childFormVal.supervisorEvalConductedBy);

        const reviewReminders: ReviewReminder[] = [];
        if (childFormVal.reminderDurationPrior != null && childFormVal.reminderdurationUnitType != null) {
            reviewReminders.push({
                durationPrior: childFormVal.reminderDurationPrior,
                durationPriorUnitTypeId: childFormVal.reminderdurationUnitType,
                reviewReminderID: childFormVal.reviewReminderId,
                reviewTemplateId: formVal.reviewTemplateId
            });
        }


        const result: IReviewTemplate = {
            evaluationPeriodFromDate: convertToDate(childFormVal.evaluationPeriodFromDate),
            evaluationPeriodToDate: convertToDate(childFormVal.evaluationPeriodToDate),
            reviewProcessStartDate: convertToDate(childFormVal.reviewProcessStartDate),
            reviewProcessEndDate: convertToDate(childFormVal.reviewProcessDueDate),
            evaluations: employeeEval.concat(supervisorEval),
            payrollRequestEffectiveDate: new Maybe(childFormVal.payrollRequestDate).map(convertToDate).valueOr(new Maybe(childFormVal.payrollRequest).map(this.convertMonthAndDateToDateType).value()),
            name: formVal.title,
            reviewTemplateId: formVal.reviewTemplateId,
            reviewProfileId: formVal.reviewProfile,
            includeReviewMeeting: childFormVal.hasReviewMeeting,
            referenceDateTypeId: formVal.scheduleBasedOn,
            delayAfterReference: formVal.scheduleBasedOn == ReferenceDate.CalendarYear && !childFormVal.startDuration ? 0 : childFormVal.startDuration,
            delayAfterReferenceUnitTypeId: formVal.scheduleBasedOn == ReferenceDate.CalendarYear && !childFormVal.startUnitType ? DateUnit.Day : childFormVal.startUnitType,
            evaluationPeriodDuration: childFormVal.evalPrevDuration,
            evaluationPeriodDurationUnitTypeId: childFormVal.evalPrevUnitType,
            reviewProcessDuration: childFormVal.timeCompleteDuration,
            reviewProcessDurationUnitTypeId: childFormVal.timeCompleteUnitType,
            clientId: clientId,
            isArchived: formVal.isArchived,
            groups: formVal.groups,
            hardCodedAnniversary: typeof childFormVal.monthAndDate === 'string' ? childFormVal.monthAndDate : this.convertMonthAndDateToDateType(childFormVal.monthAndDate),
            reviewReminders: reviewReminders,
            isRecurring: childFormVal.isRecurring,
            reviewOwners: new Maybe(formVal.realReviewOwners).map(x => ([ <ReviewOwner> {
                userId: x,
                reviewTemplateId: formVal.reviewTemplateId
            }])).valueOr([] as ReviewOwner[])
        };

        return result;
    }

    private convertMonthAndDateToDateType(input?: {month: MonthType, date: number}): string {
        return new Maybe(input)
        .map(x => x.month && x.date ? x : null)
        .map(x => moment({month: x.month - 1, day: x.date}).format('YYYY-MM-DD')).value();
    }

    private defaultModel():IReviewTemplate
    {
        var today = new Date();
        var defaultDate = today.getFullYear() + '-01-01';

        let result: IReviewTemplate =  {
            evaluationPeriodFromDate: null,
            evaluationPeriodToDate: null,
            reviewProcessStartDate: null,
            reviewProcessEndDate: null,
            evaluations: null,
            payrollRequestEffectiveDate: defaultDate,
            name: null,
            reviewTemplateId: null,
            reviewProfileId: null,
            includeReviewMeeting: null,
            referenceDateTypeId: null,
            delayAfterReference: null,
            delayAfterReferenceUnitTypeId: null,
            evaluationPeriodDuration: null,
            evaluationPeriodDurationUnitTypeId: null,
            reviewProcessDuration: null,
            reviewProcessDurationUnitTypeId: null,
            clientId: null,
            isArchived: false,
            groups: null,
            hardCodedAnniversary: null,
            reviewReminders: null,
            isRecurring: this.dialogData.openRecurringView,
            reviewOwners: null
        };
        return result;
    }

    groupMapper: MapToAutocomplete<Group> = (val) => ({display: val.name, value: val.groupId});

    cancel() {
        this.dialogRef.close();
    }
}
