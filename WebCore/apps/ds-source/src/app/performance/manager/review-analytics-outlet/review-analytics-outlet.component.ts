import { Component, OnInit, OnDestroy } from '@angular/core';
import { IReviewGroupStatus, IReviewStatus, ReviewStatusType, IReviewStatusSearchOptions} from '@ds/performance/performance-manager';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { IEvaluation, EvaluationRoleType } from '@ds/performance/evaluations';
import { IReview } from '@ds/performance/reviews';
import { ReviewsService } from '@ds/performance/reviews/shared/reviews.service';
import { ActiveEvaluationService } from '@ds/performance/evaluations/shared/active-evaluation.service';
import { Router } from '@angular/router';
import { Subscription, of, Observable, Subject, ReplaySubject,BehaviorSubject, merge, forkJoin } from 'rxjs';
import { tap, filter, switchMap, concatMap, withLatestFrom, map } from 'rxjs/operators';
import { EmployeeSearchFilterType, IEmployeeSearchResultResponseData } from '@ajs/employee/search/shared/models';
import { EmployeeApiService } from '@ds/core/employees/shared/employee-api.service';
import { IEmployeeSearchFilter } from '@ajs/employee/search/shared/models';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PerformanceManagerService } from '@ds/performance/performance-manager/performance-manager.service'
import { INameVal } from '@ajs/labor/models/name-value.model';
import { AnimationKeyframesSequenceMetadata } from '@angular/animations';
import { ICompetencyModel } from '@ds/performance/competencies/shared/competency-model.model';
import { IReviewRating } from '@ds/performance/ratings/shared/review-rating.model';
import { AccountService } from '@ds/core/account.service';
import { PerformanceReviewsService } from '@ds/performance/shared/performance-reviews.service';
import { FeedbackApiService } from '@ds/performance/feedback/feedback-api.service';
import { IFeedbackSetup } from '@ds/performance/feedback/shared/feedback-setup.model';
import { ScoreModel } from '@ds/performance/ratings/shared/score-model.model';

@Component({
    selector: 'ds-review-analytics-outlet',
    templateUrl: './review-analytics-outlet.component.html',
    styleUrls: ['./review-analytics-outlet.component.scss']
})
export class ReviewAnalyticsOutletComponent implements OnInit {

    statuses = [ReviewStatusType.Closed];
    reviewGroups: IReviewGroupStatus[];
    clientId : number;

    searchOptions: IReviewStatusSearchOptions;
    searchOptions$: Observable<any>;

    csvDownloadLink: string;
    successMsg: Subject<string> = new ReplaySubject(1);
    private readonly updateReviews: Subject<any> = new Subject();
    competencyModels: ICompetencyModel[];
    allRatings: IReviewRating[];
    allFeedbackQuestions: IFeedbackSetup[];
    scoreModel: ScoreModel;
    isLoading: boolean;

    constructor(
        private account: AccountService,
        private manager: PerformanceManagerService,
        private perfService: PerformanceReviewsService,
        private msgSvc: DsMsgService,
        private activeEvaluationSvc: ActiveEvaluationService,
        private employeeService:EmployeeApiService,
        private reviewService:ReviewsService,
        private feedbackApiService:FeedbackApiService,
        private dialog: MatDialog,
        private router: Router) { }

    ngOnInit() {
        this.successMsg.next(null);

        // Performance header search subscription
        this.searchOptions$ = this.updateView().pipe(
            tap(options => {
                this.searchOptions = options || {};
           }),
           filter(options => options && options.reviewTemplateId > 0),
            switchMap(options => {
                //this.msgSvc.loading(true);
                this.isLoading = true;
                (<IReviewStatusSearchOptions>options).includeScores = true;
                return  this.getReviewStatuses(options).pipe(
                    withLatestFrom(this.successMsg),
                    tap(groups => {
                        this.reviewGroups = groups[0];
                        this.csvDownloadLink = this.manager.downloadReviewStatusCsvUrl(options);
                
                        if (groups[1])
                            this.msgSvc.setTemporarySuccessMessage(groups[1]);
                        else
                            //this.msgSvc.loading(false);
                            this.isLoading = false;
                    }));
            })
        );
        
        // Retrieve user information for clientId only
        this.account.getUserInfo().pipe(
            concatMap(u => {
                this.clientId = u.lastClientId || u.clientId;

                // Heavy lifting of master data happens here
                return forkJoin(
                    this.perfService.getCompetencyModelsForCurrentClient(),
                    this.perfService.getPerformanceReviewRatings(this.clientId),
                    this.feedbackApiService.getFeedbackSetup(this.clientId),
                    this.perfService.getScoreModelForCurrentClient());
            }),
            tap(x => {
                if(x && x[0] && x[1] && x[2] ){
                    // Master data retrived is assigned to our properties
                    this.competencyModels = x[0] || []; 
                    if(this.competencyModels.length === 0){
                        this.competencyModels.push(<ICompetencyModel>{competencyModelId: 0, clientId: this.clientId, name: 'none', competencies: null});
                    }
                    this.allRatings = x[1] || [];
                    this.allRatings.push( { reviewRatingId : 0, clientId : this.clientId, rating : 0, label : "Not Rated", description:"", client:null} );
                    this.allRatings.sort( (x,y) => y.rating - x.rating );

                    this.scoreModel = x[3].data;

                    // Feedback master data
                    this.allFeedbackQuestions = x[2] || [];
                }
            })).subscribe();
    }

    /**
     * Returns an observable that emits when the view should be updated.
     */
    private updateView(): Observable<IReviewStatusSearchOptions> {
        return merge(this.manager.activeReviewSearchOptions$,
            this.updateReviews.pipe(
                tap(() => this.successMsg.next("Review updated successfully.")),
                withLatestFrom(this.manager.activeReviewSearchOptions$),
                map(x => x[1])));
    }

    private readonly getReviewStatuses: (options: IReviewStatusSearchOptions) => Observable<IReviewGroupStatus[]> = (options: IReviewStatusSearchOptions) =>
        of(options).pipe(
            concatMap(options => this.manager.searchReviewStatus(options)));

    reviewChanged() {
        this.updateReviews.next();
    }

    evaluationSelected(event: {evaluation:IEvaluation, review:IReview}) {
        this.activeEvaluationSvc.setActiveEvaluation(event.evaluation, event.review);
        this.activeEvaluationSvc.returnUrl = ["/performance/manage/status"];
        this.router.navigate(['/performance/evaluations']);
    }
}
