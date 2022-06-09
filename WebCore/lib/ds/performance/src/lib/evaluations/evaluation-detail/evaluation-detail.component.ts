import { Component, OnInit, ViewChildren, QueryList, ElementRef, OnDestroy, ViewChild, Inject, ChangeDetectorRef } from '@angular/core';
import { UserInfo, IRemark, FieldType, ViewRemark, API_STRING } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { zip, of, throwError, Subscription, Subject, Observable, merge, Observer, ReplaySubject } from 'rxjs';
import * as moment from 'moment';
import { GoalEvalItem } from '../shared/goal-eval-item';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { GoalWeightingDialogComponent } from '../goal-weighting/goal-weighting-dialog/goal-weighting-dialog.component';
import { map, concatMap, catchError, tap, exhaustMap, mergeMap, switchMap, filter } from 'rxjs/operators';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { DsApiCommonProvider } from '@ajs/core/api/ds-api-common.provider';
import { ActiveEvaluationService } from '../shared/active-evaluation.service';
import { IReviewRating } from '@ds/performance/ratings';
import { EvaluationsApiService } from '../shared/evaluations-api.service';
import { ICompetencyEvaluation } from '../shared/competency-evaluation.model';
import { IGoalEvaluation } from '../shared/goal-evaluation.model';
import { IFeedbackResponse, TextItemFeedbackResponse, DateFeedbackResponse, MultiSelectFeedbackResponse, IFeedbackItem } from '@ds/performance/feedback';
import { EvaluationSummaryDialogService } from '../evaluation-summary-dialog/evaluation-summary-dialog.service';
import { IChartSettings } from '../shared/chart-settings.model';
import { IReview } from '@ds/performance/reviews/shared/review.model';
import { IEvaluationWithStatusInfo } from '../shared/evaluation-status-info.model';
import { IEvaluation, EvaluationRoleType } from '@ds/performance/evaluations';
import { CompetencyGroupItem } from '../shared/competency-group-item';
import { CompetencyEvalItem } from '../shared/competency-eval-item';
import { checkboxComponent } from '@ajs/applicantTracking/application/inputComponents';
import * as _ from "lodash";
import { IContact } from '@ds/core/contacts/shared/contact.model';
import { ApprovalProcessSummaryDialogService } from '../approval-process-summary-dialog/approval-process-summary-dialog.service';
import { DOCUMENT } from '@angular/common';
import { Maybe } from '@ds/core/shared/Maybe';
import { AutoSaveThrottleStrategy, LoadingResult, TypedLoadingResult, SaveHandler } from '@ds/core/shared/save-handler-strategy';
import { ApprovalProcessHistoryAction } from '../shared/approval-process-history-action.enum';
import { ApprovalProcessStatus } from '../shared/approval-process-status.enum';
import { IApprovalProcessStatusIdAndIsEditedByApprover } from '../shared/approval-process-status-id-and-is-edited-by-approver.interface';

@Component({
    selector: 'ds-evaluation-detail',
    templateUrl: './evaluation-detail.component.html',
    styleUrls: ['./evaluation-detail.component.scss']
})
export class EvaluationDetailComponent implements OnInit, OnDestroy {

    private _subs: Subscription[] = [];

    user:UserInfo;
    isLoading: boolean = true;
    private _evaluationDetail: IEvaluationWithStatusInfo;
    get evaluationDetail(): IEvaluationWithStatusInfo {
        return this._evaluationDetail;
    }
    review:IReview = {} as IReview;
    ratings:IReviewRating[];
    competencyGroupStatus: CompetencyGroupItem[] = [];

    /** UI VARS ONLY */
    goalsComplete = 0;
    requiredFeedbackCount = 0;
    totalFeedbackComplete = 0;
    requiredFeedbackComplete = 0;
    percentComplete = 0;
    isSubmitted:boolean = false;
    isFeedbackActive = false;
    isFeedbackLoading = false;
    isFeedbackPending = false;
    FieldType = FieldType;
    @ViewChildren('compCard') compCards: QueryList<ElementRef>;
    @ViewChildren('goalCard') goalCards: QueryList<ElementRef>;
    @ViewChildren('compComment') compComments: QueryList<ElementRef<HTMLTextAreaElement>>;
    @ViewChildren('goalComment') goalComments: QueryList<ElementRef<HTMLTextAreaElement>>;
    saveFeedback: Subject<IFeedbackResponse> = new Subject();
    saveGoalEval: Subject<GoalEvalItem> = new Subject();
    saveCompEval: Subject<CompetencyEvalItem> = new Subject();
    saveApprovalStatus: Subject<{item: any, val: number, type: number}> = new Subject();
    getFeedbackResponses: Subject<IFeedbackResponse[]> = new ReplaySubject(1);
    getGoalEvals: Subject<IGoalEvaluation[]> = new ReplaySubject(1);
    getCompEvals: Subject<ICompetencyEvaluation[]> = new ReplaySubject(1);
    private readonly feedbackSavers: {[id:string]: {source: Observer<IFeedbackResponse>, target: Observable<any>}} = {};
    private apgSection: ElementRef
    @ViewChild('apgSection', { static: false }) set apgSectionSetter(apgSection: ElementRef) {
        this.apgSection = apgSection;
    };
    private apfSection: ElementRef
    @ViewChild('apfSection', { static: false }) set apfSectionSetter(apfSection: ElementRef) {
        this.apfSection = apfSection;
    };
    private carSection: ElementRef
    @ViewChild('carSection', { static: false }) set carSectionSetter(carSection: ElementRef) {
        this.carSection = carSection;
    };
    private aweSection: ElementRef
    @ViewChild('aweSection', { static: false }) set aweSectionSetter(aweSection: ElementRef) {
        this.aweSection = aweSection;
    };
    private approvedSection: ElementRef
    @ViewChild('approvedSection', { static: false }) set approvedSectionSetter(approvedSection: ElementRef) {
        this.approvedSection = approvedSection;
    };

    /** USED FOR LOGIC */
    competencyItems: CompetencyEvalItem[] = [];
    competencyGroups: string[] = [];
    goalItems: GoalEvalItem[] = [];
    lastModified: moment.Moment | null;
    competenciesComplete = 0;
    viewPayrollRequest:boolean = false;
    piechartdata:any;
    rawScoreData:any;
    returnRouterLink:any[] = ['performance', 'employees', 'reviews'];
    chartSettings:IChartSettings;
    overallScore:number = 0;
    viewableFeedbackResponses:IFeedbackResponse[] = [];
    remarkEditMode = false;
    supervisors: IContact[] = [];
    disableApproveAll = false;
    isOriginalEvaluator = false;
    disableSubmit = false;
    approvedItems: number = 0;
    approvedWithEditsItems: number = 0;
    rejectedItems: number = 0;

    isAPFinalized = false;
    isAPReopened = false;
    approvalProcessWithoutHistory = false;
    isAPFirstVisit = false;
    /**
     * Wraps up all of the 'autosave' functionality into one package.
     */
    handlers$: Observable<any>;

    private readonly continueHandler: SubmitState = {
        text: 'Continue',
        handler: this.continueEvaluation.bind(this)
    };

    private readonly submitEvalHandler: SubmitState = {
        text: 'Submit',
        handler: this.submitEvaluation.bind(this)
    };
    currSubmitState: SubmitState = this.continueHandler;


    /** GETTERS */
    get goalsLength():number {
        return this.evaluationDetail ? this.evaluationDetail.goalsLength : 0;
    }
    get hasCompetencies() {
        return this.evaluationDetail && this.evaluationDetail.hasCompetencies;
    }
    get hasGoals() {
        return this.evaluationDetail && this.evaluationDetail.hasGoals;
    }
    get hasFeedback() {
        return this.evaluationDetail && this.evaluationDetail.hasFeedback;
    }
    get isReadOnly() {
        return this.evaluationDetail && this.evaluationDetail.isReadOnly;
    }
    get isEvalComplete() {
        return this.evaluationDetail && this.evaluationDetail.isEvalComplete;
    }
    get isUserEvaluator() {
        return this.evaluationDetail && this.evaluationDetail.isUserEvaluator;
    }
    get isFeedbackComplete() {
        return !this.hasFeedback || (this.requiredFeedbackComplete === this.requiredFeedbackCount && this.totalFeedbackComplete);
    }
    get hasInstructions() {
        return this.review && this.review.instructions;
    }
    get isSupervisorEvaluationViewedByEmployee(){
        return this.review.reviewedEmployeeId == this.user.employeeId && this.isSupervisorEvaluation;
    }
    get isSupervisorEvaluation(){
        return this.evaluationDetail.role == EvaluationRoleType.Manager;
    }

    // Make this enum available in the template
    ApprovalProcessStatus = ApprovalProcessStatus;

    constructor(
        private evalStore:ActiveEvaluationService,
        private evalService:EvaluationsApiService,
        private accountService:AccountService,
        private messageService:DsMsgService,
        private router:Router,
        private activatedRoute:ActivatedRoute,
        private dialog:MatDialog,
        private evalSummaryDialog: EvaluationSummaryDialogService,
        private appProcessSummaryDialog: ApprovalProcessSummaryDialogService,
        @Inject(DOCUMENT) private document: any
    ) {
        this.handlers$ = merge(
            this.createWhenDataLoaded(this.getFeedbackResponses, (response) => {
                const source = this.filterById(this.saveFeedback, (input) => input.responseId, response.responseId);

                const handlerResult = this.createThrottlerAndLoadingHandler(this.areFeedbackEqual, (isLoading) => {
                    // the feedbackresponse passed to this method is a deep copy of the original
                    var item = this.evaluationDetail.feedbackResponses.find(x => x.responseId == isLoading.input.responseId)
                    item.isLoading = isLoading.isLoading;
                });

                const result = this.createSaveHandler(source, this.createFeedbackResponseHandler, handlerResult);
                return result;
            }),
            this.createWhenDataLoaded(this.getGoalEvals, (goalEval) => {
                const source = this.filterById(this.saveGoalEval, (input) => input.goal.goalId, goalEval.goalId);

                const handlerResult = this.createThrottlerAndLoadingHandler(this.areGoalEvalsEqual, (input) => {

                    const mahInput = input.input as GoalEvalItem | IGoalEvaluation;

                    var safeMahInput = new Maybe(mahInput);
                    const id = safeMahInput.map(x => (x as GoalEvalItem).goal).map(x => x.goalId).value() || safeMahInput.map(x => (x as IGoalEvaluation).goalId).value();

                    const item = this.goalItems.find(goalEval => goalEval.goal.goalId == id);

                    item.isLoading = input.isLoading;
                });

                const result = this.createSaveHandler(source, this.createGoalEvalHandler, handlerResult);
                return result;
            }),
            this.createWhenDataLoaded(this.getCompEvals, (compEval) => {
                const source = this.filterById(this.saveCompEval, (input) => input.competencyId, compEval.competencyId);

                const handlerResult = this.createThrottlerAndLoadingHandler(this.areCompEvalsEqual, (input) => {

                    const item = this.competencyItems.find(comp => comp.competency.competencyId == input.input.competencyId)

                    item.isLoading = input.isLoading;
                });

                const result = this.createSaveHandler(source, this.createCompEvalHandler, handlerResult);
                return result;
            }),
            this.updateApprovalStatusHandler(this.saveApprovalStatus));
    }

    /**
     * Get the data we want to display and update the evaluation to include the correct items (via sync content) if appropriate.  Once that is
     * all finished create an instance of our save handlers (see the initialization of `handlers$` in the constructor)
     */
    ngOnInit() {
        this.returnRouterLink = this.evalStore.returnUrl;

        let loadingSub = this.evalStore.isLoadingDetail$.subscribe(isLoading => {
            this.isLoading = isLoading;

            if (!isLoading) {
                const user$ = this.accountService.getUserInfo();
                const eval$ = this.evalStore.evaluationDetail$;
                const review$ = this.evalStore.review$;

                const componentData$ = zip(user$, eval$, review$);

                this._subs.push(this.evalStore.onCanShowSummaryUpdate(canShowSummary => {
                    const otherResult = canShowSummary.otherResult;

                    this.currSubmitState = (canShowSummary.isPayrollRequestEnabled && Array.isArray(otherResult) && (<IEvaluationWithStatusInfo> otherResult[1]).hasSummaryData)
                        ? this.continueHandler
                        : this.submitEvalHandler;
                }, componentData$));

                componentData$.pipe(
                        tap(([user, evaluation, review]) => {
                            if (evaluation == null && review == null) {
                                this.returnLink(); // this.router.navigate(this.returnRouterLink);
                                return;
                            }
                            this.user = user;
                            this._evaluationDetail = evaluation;

                            if (this._evaluationDetail.isApprovalProcess) {
                                this.approvalProcessInit();
                            }

                            this._evaluationDetail.feedbackResponses.forEach(x => {
                                x.isActive = false;
                             });
                            this.ratings = evaluation.ratings;
                            this.review = review != null ? review : this.review;
                            this.viewAbleResponses();

                            this.refreshEvalContent();

                            if (!this.isReadOnly) {
                                this.syncContent();
                            } else {
                                // create our handlers
                            this.getFeedbackResponses.next(this.evaluationDetail.feedbackResponses);
                            this.getGoalEvals.next(this.evaluationDetail.goalEvaluations);
                            this.getCompEvals.next(this.evaluationDetail.competencyEvaluations);
                            }

                        })
                    )
                    .subscribe();
            }
        });

        this._subs.push(loadingSub);
    }

    /**
     * Waits until we have the items we want to create an auto-save handler for.  Once we have the
     * items we set up auto-save streams that run in parallel. Normally we would create one auto-save
     * handler but this is using template driven forms, not reactive like it was intended for.
     *
     * @param source The stream that will give us the items we have to wait for.
     * @param getStream Creates an auto-save handler function/observable for the provided item.
     */
    private createWhenDataLoaded<T, U>(source: Subject<T[]>, getStream: (item: T) => Observable<U>) {
        return source.pipe(switchMap((inputs) => {
            const nonNullInputs = inputs || [];
                const responseSavers: Observable<any>[] = [];

                nonNullInputs.forEach(item => {
                    responseSavers.push(getStream(item));
                });

                return merge(... responseSavers);
        }));
    }

    /**
     * Ensure that the item is passed into the correct stream.
     * We use one source to emit into multiple streams.  Every
     * time the source passes us an item, it should continue down only one stream.
     *
     * @param source Emits the changes that we want to save.
     * @param getId The id of the item passed into our stream.
     * @param id The id of the item our stream is intended for.
     */
    private filterById<T, U extends string | number>(source: Observable<T>, getId: (input: T) => U, id: U) {
        return source.pipe(filter(input => getId(input) == id));
    }

    ngOnDestroy(): void {
        if (this._subs) {
            this._subs.forEach(s => s.unsubscribe());
            this._subs = [];
        }
    }

    goToCompetencyGroupSection(index: number) {
        const elem = this.document.getElementById('competencyGroup_' + index);
        if (elem == null) return;
        elem.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }

    syncContent() {
        const shouldNotSync = new Maybe(this.evaluationDetail.approvalProcessHistory).map(x => {
            const wasContinued = x.length && x[x.length - 1].action === ApprovalProcessHistoryAction.Continued;
            const wasFinalized = x.length && x[x.length - 1].action === ApprovalProcessHistoryAction.Finalized;
            const wasRejected = x.length && x[x.length - 1].action === ApprovalProcessHistoryAction.Rejected;
            const isFirstStepOfProcess = x.length === 1;
            return (wasContinued && !isFirstStepOfProcess) || wasFinalized || wasRejected;
        }).valueOr(false);
        if(shouldNotSync){
            this.getFeedbackResponses.next(this.evaluationDetail.feedbackResponses);
                this.getGoalEvals.next(this.evaluationDetail.goalEvaluations);
                this.getCompEvals.next(this.evaluationDetail.competencyEvaluations);
        } else {
            this.messageService.loading(true);
            this.evalService.syncEvaluationContent(this.evaluationDetail.evaluationId)
                .subscribe(data => {
                    this._evaluationDetail = this.evalStore.setEvaluationDetail(data);
                    if (this._evaluationDetail.isApprovalProcess) {
                        this.approvalProcessInit();
                    }
                    this.ratings = data.ratings;
                    this.refreshEvalContent();

                    this.getFeedbackResponses.next(this.evaluationDetail.feedbackResponses);
                    this.getGoalEvals.next(this.evaluationDetail.goalEvaluations);
                    this.getCompEvals.next(this.evaluationDetail.competencyEvaluations);

                    this.messageService.loading(false);
                });
        }
    }

    filterItemsOfType(group) {
        return this.evalStore.filterItemsOfType(group, this.competencyItems);
    }

    castItemsAsType() {
        return <CompetencyEvalItem[]>this.competencyItems;
    }

    selectCompetencyEvaluation(item: CompetencyEvalItem) {
        item.isActive = !item.isActive;
        this.competencyItems.forEach(x => {
            if (item.competency.competencyId !== x.competency.competencyId)
                x.isActive = false;
            x.color(false);
        });
    }

    selectResponseEvaluation(item: any) {
        item.isActive = !item.isActive;
        this.evaluationDetail.feedbackResponses.forEach(x => {
            if (x.feedbackId != item.feedbackId)
                x.isActive = false;
        });
    }

    selectGoalEvaluation(item: GoalEvalItem) {
        item.isActive = !item.isActive;
        this.goalItems.forEach(x => {
            if (item.goal.goalId !== x.goal.goalId)
                x.isActive = false;
            x.color(false);
        });
    }

    private areCompEvalsEqual(compEvalA: CompetencyEvalItem, compEvalB: CompetencyEvalItem): boolean {
        return compEvalA.ratingValue == compEvalB.ratingValue &&
        compEvalA.comments == compEvalB.comments;
    }

    /**
     * Create a throttling strategy function with a custom equality and loading callback
     *
     * @param areEqual A function that determines whether the input is different than the input originally passed into our api call
     * @param setLoading A callback for our custom loading logic
     */
    private createThrottlerAndLoadingHandler<T>(areEqual: (a: T, b: T) => boolean, setLoading: (input: TypedLoadingResult<T>) => void): HandlerResult {
        var isLoading = new Subject<LoadingResult>();
        const setLoadingStream$ = isLoading.pipe(tap(x => setLoading(x)))
        const saveHandler = AutoSaveThrottleStrategy(areEqual, isLoading);

        return {
            handler: saveHandler,
            loadingStream$: setLoadingStream$
        };
    }

    private createCompEvalHandler: CreateSaver<CompetencyEvalItem> = (source, saveHandler) => {
        return source.pipe(map(item => {
            if (item.comments && item.comments.trim()) {
                if (!item.competency.comment) {
                    item.competency.comment = <IRemark>{};
                }

                item.competency.comment.description = item.comments.trim();
            }
            else {
                item.competency.comment = null;
            }
            return item;
        }),
        saveHandler(this.messageService,
            mahItem => {

                var item = this.competencyItems.find(x => x.competencyId == mahItem.competencyId);
                item = item == null ? mahItem : item;

                let isRatingUpdate = mahItem.competency.ratingValue !== mahItem.ratingValue;
            mahItem.competency.ratingValue = mahItem.ratingValue;

                return this.evalService.saveCompetencyEvaluation(mahItem.competency).pipe(tap(data => {
                    if (this._evaluationDetail.isApprovalProcess) {
                        item.competency.activityFeed = data.activityFeed;
                        item.competency.ratingValue = data.ratingValue;
                        item.competency.comment = data.comment;
                    } else {
                        item.competency = data;
                    }

                    if (isRatingUpdate) {
                        item.isCommentRequired = this._evaluationDetail.competencyRateCommentRequired.some(x => {
                            return x.reviewRating.rating === item.ratingValue;
                        });
                    }

                    item.isLoading = false;

                    let idx = _.findIndex(this.evaluationDetail.competencyEvaluations, x => x.competencyId === data.competencyId);
                    if (idx > -1) {
                        if (this._evaluationDetail.isApprovalProcess) {
                            this.evaluationDetail.competencyEvaluations[idx].activityFeed = data.activityFeed;
                            this.evaluationDetail.competencyEvaluations[idx].ratingValue = data.ratingValue;
                            this.evaluationDetail.competencyEvaluations[idx].comment = data.comment;
                        } else {
                            this.evaluationDetail.competencyEvaluations[idx] = data;
                        }
                        this.evaluationDetail.competencyEvaluations[idx].isCommentRequired = item.isCommentRequired;
                    }

                    if (this.isOriginalEvaluator && item.isCommentRequired && item.isWithoutComment && item.approvalProcessStatusId == null) {
                        this.updateApprovalStatus(item, 0, 2)
                    }

                    this.refreshEvalStats();

                    if (!isRatingUpdate && !!data.ratingValue)
                        item.isActive = false;
                    else
                        this.focusCompComment(idx);
                }),
                catchError((error: HttpErrorResponse) => {
                    this.messageService.showWebApiException(error.error);
                    item.isLoading = false;
                    return of(null);
                }))
            },
            'Save Competency',
            true));
    }

    /**
     * Turns a few pre-configured items into a resulting function/observable that will handle all saving logic for an item.
     *
     * @param source A function/observable that will provide the item we need to save
     * @param createEffect The function/observable that handles calling a specific endoint on the webserver
     * @param handlerResult see `HandlerResult`
     */
    private createSaveHandler<T>(
        source: Observable<T>,
        createEffect: (source: Observable<T>, saveHandler: SaveHandler) => Observable<any>,
        handlerResult: HandlerResult): Observable<any> {

        const saveStream$ = createEffect(source, handlerResult.handler);


        const loadHandler$ = handlerResult.loadingStream$;

        return merge(saveStream$, loadHandler$)
    }

    saveCompetencyEvaluation(item: CompetencyEvalItem, idx: number) {
        const hasChangesAndIsComplete = item.hasChanges && !item.isWithoutComment && !!item.ratingValue;
        const hasChangesAndWasCleared = item.hasChanges && !item.isWithoutComment && !item.ratingValue
        if(hasChangesAndIsComplete || hasChangesAndWasCleared){
            this.saveCompEval.next(item);
        }
    }

    focusCompComment(idx: number, scrollTo = false) {
        let elem = this.compComments.find((el, i) => i === idx);
        elem.nativeElement.focus();
        if (scrollTo) {
            if (this.compCards.find((el, i) => i === idx).nativeElement) {
                this.compCards.find((el, i) => i === idx).nativeElement.scrollIntoView();
            }
        }
    }

    removeCompetencyEvaluation(competency: ICompetencyEvaluation) {
        this.messageService.loading(true);
        this.evalService.removeCompetencyEvaluation(this.evaluationDetail.evaluationId, competency.competencyId)
            .subscribe(data => {
                _.remove(this.evaluationDetail.competencyEvaluations, ce => ce.competencyId === competency.competencyId);
                this.evalStore.refreshCompetencyItems(this.evaluationDetail, this.competencyGroups, this.competencyItems, this.ratings);
                this.refreshEvalStats();
                this.messageService.loading(false);
            },
            (error: HttpErrorResponse) => {
                this.messageService.showWebApiException(error.error);
            });
    }



    addGoalWeights(isInApprovalProcess?: boolean) {
        const isApprovalProcess = !! isInApprovalProcess;
        const originalWeights = {};

        if(isApprovalProcess){
            this.goalItems.reduce((prev, next) => {
                prev[next.goal.goalId] = next.goal.weight;
                return prev;
            }, originalWeights)
        }


        this.goalItems.forEach(x => { x.goal.onReview = true; })
        this.dialog.open(GoalWeightingDialogComponent, {
            width: '800px',
            disableClose: true,
            data: {
                goalItems: this.goalItems
            }
        })
            .afterClosed()
            .pipe(map(result => {
                if (result == null || result == undefined) return null;
                let goalWeightDto = [];
                this.messageService.sending(true);
                result.forEach(gei => {
                    var goalWeightItem = {
                        evaluationId: this.evaluationDetail.evaluationId,
                        GoalId: gei.goal.goalId,
                        Weight: Number(gei.goal.weight),
                        OnReview: gei.goal.onReview
                    }
                    goalWeightDto.push(goalWeightItem);
                });
                return goalWeightDto;
            }),
                concatMap((x: any[] | null) => {
                    if (x == null || x == undefined) {
                        return of(null);
                    } else {
                        x.forEach(item => {
                            if(originalWeights[item.GoalId] != null && originalWeights[item.GoalId] != item.Weight){
                                item.approvalProcessStatus = 0;
                            }
                        })

                        return this.evalService.saveWeightedGoals(x);
                    }
                }),
                catchError(err => {
                    this.messageService.sending(false);

                    this.messageService.setTemporaryMessage('Sorry, goal weighting failed to save.', MessageTypes.error, 3000);
                    return throwError(err);
                })).subscribe(x => {
                    if (x != null) {
                        this.updateGoalWeights(x);
                    }
                });
    }

    updateGoalWeights(goalWeights: any[]) {
        var x = goalWeights;
        this.goalItems.forEach(x => {
            goalWeights.forEach(gw => {
                if (gw.goalId == x.goal.goalId) {
                    x.goal.onReview = gw.onReview;
                    x.goal.weight = gw.weight;
                    if(gw.approvalProcessStatus != null){
                        x.goal.approvalProcessStatusId = gw.approvalProcessStatus;
                    }
                    if(gw.newRemark && x.goal.activityFeed){
                        x.goal.activityFeed.unshift(gw.newRemark);
                    }
                }
            });
        });

        this.goalItems = this.goalItems.filter(gi => gi.goal.onReview);
        this.messageService.sending(false);
        this.messageService.setTemporaryMessage("Goal weights saved.");
    }

    private areGoalEvalsEqual(goalEvalA: GoalEvalItem, goalEvalB: GoalEvalItem): boolean {
        return goalEvalA.ratingValue == goalEvalB.ratingValue &&
        goalEvalA.comments == goalEvalB.comments;
    }

    private createGoalEvalHandler: CreateSaver<GoalEvalItem> = (source, saveHandler) => {
        return source.pipe(map(item => {

            if (item.comments && item.comments.trim()) {
                if (!item.goal.comment) {
                    item.goal.comment = <IRemark>{};
                }

                item.goal.comment.description = item.comments.trim();
            }
            else {
                item.goal.comment = null;
            }

            return item;
        }),
        saveHandler(this.messageService,
            mahGoal => {

                var item = this.goalItems.find(x => x.goal.goalId == mahGoal.goal.goalId);
                item = item == null ? mahGoal : item;


                let isRatingUpdate = mahGoal.goal.ratingValue !== mahGoal.ratingValue;
                mahGoal.goal.ratingValue = mahGoal.ratingValue;
                item.isLoading = true;

                return this.evalService.saveGoalEvaluation(mahGoal.goal).pipe(tap(data => {
                    if (this._evaluationDetail.isApprovalProcess) {
                        item.goal.activityFeed = data.activityFeed;
                        item.goal.ratingValue = data.ratingValue;
                        item.goal.comment = data.comment;
                    } else {
                        item.goal = data;
                    }

                    if (isRatingUpdate) {
                        item.isCommentRequired = this._evaluationDetail.goalRateCommentRequired.some(x => {
                            return x.reviewRating.rating === item.goal.ratingValue;
                        });
                    }

                    item.isLoading = false;

                    let idx = _.findIndex(this.evaluationDetail.goalEvaluations, x => x.goalId === data.goalId);
                    if (idx > -1) {
                        if (this._evaluationDetail.isApprovalProcess) {
                            this.evaluationDetail.goalEvaluations[idx].activityFeed = data.activityFeed;
                            this.evaluationDetail.goalEvaluations[idx].ratingValue = data.ratingValue;
                            this.evaluationDetail.goalEvaluations[idx].comment = data.comment;
                        } else {
                            this.evaluationDetail.goalEvaluations[idx] = data;
                        }
                        this.evaluationDetail.goalEvaluations[idx].isCommentRequired = item.isCommentRequired;

                    }

                    if (this.isOriginalEvaluator && item.isCommentRequired && item.isWithoutComment && item.approvalProcessStatusId == null) {
                        this.updateApprovalStatus(item, 0, 0)
                    }

                    this.refreshEvalStats();

                    if (!isRatingUpdate && !!data.ratingValue)
                        item.isActive = false;
                    else
                        this.focusGoalComment(idx, false, true);
                }),
                catchError((error: HttpErrorResponse) => {
                    this.messageService.showWebApiException(error.error);
                    item.isLoading = false;
                    return of(null);
                }))
            },
            'Save Goal',
            true))
    }

    saveGoalEvaluation(item: GoalEvalItem, idx: number) {

        const hasChangesAndIsComplete = item.hasChanges && !item.isWithoutComment && !!item.ratingValue;
        const hasChangesAndWasCleared = item.hasChanges && !item.isWithoutComment && !item.ratingValue
        if(hasChangesAndIsComplete || hasChangesAndWasCleared){
            this.saveGoalEval.next(item);
        }
    }

    focusGoalComment(idx: number, autoFocus: boolean, scrollTo = false) {
        let elem = this.goalComments.find((el, i) => i === idx);
        if (autoFocus)
            elem.nativeElement.focus();
        if (scrollTo) {
            if (this.goalCards.find((el, i) => i === idx).nativeElement) {
                this.goalCards.find((el, i) => i === idx).nativeElement.scrollIntoView();
            }
        }
    }

    removeGoalEvaluation(goal: IGoalEvaluation) {
        this.messageService.loading(true);
        this.evalService.removeGoalEvaluation(this.evaluationDetail.evaluationId, goal.goalId)
            .subscribe(data => {
                _.remove(this.evaluationDetail.goalEvaluations, g => g.goalId === goal.goalId);
                this.refreshGoalItems();
                this.messageService.loading(false);
            },
            (error: HttpErrorResponse) => {
                this.messageService.showWebApiException(error.error);
            });
    }

    // Hack to collect all the checked items
    updateMultiselectResponse(response: MultiSelectFeedbackResponse): void {
        response.value = response.feedbackItems.filter(x=>x.checked).map(y=>y.feedbackItemId).join();
        this.saveFeedbackResponse(response);
    }

    saveFeedbackResponse(response: IFeedbackResponse) {
        const isValid = !response.isRequired || (response.isRequired && response.hasResponse())
        const responseNoType = response as any;
        if(isValid && responseNoType.oldVal != responseNoType.value){
            this.saveFeedback.next(response);
        }
    }

    private areFeedbackEqual: (feedBackA: IFeedbackResponse, feedbackB: IFeedbackResponse) => boolean = (feedbackA: IFeedbackResponse, feedbackB: IFeedbackResponse) => {

        return feedbackA.feedbackId == feedbackB.feedbackId &&
        feedbackA.responseId == feedbackB.responseId &&
        feedbackA.feedbackBody == feedbackB.feedbackBody &&
        feedbackA.fieldType == feedbackB.fieldType &&
        feedbackA.isVisibleToEmployee == feedbackB.isVisibleToEmployee &&
        feedbackA.isRequired == feedbackB.isRequired &&
        feedbackA.orderIndex == feedbackB.orderIndex &&
        feedbackA.isActive == feedbackB.isActive &&
        feedbackA.activityFeed == feedbackB.activityFeed &&
        feedbackA.approvalProcessStatusId == feedbackB.approvalProcessStatusId &&
        feedbackA.isEditedByApprover == feedbackB.isEditedByApprover &&
        (feedbackA as any).value == (feedbackB as any).value &&

        new Maybe(feedbackA.responseByContact).map(x => x.userId).valueOr(-1) == new Maybe(feedbackB.responseByContact).map(x => x.userId).valueOr(-1);
    }

    createFeedbackResponseHandler: CreateSaver<IFeedbackResponse> = (source, saveHandler) => {
        return source.pipe(
            tap(response => this.isFeedbackLoading = true),
            map(response => {
                if (response instanceof TextItemFeedbackResponse && !response.hasResponse())
                    response.value = null;
                else if (response instanceof DateFeedbackResponse && response.hasResponse())
                    response.value = moment(response.value).format(DsApiCommonProvider.TimeFormat.DATE_ONLY);

                return response;
            }),
            saveHandler(this.messageService,
                (response) => {
                    return this.evalService.saveFeedbackResponse(this.evaluationDetail.evaluationId, response).pipe(
                        tap(data => {
                            let idx = _.findIndex(this.evaluationDetail.feedbackResponses, r => r.responseId == response.responseId);
                            if (this._evaluationDetail.isApprovalProcess) {
                                this.evaluationDetail.feedbackResponses[idx].activityFeed = data.activityFeed;
                            } else {
                                Object.assign(this.evaluationDetail.feedbackResponses[idx], data);
                            }

                            this.setOldValOnFeedback(this.evaluationDetail.feedbackResponses[idx]);

                            this.refreshEvalStats();
                            this.viewAbleResponses();
                            this.isFeedbackLoading = false;
                            this.isFeedbackPending = false;
                    }),
                    catchError((error: HttpErrorResponse) => {
                        this.messageService.showWebApiException(error.error);
                        return of(null);
                    }));
                },
                'Save feedback',
                true));
    }

    private setOldValOnFeedback(feedback: IFeedbackResponse): IFeedbackResponse {
        const feedbackNoType = feedback as any;
        feedbackNoType.oldVal = feedbackNoType.value;
        return feedback;
    }

    private refreshEvalContent() {
        this.evalStore.refreshCompetencyItems(this.evaluationDetail, this.competencyGroups, this.competencyItems, this.ratings);
        this.refreshEvalStats();
        this.refreshGoalItems();
        this.refreshFeedbackOrder();
        this._evaluationDetail.feedbackResponses.forEach(x => {
            this.setOldValOnFeedback(x);
         });
        this.refreshSelfReviewOnFeedbacks();
    }

    private refreshGoalItems() {
        this.goalItems = [];
        this.evaluationDetail.goalEvaluations.sort((a, b) => {
            return a.title > b.title ? 1 : -1;
        }).forEach(g => {
            let item = new GoalEvalItem(g, this.ratings);
            item.GoalRateCommentRequired = this.evaluationDetail.goalRateCommentRequired;
            this.goalItems.push(item);
        });

        this.refreshEvalStats();
    }

    private refreshFeedbackOrder() {
        if (this.evaluationDetail && this.evaluationDetail.feedbackResponses) {
            this.evaluationDetail.feedbackResponses.sort((f1, f2) => {
                let f1OrderIndex = f1.orderIndex === null ? 999 : f1.orderIndex;
                let f2OrderIndex = f2.orderIndex === null ? 999 : f2.orderIndex;

                if (f1OrderIndex === f2OrderIndex)
                    return f1.feedbackBody < f2.feedbackBody ? -1 : 1;

                return f1OrderIndex < f2OrderIndex ? -1 : 1;
            });
        }
    }

    private refreshCompetencyItems() {
        this.competencyItems = [];
        let idx = 0;
        let hasDefaultCompetencyGroup = false;
        this.evaluationDetail.competencyEvaluations.sort((a, b) => {
            let aName = (a.groupName || "zzzzz") + a.name;
            let bName = (b.groupName || "zzzzz") + b.name;

            return aName > bName ? 1 : -1;
        }).forEach(ce => {
            if (ce.groupName == null) {
                ce.groupName = "Competencies";
                hasDefaultCompetencyGroup = true;
            }
            if (this.competencyGroups.indexOf(ce.groupName) < 0) {
                this.competencyGroups.push(ce.groupName);
            }
            let item = new CompetencyEvalItem(ce, this.ratings);
            //if(idx === 0)
            //    item.isActive = true;

            this.competencyItems.push(item);
            idx++;
        });
        this.competencyGroups.sort();
        if (hasDefaultCompetencyGroup) {
            let tempArray = [];
            tempArray.push("Competencies");
            this.competencyGroups.forEach(c => {
                if (c != "Competencies") {
                    tempArray.push(c);
                }
            });
            this.competencyGroups = tempArray;
        }

        this.refreshEvalStats();
    }

    feedbackListItemCompare(item1: IFeedbackItem, item2: IFeedbackItem) {
        return item1 && item2 ? item1.feedbackItemId === item2.feedbackItemId : !!item1 === !!item2;
    }

    private continueEvaluation() {
        this.isSubmitted = true;
        const isFormValid = this.checkFormValidity();
        if (isFormValid) {
            this.evalStore.formIsValidAndComplete(true);

                this.evaluationDetail.meritIncreaseInfo.payrollRequestEffectiveDate = this.review.payrollRequestEffectiveDate;

                /** route to summary component */
                this.router.navigate(['../summary'], {relativeTo: this.activatedRoute});
        }

    }

    private checkFormValidity(): boolean {
        var weightsAreValid = !this.evaluationDetail.allowGoalWeightAssignment;

        if (this.evaluationDetail.allowGoalWeightAssignment) {
            var totalWeightPercent = 0;
            if (this.goalItems.length > 0) {
                this.goalItems.forEach(x => {
                    if (!x.goal.weight || x.goal.weight == 0) {
                        this.messageService.setTemporaryMessage("All goals must have a weight.", MessageTypes.error)
                        this.evalStore.formIsValidAndComplete(false);
                        return false;
                    }
                    totalWeightPercent += (100 * x.goal.weight)
                });
                weightsAreValid = Math.round(totalWeightPercent) == 10000;
            } else {
                weightsAreValid = true;
            }
        }
        if (!weightsAreValid) {
            this.messageService.setTemporaryMessage("Please make sure goal weights equal 100%.", MessageTypes.error);
            this.evalStore.formIsValidAndComplete(false);
            return false;
        }

        if (this.percentComplete !== 100) {
          this.isSubmitted = true;
            if (this.currSubmitState == this.submitEvalHandler) {
                this.messageService.setTemporaryMessage("Please complete the evaluation before submitting.", MessageTypes.error);
            } else {
                this.messageService.setTemporaryMessage("Please complete the evaluation before continuing.", MessageTypes.error);
            }
            this.evalStore.formIsValidAndComplete(false);
            return false;
        }

        for (let idx = 0; idx < this.competencyItems.length; idx++) {
            let comp = this.competencyItems[idx];
            if (!comp.isComplete) {
                this.selectCompetencyEvaluation(comp);
                setTimeout(() => this.focusCompComment(idx, true));
                if (this.currSubmitState == this.submitEvalHandler) {
                    this.messageService.setTemporaryMessage("Please complete the evaluation before submitting.", MessageTypes.error);
                } else {
                    this.messageService.setTemporaryMessage("Please complete the evaluation before continuing.", MessageTypes.error);
                }
                this.evalStore.formIsValidAndComplete(false);
                return false;
            }
        }

        for (let idx = 0; idx < this.goalItems.length; idx++) {
            let goal = this.goalItems[idx];
            if (!goal.isComplete) {
                this.selectGoalEvaluation(goal);
                setTimeout(() => this.focusGoalComment(idx, true));
                if (this.currSubmitState == this.submitEvalHandler) {
                    this.messageService.setTemporaryMessage("Please complete the evaluation before submitting.", MessageTypes.error);
                } else {
                    this.messageService.setTemporaryMessage("Please complete the evaluation before continuing.", MessageTypes.error);
                }
                this.evalStore.formIsValidAndComplete(false);
                return false;
            }
        }

       this.evalStore.formIsValidAndComplete(true);
       return true;
    }



    submitEvaluation() {
        const isFormValid = this.checkFormValidity();
        this.isSubmitted = true;
        if(isFormValid){
            this.evalSummaryDialog.open(
                this.evaluationDetail,
                this.review,
                this.overallScore
            ).afterClosed().subscribe(result => {
                if(result == null) return;
                if (this._evaluationDetail.isApprovalProcess && result.evaluation.approvalProcessAction !== ApprovalProcessHistoryAction.Finalized) {
                    this.appProcessSummaryDialog.open(
                        this.evaluationDetail,
                        this.review,
                        this.user
                    ).afterClosed().subscribe(result => {
                        this.returnLink(); // this.router.navigate(this.returnRouterLink);
                    });
                } else {
                    this.returnLink(); // this.router.navigate(this.returnRouterLink);
                }

            });
        }
    }

    returnLink() {
        let url: string = this.returnRouterLink[0];
        if (url.indexOf('aspx') !== -1) {
            location.href = url;
        } else {
            this.router.navigate(this.returnRouterLink);
        }
    }

    refreshSelfReviewOnFeedbacks(){
        if( this.isSupervisorEvaluation ){
            // default applies
        } else {
            // For self reviews the isVisibleToEmployee is always true
            this.evaluationDetail.feedbackResponses.forEach(x=>{
                x.isVisibleToEmployee = true;
            });
        }
    }
    viewAbleResponses()
    {
        this.viewableFeedbackResponses = [];
        this.evaluationDetail.feedbackResponses.forEach(x=>{
            var y = x.isVisibleToEmployee;
            if(this.review.reviewedEmployeeId == this.user.employeeId )
            {
                if(this.isUserEvaluator){
                    this.viewableFeedbackResponses.push(x);
                }else if(x.isVisibleToEmployee){
                    this.viewableFeedbackResponses.push(x);
                }
            }else{
                this.viewableFeedbackResponses.push(x);
            }
        });
    }

    //Turn comComplete into object of int complete int total string name
    private refreshEvalStats() {
        let latest = null;
        let totalCompsComplete = 0;
        let compComplete = 0;
        let totalInGroup = 0;
        let goalComplete = 0;
        let totalFeedbackComplete = 0;
        let requiredFeedbackComplete = 0;
        let requiredFeedbackCount = 0;

        this.competencyGroupStatus = [];
        this.competencyGroups.forEach(comGroup => {
            compComplete = 0;
            totalInGroup = 0;
            this.competencyItems.filter(f => f.competency.groupName == comGroup).forEach(x => {
                if (x.competency.modified) {
                    let m = moment(x.competency.modified);
                    if (!latest || m.isAfter(latest))
                        latest = m;
                }
                totalInGroup++;
                if (x.hasContent && (x.ratingValue != null) && ((x.hasComment && x.isCommentRequired) || !x.isCommentRequired)) {
                    totalCompsComplete++;
                    compComplete++;
                }
            });
            this.competencyGroupStatus.push(new CompetencyGroupItem(compComplete, totalInGroup, comGroup));
        });

        this.goalItems.forEach(x => {
            if (x.goal.modified) {
                let m = moment(x.goal.modified);
                if (!latest || m.isAfter(latest))
                    latest = m;
            }

            if (x.hasContent && ((x.hasComment && x.isCommentRequired) || !x.isCommentRequired))
                goalComplete++;
        });

        if (this.hasFeedback) {
            this.evaluationDetail.feedbackResponses.forEach(x => {
                if (x.isRequired) {
                    requiredFeedbackCount++;
                    if (x.hasResponse()) {
                        requiredFeedbackComplete++;
                    }
                }

                if (x.hasResponse()) {
                    totalFeedbackComplete++;
                }
            })
        }

        this.lastModified = latest;
        this.competenciesComplete = totalCompsComplete;
        this.goalsComplete = goalComplete;
        this.requiredFeedbackCount = requiredFeedbackCount;
        this.totalFeedbackComplete = totalFeedbackComplete;
        this.requiredFeedbackComplete = requiredFeedbackComplete;

        let totalSteps = this.competencyItems.length + this.goalItems.length + requiredFeedbackCount;
        let totalComplete = totalCompsComplete + goalComplete + requiredFeedbackComplete;

        // If there are no steps required to complete, mark as 100% complete.
        this.percentComplete = totalSteps ? (totalComplete / totalSteps) * 100.0 : 100.0;

        var weightsAreValid = !this.evaluationDetail.allowGoalWeightAssignment;

        var formIsValid = true;
        var allGoalsHaveWeight = true;
        if (this.evaluationDetail.allowGoalWeightAssignment) {
            var totalWeightPercent = 0;
            if (this.goalItems.length > 0) {
                this.goalItems.forEach(x => {
                    if (!x.goal.weight || x.goal.weight == 0) {
                        allGoalsHaveWeight = false;
                    }
                    if(allGoalsHaveWeight){
                        totalWeightPercent += (100 * x.goal.weight)
                    }
                });
                weightsAreValid = Math.round(totalWeightPercent) == 10000;
            } else {
                weightsAreValid = true;
            }
        }
        if (!weightsAreValid) {
            formIsValid = false;
        }

        formIsValid = weightsAreValid && this.percentComplete == 100 && allGoalsHaveWeight;
        this.evalStore.formIsValidAndComplete(formIsValid);

        this.evalStore.setPercentComplete(this.percentComplete);

        return latest;
    }

    // Approval Process Methods

    approvalProcessInit() {

        if (this.evaluationDetail.approvalProcessHistory != null) {
            this.evaluationDetail.approvalProcessAction = ApprovalProcessHistoryAction.Continued;

            if (this.evaluationDetail.approvalProcessHistory.length != 1) {
                const recentHistory = this.evaluationDetail.approvalProcessHistory[this.evaluationDetail.approvalProcessHistory.length - 1];

                if ((recentHistory.action === ApprovalProcessHistoryAction.Rejected) && recentHistory.toUserId == this.user.userId)
                    this.isOriginalEvaluator = true;
                if (recentHistory.action === ApprovalProcessHistoryAction.Finalized)
                    this.isAPFinalized = true;
                if (recentHistory.action === ApprovalProcessHistoryAction.Reopened)
                    this.isAPReopened = true;
                if (recentHistory.action === ApprovalProcessHistoryAction.Reassigned){
                    this.isAPReopened = true;
                    //Check if rejected before re-assignment
                    let previousAction = this.evaluationDetail.approvalProcessHistory[this.evaluationDetail.approvalProcessHistory.length - 2];
                    if (!!previousAction && previousAction.action === ApprovalProcessHistoryAction.Rejected && recentHistory.toUserId == this.user.userId) {
                        this.isOriginalEvaluator = true;
                        this.isAPReopened = false;
                    }
                }
                    
            } else {
                this.isAPFirstVisit = true
            }

        } else {
            this.approvalProcessWithoutHistory = true;
        }

        if (this.isOriginalEvaluator) {
            this.getSectionCountsForOriginalSupervisor();
            this.setSubmitStatus();
        } else {
            this.setApproveAllStatus();
            this.setSubmitStatusForApprover();
        }
    }

    approveAll() {

        this.evalService.approveAllEvaluationItems(this._evaluationDetail.evaluationId).subscribe(x => {
            this.competencyItems.forEach(x => {
                x.approvalProcessStatusId = ApprovalProcessStatus.Approved;
            });
            this.evaluationDetail.feedbackResponses.forEach(y => {
                y.approvalProcessStatusId = ApprovalProcessStatus.Approved;
            });
            this.goalItems.forEach(z => {
                z.approvalProcessStatusId = ApprovalProcessStatus.Approved;
            });
            this.setSubmitStatusForApprover();
        });

    }

    setApproveAllStatus() {

        if (this.disableApproveAll) return;

        let itemSet = this.competencyItems.find(x => (x.approvalProcessStatusId === ApprovalProcessStatus.Rejected || x.approvalProcessStatusId === ApprovalProcessStatus.Approved));

        if (itemSet) this.disableApproveAll = true;
        else {
            let itemSet2 = this.goalItems.find(x => (x.approvalProcessStatusId === ApprovalProcessStatus.Rejected || x.approvalProcessStatusId === ApprovalProcessStatus.Approved));
            if (itemSet2) this.disableApproveAll = true;
            else {
                let itemSet3 = this.evaluationDetail.feedbackResponses.find(x => (x.approvalProcessStatusId === ApprovalProcessStatus.Rejected || x.approvalProcessStatusId === ApprovalProcessStatus.Approved));
                if (itemSet3) this.disableApproveAll = true;
            }
        }
    }

    setSubmitStatus() {
        let itemSet = this.competencyItems.find(x => (x.approvalProcessStatusId === ApprovalProcessStatus.Rejected));

        if (itemSet) this.disableSubmit = true;
        else {
            let itemSet2 = this.goalItems.find(x => (x.approvalProcessStatusId === ApprovalProcessStatus.Rejected));
            if (itemSet2) this.disableSubmit = true;
            else {
                let itemSet3 = this.evaluationDetail.feedbackResponses.find(x => (x.approvalProcessStatusId === ApprovalProcessStatus.Rejected));
                if (itemSet3) {
                    this.disableSubmit = true;
                } else {
                    this.disableSubmit = false;
                }
            }
        }
    }

    setSubmitStatusForApprover() {
        let itemSet = this.competencyItems.find(x => (x.approvalProcessStatusId == null));

        if (itemSet) this.disableSubmit = true;
        else {
            let itemSet2 = this.goalItems.find(x => (x.approvalProcessStatusId == null));
            if (itemSet2) this.disableSubmit = true;
            else {
                let itemSet3 = this.evaluationDetail.feedbackResponses.find(x => (x.approvalProcessStatusId == null));
                if (itemSet3) {
                    this.disableSubmit = true;
                } else {
                    this.disableSubmit = false;
                }
            }
        }
    }

    getSectionCountsForOriginalSupervisor() {
        let appCompList = this.evaluationDetail.competencyEvaluations.filter(x => !x.isEditedByApprover && x.approvalProcessStatusId === ApprovalProcessStatus.Approved);
        let appGoalList = this.evaluationDetail.goalEvaluations.filter(x => !x.isEditedByApprover && x.approvalProcessStatusId === ApprovalProcessStatus.Approved);
        let appFebaList = this.evaluationDetail.feedbackResponses.filter(x => !x.isEditedByApprover && x.approvalProcessStatusId === ApprovalProcessStatus.Approved);

        this.approvedItems = appCompList.length + appGoalList.length + appFebaList.length;

        let appWECompList = this.evaluationDetail.competencyEvaluations.filter(x => x.isEditedByApprover && x.approvalProcessStatusId === ApprovalProcessStatus.Approved);
        let appWEGoalList = this.evaluationDetail.goalEvaluations.filter(x => x.isEditedByApprover && x.approvalProcessStatusId === ApprovalProcessStatus.Approved);
        let appWEFebaList = this.evaluationDetail.feedbackResponses.filter(x => x.isEditedByApprover && x.approvalProcessStatusId === ApprovalProcessStatus.Approved);

        this.approvedWithEditsItems = appWECompList.length + appWEGoalList.length + appWEFebaList.length;

        let rejCompList = this.evaluationDetail.competencyEvaluations.filter(x => x.approvalProcessStatusId === ApprovalProcessStatus.Rejected || x.approvalProcessStatusId == null);
        let rejGoalList = this.evaluationDetail.goalEvaluations.filter(x => x.approvalProcessStatusId === ApprovalProcessStatus.Rejected || x.approvalProcessStatusId == null);
        let rejFebaList = this.evaluationDetail.feedbackResponses.filter(x => x.approvalProcessStatusId === ApprovalProcessStatus.Rejected || x.approvalProcessStatusId == null);

        this.rejectedItems = rejCompList.length + rejGoalList.length + rejFebaList.length;
    }

    private updateApprovalStatusHandler(source: Observable<{item: any, val: number, type: number}>): Observable<any> {
        return source.pipe(map(items => {
            const item = items.item;

            if (items.val == -1) items.val = null;
        if (items.val == null && item.approvalProcessStatusId == null) {
            items.val = 0;
            item.approvalProcessStatusId = items.val;
        } else {
            item.approvalProcessStatusId = items.val;
        }

        item.competencyId ? this.competencyItems.forEach(x => { if (x.competencyId == item.competencyId) x.approvalProcessStatusId = items.val; })
                : item.responseId ? this.evaluationDetail.feedbackResponses.forEach(x => { if (x.responseId == item.responseId) x.approvalProcessStatusId = items.val; } )
                : this.goalItems.forEach(x => { if (x.goal.goalId == item.goal.goalId) x.goal.approvalProcessStatusId = items.val; });
        return items;
        }),
        tap(() => {
            if (this.isOriginalEvaluator) {
                this.setSubmitStatus();
            } else {
                this.setSubmitStatusForApprover();
            }
        }),
        exhaustMap(items => {
            this.disableApproveAll = true;
        let fk = items.item.competencyId ? items.item.competencyId : items.item.responseId ? items.item.responseId : items.item.goal.goalId;

        return this.evalService.approveEvaluationItem(this.evaluationDetail.evaluationId, items.type, fk, items.val);
        }))
    }

    updateApprovalStatus(item: any, val: number, type: number) {
        this.saveApprovalStatus.next({item: item, val: val, type: type});
    }

    approveStatusLabel(item: IApprovalProcessStatusIdAndIsEditedByApprover) {
        if (item.approvalProcessStatusId === ApprovalProcessStatus.Rejected)
            return "Needs Revision"

        if (item.approvalProcessStatusId === ApprovalProcessStatus.Approved)
            return "Approved"

        return "Select Status";
    }

    approvalColor(item: IApprovalProcessStatusIdAndIsEditedByApprover) {

        if (item.approvalProcessStatusId === ApprovalProcessStatus.Rejected)
            return "warning"

        if (item.approvalProcessStatusId === ApprovalProcessStatus.Approved)
            return "success"

        return "info";
    }

    approvalBtnClass(item: IApprovalProcessStatusIdAndIsEditedByApprover) {

        if (item.approvalProcessStatusId === ApprovalProcessStatus.Rejected)
            return "btn-warning"

        if (item.approvalProcessStatusId === ApprovalProcessStatus.Approved)
            return "btn-success"

        return "btn-info";
    }

    getRatingLabel(ratingValue:number){
        let rating = _.find(this.evaluationDetail.ratings, r => r.rating === ratingValue);
        return rating.label;
    }

    addRemark(item: (CompetencyEvalItem | IFeedbackResponse | IGoalEvaluation)) {

            if (item.activityFeed == null)
                item.activityFeed = [];
            item.activityFeed.unshift(
                {
                    remarkId: null,
                    description: null,
                    addedDate: moment(),
                    addedBy: this.user.userId,
                    isSystemGenerated: false,
                    user: <UserInfo> {
                        userId: this.user.userId,
                        firstName: this.user.userFirstName,
                        lastName: this.user.userLastName
                    },
                    editItem: true
                }
            );
    }

    enableRemarkEdit(r:any) {
        r.editItem=!r.editItem;
        this.remarkEditMode = r.editItem;
    }

    deleteRemark(item: any, remark:ViewRemark, index: number, type: number) {
        this.remarkEditMode = false;
        remark              = item.activityFeed[index];
        remark.editItem     = false;

        let fk = item.competencyId ? item.competencyId : item.responseId ? item.responseId : item.goal.goalId;
        this.evalService.removeRemark(remark, this.evaluationDetail.evaluationId, type, fk).subscribe(result => {
            item.activityFeed.splice(index, 1);
        });
    }

    updateExistingRemark(item: any, remark : ViewRemark, i: number, type: number) {
        this.remarkEditMode = false;
        if (remark.description == '' || remark.description == null) return;
        remark              = item.activityFeed[i];
        remark.editItem     = false;

        remark.addedDate = remark.addedDate ? moment(remark.addedDate).format(API_STRING) : moment().format(API_STRING);
        let fk = item.competencyId ? item.competencyId : item.responseId ? item.responseId : item.goal.goalId;

        this.evalService.saveRemark(remark, this.evaluationDetail.evaluationId, type, fk).subscribe(result => {
            item.activityFeed[i] = result;
        });

    }

    clearRemark(item: (CompetencyEvalItem | IFeedbackResponse | IGoalEvaluation), index:number):void {

            const remark = item.activityFeed[index];
            if(remark.remarkId) {
                remark.editItem = !remark.editItem;
                this.remarkEditMode = remark.remark.editItem;
                return;
            }

            item.activityFeed.splice(index, 1);
    }

}

interface SaveGoalService {
    SaveGoal(goals: any[]): Observable<any>;
}

interface SubmitState {
    text:string,
    handler:(realSubmit: boolean) => void
}

/**
 * An object with two different items we need to access when creating an auto-save handler for an item
 */
interface HandlerResult {
    /**
     * A function that provides custom auto-save throttling and automatically wraps the provided `effect` function in custom error handling
     */
    handler: SaveHandler;
    /**
     * An observable that handles changes to the loading status of an item
     */
    loadingStream$: Observable<any>;
};

interface IFeedbackResponseWithLoading extends IFeedbackResponse {
    isLoading: boolean;
    isActive: boolean;
}

type CreateSaver<T> = (source: Observable<T>, saveHandler: SaveHandler) => Observable<any>


