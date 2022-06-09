import { Injectable } from '@angular/core';
import { IReview } from '@ds/performance/reviews';
import { BehaviorSubject, Observable, of, ReplaySubject, zip, Subject, Subscription, combineLatest } from 'rxjs';
import { map, switchMap, tap, share, withLatestFrom, debounceTime } from 'rxjs/operators';
import { coerceNumberProperty } from '@angular/cdk/coercion';
import { IEvaluation } from '../shared/evaluation.model';
import { IEvaluationDetail } from '../shared/evaluation-detail.model';
import { EvaluationRoleType } from '../shared/evaluation-role-type.enum';
import { EvaluationsApiService } from './evaluations-api.service';
import { UserInfo, UserType } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { IEvaluationWithStatusInfo } from './evaluation-status-info.model';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { CompetencyEvalItem } from './competency-eval-item';
import { IReviewRating } from '@ds/performance/ratings';
import { IMeritLimit } from './merit-limit.model';
import { Maybe } from '@ds/core/shared/Maybe';

const DEFAULT_URL = ['/performance/employees/reviews'];

export interface IEvaluationWithReview extends IEvaluation {
    review?: IReview
}

@Injectable({
    providedIn: 'root'
})
export class ActiveEvaluationService {

    private _user: UserInfo;
    private formValidAndComplete: Subject<boolean> = new BehaviorSubject(false);
    private readonly _canShowSummary$: Observable<{isFormValidAndComplete: boolean, isPayrollRequestEnabled: boolean}>
    /** This public getter was only necessary for an auth guard @see EvalSummaryGuard. If you need to call some functionality
     * in something other than an auth guard when this observable emits please use the @see onCanShowSummaryUpdate function or
     * add another function to this class using this observable. */
    get canShowSummary(){
        return this._canShowSummary$;
    }

    private _returnUrl:any[];
    get returnUrl():any[] {
        return this._returnUrl != null && this._returnUrl.length
            ? this._returnUrl
            : DEFAULT_URL;
    }
    set returnUrl(value:any[]) {
        if(!value || value == null)
            this._returnUrl = DEFAULT_URL;
        else
            this._returnUrl = value;
    }

    private _evaluation: IEvaluationWithReview;
    private evaluation = new BehaviorSubject<IEvaluation>(null);
    get evaluation$():Observable<IEvaluation> {
        return this.evaluation.asObservable();
    }
    private _review: IReview;
    private review = new BehaviorSubject<IReview>(null);
    get review$():Observable<IReview> {
        return this.review.asObservable();
    }

    private _evaluationDetail: IEvaluationWithStatusInfo;
    private evaluationDetail = new BehaviorSubject<IEvaluationWithStatusInfo>(null);
    get evaluationDetail$(): Observable<IEvaluationWithStatusInfo> {
        return this.evaluationDetail.asObservable();
    }

    private _percentComplete:number;
    private percentComplete = new ReplaySubject<number>(1);

    setPercentComplete(value:number) {
        this._percentComplete = coerceNumberProperty(value);
        this.percentComplete.next(this._percentComplete);
    }
    get percentComplete$():Observable<number> {
        return this.percentComplete.asObservable();
    }

    private isLoadingDetail = new BehaviorSubject(false);
    /**
     * Indicates whether the ActiveEvaluationService is still getting data for the selected evaluation
     */
    get isLoadingDetail$() {
        return this.isLoadingDetail.asObservable();
    }

    constructor(
        private evalSvc:EvaluationsApiService,
        private accountSvc: AccountService,
        private perfService:PerformanceReviewsService
    ) {
        this.evaluation.subscribe(next => this._evaluation = next);
        this.review.subscribe(next => this._review = next);
        this.evaluationDetail.subscribe(next => this._evaluationDetail = next);

        const isPayrollRequestEnabledForSelectedReview$ = this.review$.pipe( // need to get the scoring settings for each review
            switchMap(x => {
                return x ? this.perfService.getScoringSettings(x.reviewId).pipe(map(x => x.data.isPayrollRequestsEnabled)) : of(null);
            }));

        const combinedStreams$ =
            combineLatest(this.isFormValidAndComplete(), isPayrollRequestEnabledForSelectedReview$);

        this._canShowSummary$ = combinedStreams$.pipe(
            map(x => ({ isFormValidAndComplete: x[0], isPayrollRequestEnabled: x[1] }))
        );
    }

    setActiveEvaluation(evaluation: IEvaluation, review?: IReview) {
        this.evaluation.next(evaluation);
        this.review.next(review);

        //update detail
        this.isLoadingDetail.next(true);
        this.evaluationDetail.next(null);

        let user$ = this.accountSvc.getUserInfo();
        let detail$ = this.evalSvc.getEvaluationDetail(evaluation.evaluationId);

        zip(user$, detail$, (user, detail) => { return {user, detail} })
            .subscribe(result => {
                this._user = result.user;
                this.setEvaluationDetail(result.detail);
                this.isLoadingDetail.next(false);
            });
    }

    setEvaluationDetail(evaluation: IEvaluationDetail) {
        let status = this.setEvaluationStatusInfo(evaluation);
        this.evaluationDetail.next(status);
        return status;
    }

    setEvaluationStatusInfo(evaluationDetail: IEvaluationDetail, user?:UserInfo): IEvaluationWithStatusInfo {
        user = user || this._user;

        let status = <IEvaluationWithStatusInfo>evaluationDetail;

        status.goalsLength = evaluationDetail != null ? evaluationDetail.goalEvaluations.length : 0;
        status.hasCompetencies = evaluationDetail && evaluationDetail.competencyEvaluations && !!evaluationDetail.competencyEvaluations.length;
        status.hasGoals = evaluationDetail && evaluationDetail.goalEvaluations && !!evaluationDetail.goalEvaluations.length;
        status.hasFeedback = evaluationDetail && evaluationDetail.feedbackResponses && !!evaluationDetail.feedbackResponses.length;
        status.isEvalComplete = evaluationDetail && !!evaluationDetail.completedDate;
        status.isUserEvaluator = user && evaluationDetail && evaluationDetail.currentAssignedUserId == user.userId;
        status.isReadOnly = status.isEvalComplete || (user.userTypeId != UserType.systemAdmin && !status.isUserEvaluator);
        status.hasSummaryData = evaluationDetail && evaluationDetail.role === EvaluationRoleType.Manager && (status.isUserEvaluator || user.userTypeId == UserType.systemAdmin);

        return status;
    }

    findRecommendedIncrease(score: number, limits: IMeritLimit[]): number {
        const _score = score || 0;
        return new Maybe(limits).map(x => {
            for(var i = 0; i < x.length; i++){
                const item = x[i];
                if(_score > item.minScore && _score <= item.maxScore){
                    return item.meritPercent || 0;
                }
            }
            return 0;
        }).valueOr(0);
    }

    setActiveReview(review:IReview) {
        this.review.next(review);
    }

    getEvaluationTypeName(evaluation: IEvaluation) {
        switch (evaluation.role) {
            case EvaluationRoleType.Manager:
                return "Supervisor Evaluation";
            case EvaluationRoleType.Peer:
                return "Peer Evaluation";
            case EvaluationRoleType.Self:
                return "Self-evaluation";
        }
    }

    /**
     * When the status of whether the summary tab should be shown is updated.  Call the provided handler.
     * @param action When the status of the onCanShowSummary status is updated this function is invoked
     * @param observableToMerge$ Any other asyncronous events to be passed to the provided action
     */
    onCanShowSummaryUpdate<T>(
        action: (next: { isFormValidAndComplete: boolean, isPayrollRequestEnabled: boolean, otherResult?: boolean | T }) => void,
        observableToMerge$?: Observable<T>): Subscription {

        const nonNullObs$ = observableToMerge$ == null ? of(true) : observableToMerge$;

        return this._canShowSummary$.pipe(debounceTime(20) ,withLatestFrom(nonNullObs$), map(result => ({
            isFormValidAndComplete: result[0].isFormValidAndComplete,
            isPayrollRequestEnabled: result[0].isPayrollRequestEnabled,
            otherResult: result[1]
        }))).subscribe({next: action, complete: () => console.log('done!')});
    }

    /**
     * Used to determine whether the eval summary tab should show
     * @see eval-summary.guard.ts
     * @see evaluation-header.component.ts ngOnInit
     */
    formIsValidAndComplete(isValidAndComplete: boolean){
        this.formValidAndComplete.next(isValidAndComplete);
    }

    isFormValidAndComplete(): Observable<boolean> {
        return this.formValidAndComplete.asObservable();
    }

    refreshCompetencyItems(detail: IEvaluationWithStatusInfo,
         compGroups: string[],
          compItems,
          ratings: IReviewRating[]) {
        compItems.splice(0, compItems.length);
        let hasDefaultCompetencyGroup = false;
        detail.competencyEvaluations.sort((a, b) => {
            let aName = (a.groupName || "zzzzz") + a.name;
            let bName = (b.groupName || "zzzzz") + b.name;

            return aName > bName ? 1 : -1;
        }).forEach(ce => {
            if (ce.groupName == null) {
                ce.groupName = "Competencies";
                hasDefaultCompetencyGroup = true;
            }
            if (compGroups.indexOf(ce.groupName) < 0) {
                compGroups.push(ce.groupName);
            }
            let item = new CompetencyEvalItem(ce, ratings);
            item.CompetencyRateCommentRequired = detail.competencyRateCommentRequired;

            compItems.push(item);
        });
        this.sortAndIgnoreCase(compGroups);
        if (hasDefaultCompetencyGroup) {
            let tempArray = [];
            tempArray.push("Competencies");
            compGroups.forEach(c => {
                if (c != "Competencies") {
                    tempArray.push(c);
                }
            });
            this.sortAndIgnoreCase(tempArray);
            var index = tempArray.indexOf("Competencies");
            var item = tempArray.splice(index, 1);
            tempArray.unshift(item[0]);

            compGroups.splice(0, compGroups.length);
            tempArray.forEach(c => {
                compGroups.push(c);
            });
        }
    }

    private sortAndIgnoreCase(array: string[]): void {
        array.map(x => [x, (x || '').toLowerCase()]).sort((a, b) => a[1].localeCompare(b[1])).map(x => x[0]).forEach((x, index) => array[index] = x);
    }

    filterItemsOfType(group, compItems: CompetencyEvalItem[]) {
        return compItems.filter(x => x.competency.groupName == group);
    }
}

